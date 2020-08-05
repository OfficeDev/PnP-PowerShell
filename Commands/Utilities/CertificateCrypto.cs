using System;
using System.IO;
using System.Security.Cryptography;

namespace PnP.PowerShell.Commands.Utilities
{
    internal class CertificateCrypto
    {
        /// <summary>
        ///     This helper function parses an RSA private key using the ASN.1 format
        /// </summary>
        /// <param name="privateKeyBytes">Byte array containing PEM string of private key.</param>
        /// <returns>
        ///     An instance of <see cref="RSACryptoServiceProvider" /> rapresenting the requested private key.
        ///     Null if method fails on retriving the key.
        /// </returns>
        public static RSACryptoServiceProvider DecodeRsaPrivateKey(byte[] privateKeyBytes)
        {
            var ms = new MemoryStream(privateKeyBytes);
            var rd = new BinaryReader(ms);

            try
            {
                ushort shortValue = rd.ReadUInt16();

                switch (shortValue)
                {
                    case 0x8130:
                        // If true, data is little endian since the proper logical seq is 0x30 0x81
                        rd.ReadByte(); //advance 1 byte
                        break;
                    case 0x8230:
                        rd.ReadInt16(); //advance 2 bytes
                        break;
                    default:
                        return null;
                }

                shortValue = rd.ReadUInt16();
                if (shortValue != 0x0102) // (version number)
                    return null;

                byte byteValue = rd.ReadByte();
                if (byteValue != 0x00)
                    return null;

                // The data following the version will be the ASN.1 data itself, which in our case
                // are a sequence of integers.

                var parms = new CspParameters
                {
                    Flags = CspProviderFlags.UseMachineKeyStore,
                    KeyContainerName = Guid.NewGuid().ToString().ToUpperInvariant()
                };

                var rsa = new RSACryptoServiceProvider(parms);
                var rsAparams = new RSAParameters { Modulus = rd.ReadBytes(DecodeIntegerSize(rd)) };


                // Argh, this is a pain.  From empirical testing it appears to be that RSAParameters doesn't like byte buffers that
                // have their leading zeros removed.  The RFC doesn't address this area that I can see, so it's hard to say that this
                // is a bug, but it sure would be helpful if it allowed that. So, there's some extra code here that knows what the
                // sizes of the various components are supposed to be.  Using these sizes we can ensure the buffer sizes are exactly
                // what the RSAParameters expect.  Thanks, Microsoft.
                var traits = new RSAParameterTraits(rsAparams.Modulus.Length * 8);

                rsAparams.Modulus = AlignBytes(rsAparams.Modulus, traits.SizeMod);
                rsAparams.Exponent = AlignBytes(rd.ReadBytes(DecodeIntegerSize(rd)), traits.SizeExp);
                rsAparams.D = AlignBytes(rd.ReadBytes(DecodeIntegerSize(rd)), traits.SizeD);
                rsAparams.P = AlignBytes(rd.ReadBytes(DecodeIntegerSize(rd)), traits.SizeP);
                rsAparams.Q = AlignBytes(rd.ReadBytes(DecodeIntegerSize(rd)), traits.SizeQ);
                rsAparams.DP = AlignBytes(rd.ReadBytes(DecodeIntegerSize(rd)), traits.SizeDp);
                rsAparams.DQ = AlignBytes(rd.ReadBytes(DecodeIntegerSize(rd)), traits.SizeDq);
                rsAparams.InverseQ = AlignBytes(rd.ReadBytes(DecodeIntegerSize(rd)), traits.SizeInvQ);

                rsa.ImportParameters(rsAparams);
                return rsa;
            }
            //catch (Exception e)
            //{
            //    return null;
            //}
            finally
            {
                rd.Close();
            }
        }

        private static byte[] AlignBytes(byte[] inputBytes, int alignSize)
        {
            int inputBytesSize = inputBytes.Length;

            if (alignSize != -1 && inputBytesSize < alignSize)
            {
                var buf = new byte[alignSize];
                for (var i = 0; i < inputBytesSize; ++i)
                    buf[i + (alignSize - inputBytesSize)] = inputBytes[i];
                return buf;
            }
            return inputBytes; // Already aligned, or doesn't need alignment
        }

        private static int DecodeIntegerSize(BinaryReader rd)
        {
            byte byteValue;
            int count;

            byteValue = rd.ReadByte();
            if (byteValue != 0x02) // indicates an ASN.1 integer value follows
                return 0;

            byteValue = rd.ReadByte();
            if (byteValue == 0x81)
            {
                count = rd.ReadByte(); // data size is the following byte
            }
            else if (byteValue == 0x82)
            {
                byte hi = rd.ReadByte(); // data size in next 2 bytes
                byte lo = rd.ReadByte();
                count = BitConverter.ToUInt16(new[] { lo, hi }, 0);
            }
            else
            {
                count = byteValue; // we already have the data size
            }

            //remove high order zeros in data
            while (rd.ReadByte() == 0x00)
                count -= 1;
            rd.BaseStream.Seek(-1, SeekOrigin.Current);

            return count;
        }
    }

    internal class RSAParameterTraits
    {
        public int SizeD = -1;
        public int SizeDp = -1;
        public int SizeDq = -1;
        public int SizeExp = -1;
        public int SizeInvQ = -1;

        public int SizeMod = -1;
        public int SizeP = -1;
        public int SizeQ = -1;

        public RSAParameterTraits(int modulusLengthInBits)
        {
            // The modulus length is supposed to be one of the common lengths, which is the commonly referred to strength of the key,
            // like 1024 bit, 2048 bit, etc.  It might be a few bits off though, since if the modulus has leading zeros it could show
            // up as 1016 bits or something like that.
            int assumedLength;
            double logbase = Math.Log(modulusLengthInBits, 2);
            if (logbase == (int)logbase)
            {
                // It's already an even power of 2
                assumedLength = modulusLengthInBits;
            }
            else
            {
                // It's not an even power of 2, so round it up to the nearest power of 2.
                assumedLength = (int)(logbase + 1.0);
                assumedLength = (int)Math.Pow(2, assumedLength);
                // you should verify that this really does the 'right' thing!
            }

            switch (assumedLength)
            {
                case 1024:
                    SizeMod = 0x80;
                    SizeExp = -1;
                    SizeD = 0x80;
                    SizeP = 0x40;
                    SizeQ = 0x40;
                    SizeDp = 0x40;
                    SizeDq = 0x40;
                    SizeInvQ = 0x40;
                    break;
                case 2048:
                    SizeMod = 0x100;
                    SizeExp = -1;
                    SizeD = 0x100;
                    SizeP = 0x80;
                    SizeQ = 0x80;
                    SizeDp = 0x80;
                    SizeDq = 0x80;
                    SizeInvQ = 0x80;
                    break;
                case 4096:
                    SizeMod = 0x200;
                    SizeExp = -1;
                    SizeD = 0x200;
                    SizeP = 0x100;
                    SizeQ = 0x100;
                    SizeDp = 0x100;
                    SizeDq = 0x100;
                    SizeInvQ = 0x100;
                    break;
            }
        }
    }
}