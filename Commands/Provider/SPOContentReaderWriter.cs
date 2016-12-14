using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation.Provider;
using Microsoft.SharePoint.Client;
using File = Microsoft.SharePoint.Client.File;

namespace SharePointPnP.PowerShell.Commands.Provider
{
    internal class SPOContentReaderWriter : IContentReader, IContentWriter
    {
        //Private properties
        private readonly File _file;
        private MemoryStream _stream;
        private StreamReader _streamReader;
        private StreamWriter _streamWriter;
        private readonly bool _isBinary;

        public SPOContentReaderWriter(File file, bool isBinary)
        {
            _file = file;
            _isBinary = isBinary;
            _stream = new MemoryStream();

            var spStream = _file.OpenBinaryStream();
            _file.Context.ExecuteQuery();
            spStream.Value.CopyTo(_stream);
            _stream.Position = 0;

            _streamWriter = new StreamWriter(_stream);
            _streamReader = new StreamReader(_stream);
        }

        public void Seek(long offset, SeekOrigin origin)
        {
            _stream.Seek(offset, origin);
        }

        public IList Write(IList content)
        {
            if (!_isBinary)
            {
                foreach (string str in content)
                {
                    _streamWriter.WriteLine(str);
                }
            }
            else
            {
                foreach (var obj in content)
                {
                    if (obj is byte)
                    {
                        _stream.WriteByte((byte)obj);
                    }
                    else if (obj is char)
                    {
                        _stream.WriteByte(Convert.ToByte((char)obj));
                    }
                }
            }

            _streamWriter.Flush();
            _stream.Position = 0;
            File.SaveBinaryDirect((_file.Context as ClientContext), _file.ServerRelativeUrl, _stream, true);
            return content;
        }

        public IList Read(long readCount)
        {
            var list = new List<object>();
            long counter = 0;
            if (!_isBinary)
            {
                while (!_streamReader.EndOfStream && (counter < readCount || readCount < 1))
                {
                    list.Add(_streamReader.ReadLine());
                    counter++;
                }

            }
            else
            {
                while (counter < readCount || readCount < 1)
                {
                    var value = _stream.ReadByte();
                    if (value == -1) break;

                    list.Add((byte)value);
                    counter++;
                }
            }
            return list;

        }

        public void Close()
        {
            if (_streamReader != null)
            {
                _streamReader.Close();
            }

            if (_streamWriter != null)
            {
                _streamWriter.Close();
            }
        }

        public void Dispose()
        {
            if (_streamReader != null)
            {
                _streamReader.Dispose();
                _streamReader = null;
            }

            if (_streamWriter != null)
            {
                _streamWriter.Dispose();
                _streamWriter = null;
            }

            if (_stream != null)
            {
                _stream.Dispose();
                _stream = null;
            }
        }
    }
}