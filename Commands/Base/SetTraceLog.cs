using PnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Diagnostics;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsCommon.Set, "PnPTraceLog")]
    [CmdletHelp("Turn log tracing on or off",
        "Defines if tracing should be turned on. PnP Core, which is the foundation of these cmdlets, uses the standard Trace functionality of .NET. With this cmdlet you can turn capturing of this trace to a log file on or off. Notice that basically only the Provisioning Engine writes to the tracelog which means that cmdlets related to the engine will produce output.",
        Category = CmdletHelpCategory.Base)]
    [CmdletExample(
        Code = @"PS:> Set-PnPTraceLog -On -LogFile traceoutput.txt",
        Remarks = @"This turns on trace logging to the file 'traceoutput.txt' and will capture events of at least 'Information' level.",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Set-PnPTraceLog -On -LogFile traceoutput.txt -Level Debug",
        Remarks = @"This turns on trace logging to the file 'traceoutput.txt' and will capture debug events.",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Set-PnPTraceLog -On -LogFile traceoutput.txt -Level Debug -Delimiter "",""",
        Remarks = @"This turns on trace logging to the file 'traceoutput.txt' and will write the entries as comma separated. Debug events are captured.",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Set-PnPTraceLog -Off",
        Remarks = @"This turns off trace logging. It will flush any remaining messages to the log file.",
        SortOrder = 3)]
    public class SetTraceLog : PSCmdlet
    {
        [Parameter(Mandatory = true, ParameterSetName = "On", HelpMessage = "Turn on tracing to log file")]
        public SwitchParameter On;

        [Parameter(Mandatory = false, ParameterSetName = "On", HelpMessage = "The path and filename of the file to write the trace log to.")]
        public string LogFile;

        [Parameter(Mandatory = false, ParameterSetName = "On", HelpMessage = "Turn on console trace output.")]
        public SwitchParameter WriteToConsole;

        [Parameter(Mandatory = false, ParameterSetName = "On", HelpMessage = "The level of events to capture. Possible values are 'Debug', 'Error', 'Warning', 'Information'. Defaults to 'Information'.")]
        public OfficeDevPnP.Core.Diagnostics.LogLevel Level = OfficeDevPnP.Core.Diagnostics.LogLevel.Information;

        [Parameter(Mandatory = false, ParameterSetName = "On", HelpMessage = "If specified the trace log entries will be delimited with this value.")]
        public string Delimiter;

        [Parameter(Mandatory = false, ParameterSetName = "On", HelpMessage = "Indents in the tracelog will be with this amount of characters. Defaults to 4.")]
        public int IndentSize = 4;

        [Parameter(Mandatory = false, ParameterSetName = "On", HelpMessage = "Auto flush the trace log. Defaults to true.")]
        public bool AutoFlush = true;

        [Parameter(Mandatory = true, ParameterSetName = "Off", HelpMessage = "Turn off tracing to log file.")]
        public SwitchParameter Off;

        private const string FileListenername = "PNPPOWERSHELLFILETRACELISTENER";
        private const string ConsoleListenername = "PNPPOWERSHELLCONSOLETRACELISTENER";
        protected override void ProcessRecord()
        {

            if (ParameterSetName == "On")
            {
                // Setup Console Listener if Console switch has been specified or No file LogFile parameter has been set
                if (WriteToConsole.IsPresent || string.IsNullOrEmpty(LogFile))
                {
                    RemoveListener(ConsoleListenername);
#if !PNPPSCORE
                    ConsoleTraceListener consoleListener = new ConsoleTraceListener(false);
                    consoleListener.Name = ConsoleListenername;
                    Trace.Listeners.Add(consoleListener);
                    OfficeDevPnP.Core.Diagnostics.Log.LogLevel = Level;
#else
                    WriteWarning("Console logging not supported");
#endif
                }

                // Setup File Listener
                if (!string.IsNullOrEmpty(LogFile))
                {
                    RemoveListener(FileListenername);

                    if (!System.IO.Path.IsPathRooted(LogFile))
                    {
                        LogFile = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, LogFile);
                    }
                    // Create DelimitedListTraceListener in case Delimiter parameter has been specified, if not create TextWritterTraceListener
                    TraceListener listener = !string.IsNullOrEmpty(Delimiter) ?
                        new DelimitedListTraceListener(LogFile) { Delimiter = Delimiter, TraceOutputOptions = TraceOptions.DateTime } :
                        new TextWriterTraceListener(LogFile);

                    listener.Name = FileListenername;
                    Trace.Listeners.Add(listener);
                    OfficeDevPnP.Core.Diagnostics.Log.LogLevel = Level;
                }

                Trace.AutoFlush = AutoFlush;
                Trace.IndentSize = IndentSize;
            }
            else
            {
                Trace.Flush();
                RemoveListener(ConsoleListenername);
                RemoveListener(FileListenername);
            }
        }

        private void RemoveListener(string listenerName)
        {
            try
            {
                var existingListener = Trace.Listeners[listenerName];
                if (existingListener != null)
                {
                    existingListener.Flush();
                    existingListener.Close();
                    Trace.Listeners.Remove(existingListener);
                }
            }
            catch (Exception)
            {
                // ignored
            }
            
        }
    }
}