namespace PnP.PowerShell.Commands.Model
{
    /// <summary>
    /// Name/value parameter of a recorded event in the Office 365 Management API
    /// </summary>
    public class ManagementApiUnifiedLogRecordKeyValuePair
    {
        /// <summary>
        /// The name of the parameter
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The value of the parameter
        /// </summary>
        public string Value { get; set; }
    }
}
