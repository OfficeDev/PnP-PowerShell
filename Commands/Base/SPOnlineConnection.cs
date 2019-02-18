using Microsoft.ApplicationInsights;
using Microsoft.Identity.Client;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json;
using SharePointPnP.PowerShell.Commands.Enums;
using SharePointPnP.PowerShell.Commands.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Net.Http;
using System.Reflection;
using System.Web;

namespace SharePointPnP.PowerShell.Commands.Base
{
    public class SPOnlineConnection
    {
        internal static string DeviceLoginAppId = "31359c7f-bd7e-475c-86db-fdb8c937548e";

        internal Assembly coreAssembly;
        internal string userAgent;
        internal ConnectionMethod ConnectionMethod { get; set; }
        internal string PnPVersionTag { get; set; }
        internal static List<ClientContext> ContextCache { get; set; }

        public static AuthenticationResult AuthenticationResult { get; set; }
        public static TokenResult TokenResult { get; set; }
        public static SPOnlineConnection CurrentConnection { get; internal set; }
        public ConnectionType ConnectionType { get; protected set; }
        public int MinimalHealthScore { get; protected set; }
        public int RetryCount { get; protected set; }
        public int RetryWait { get; protected set; }
        public PSCredential PSCredential { get; protected set; }

        public TelemetryClient TelemetryClient { get; set; }

        public string Url { get; protected set; }

        public string TenantAdminUrl { get; protected set; }

        public ClientContext Context { get; set; }
        internal string AccessToken
        {
            get
            {
                if (!string.IsNullOrEmpty(TokenResult.AccessToken) && DateTime.Now > TokenResult.ExpiresOn && !string.IsNullOrEmpty(TokenResult.RefreshToken))
                {
                    // Expired token
                    var client = new HttpClient();
                    var uri = new Uri(Url);
                    var url = $"{uri.Scheme}://{uri.Host}";
                    var body = new StringContent($"resource={url}&client_id={DeviceLoginAppId}&grant_type=refresh_token&refresh_token={TokenResult.RefreshToken}");
                    body.Headers.ContentType.MediaType = "application/x-www-form-urlencoded";
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var result = client.PostAsync("https://login.microsoftonline.com/common/oauth2/token", body).GetAwaiter().GetResult();
                    var tokens = JsonConvert.DeserializeObject<Dictionary<string, string>>(result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                    TokenResult.AccessToken = tokens["access_token"];
                    TokenResult.RefreshToken = tokens["refresh_token"];
                    TokenResult.ExpiresOn = DateTime.Now.AddSeconds(int.Parse(tokens["expires_in"]));
                }
                return TokenResult.AccessToken;
            }
            set
            {
                if (TokenResult != null)
                {
                    TokenResult.AccessToken = value;
                }
            }
        }

        public SPOnlineConnection(ClientContext context, ConnectionType connectionType, int minimalHealthScore, int retryCount, int retryWait, PSCredential credential, string url, string tenantAdminUrl, string pnpVersionTag, System.Management.Automation.Host.PSHost host, bool disableTelemetry)
        {
            if (!disableTelemetry)
            {
                InitializeTelemetry(context, host);
            }
            var coreAssembly = Assembly.GetExecutingAssembly();
            userAgent = $"NONISV|SharePointPnP|PnPPS/{((AssemblyFileVersionAttribute)coreAssembly.GetCustomAttribute(typeof(AssemblyFileVersionAttribute))).Version}";
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            Context = context;
            Context.ExecutingWebRequest += Context_ExecutingWebRequest;
            ConnectionType = connectionType;
            MinimalHealthScore = minimalHealthScore;
            RetryCount = retryCount;
            RetryWait = retryWait;
            PSCredential = credential;
            TenantAdminUrl = tenantAdminUrl;
            ContextCache = new List<ClientContext> { context };
            PnPVersionTag = pnpVersionTag;
            Url = (new Uri(url)).AbsoluteUri;
            ConnectionMethod = ConnectionMethod.Credentials;
        }

        public SPOnlineConnection(ClientContext context, TokenResult tokenResult, ConnectionType connectionType, int minimalHealthScore, int retryCount, int retryWait, PSCredential credential, string url, string tenantAdminUrl, string pnpVersionTag, PSHost host, bool disableTelemetry)
        {
            if (!disableTelemetry)
            {
                InitializeTelemetry(context, host);
            }

            if (context == null)
                throw new ArgumentNullException(nameof(context));
            TokenResult = tokenResult;
            var coreAssembly = Assembly.GetExecutingAssembly();
            userAgent = $"NONISV|SharePointPnP|PnPPS/{((AssemblyFileVersionAttribute)coreAssembly.GetCustomAttribute(typeof(AssemblyFileVersionAttribute))).Version}";
            Context = context;
            ConnectionType = connectionType;
            MinimalHealthScore = minimalHealthScore;
            RetryCount = retryCount;
            RetryWait = retryWait;
            PSCredential = credential;
            TenantAdminUrl = tenantAdminUrl;
            ContextCache = new List<ClientContext> { context };
            PnPVersionTag = pnpVersionTag;
            Url = (new Uri(url)).AbsoluteUri;
            ConnectionMethod = ConnectionMethod.AccessToken;
            context.ExecutingWebRequest += (sender, args) =>
            {
                args.WebRequestExecutor.WebRequest.UserAgent = userAgent;
                args.WebRequestExecutor.RequestHeaders["Authorization"] = "Bearer " + CurrentConnection.AccessToken;
            };
        }


        public SPOnlineConnection(TokenResult tokenResult, ConnectionMethod connectionMethod, ConnectionType connectionType, int minimalHealthScore, int retryCount, int retryWait, string pnpVersionTag, PSHost host, bool disableTelemetry)
        {
            if (!disableTelemetry)
            {
                InitializeTelemetry(null, host);
            }
            TokenResult = tokenResult;
            var coreAssembly = Assembly.GetExecutingAssembly();
            userAgent = $"NONISV|SharePointPnP|PnPPS/{((AssemblyFileVersionAttribute)coreAssembly.GetCustomAttribute(typeof(AssemblyFileVersionAttribute))).Version}";
            ConnectionType = connectionType;
            MinimalHealthScore = minimalHealthScore;
            RetryCount = retryCount;
            RetryWait = retryWait;
            PnPVersionTag = pnpVersionTag;
            ConnectionMethod = ConnectionMethod;
        }


        private void Context_ExecutingWebRequest(object sender, WebRequestEventArgs e)
        {
            e.WebRequestExecutor.WebRequest.UserAgent = userAgent;
        }

        public void RestoreCachedContext(string url)
        {
            Context = ContextCache.FirstOrDefault(c => HttpUtility.UrlEncode(c.Url) == HttpUtility.UrlEncode(url));
        }

        internal void CacheContext()
        {
            var c = ContextCache.FirstOrDefault(cc => HttpUtility.UrlEncode(cc.Url) == HttpUtility.UrlEncode(Context.Url));
            if (c == null)
            {
                ContextCache.Add(Context);
            }
        }

        public ClientContext CloneContext(string url)
        {
            var context = ContextCache.FirstOrDefault(c => HttpUtility.UrlEncode(c.Url) == HttpUtility.UrlEncode(url));
            if (context == null)
            {
                context = Context.Clone(url);
                ContextCache.Add(context);
            }
            Context = context;
            return context;
        }

        internal static ClientContext GetCachedContext(string url)
        {
            return ContextCache.FirstOrDefault(c => HttpUtility.UrlEncode(c.Url) == HttpUtility.UrlEncode(url));
        }

        internal static void ClearContextCache()
        {
            ContextCache.Clear();
        }

        internal void InitializeTelemetry(ClientContext context, PSHost host)
        {

            var enableTelemetry = false;
            var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var telemetryFile = System.IO.Path.Combine(userProfile, ".pnppowershelltelemetry");

            if (!System.IO.File.Exists(telemetryFile))
            {
#if ONPREMISES
                if (Environment.UserInteractive && Environment.GetCommandLineArgs().FirstOrDefault(a => a.ToLower().StartsWith("-noni")) == null)
                {
                    var choices = new System.Collections.ObjectModel.Collection<ChoiceDescription>();
                    choices.Add(new ChoiceDescription("&Allow", "You will allow us to transmit anonymous data"));
                    choices.Add(new ChoiceDescription("&Do not allow", "You do not allow us to transmit anonymous data"));
                    if (host.UI.PromptForChoice("PnP PowerShell Telemetry", $"Please allow us to transmit anonymous metrics in order to make PnP PowerShell even better.{Environment.NewLine}We transmit the version of PnP PowerShell you are using, the version of SharePoint you are connecting to and which cmdlet you are executing. We do not transmit tenant/server URLs nor parameter values and content.{Environment.NewLine}{Environment.NewLine}Your decision will be recorded in a file a called .pnppowershelltelemetry which will be located in your profile folder ({userProfile}).{Environment.NewLine}{Environment.NewLine}You can choose to disable and/or enable telemetry at a later stage by using Enable-PnPPowerShellTelemetry or Disable-PnPPowerShellTelemetry. Get-PnPPowerShellTelemetryEnabled will provide you with your current setting.", choices, 0) == 0)
                    {
                        enableTelemetry = true;
                        System.IO.File.WriteAllText(telemetryFile, "allow");
                    }
                    else
                    {
                        System.IO.File.WriteAllText(telemetryFile, "disallow");
                    }
                }
#else
                enableTelemetry = true;
#endif

            }
            else
            {
                if (System.IO.File.ReadAllText(telemetryFile).ToLower() == "allow")
                {
                    enableTelemetry = true;
                }
            }
            if (enableTelemetry)
            {
                var serverLibraryVersion = "";
                var serverVersion = "";
                if (context != null)
                {
                    try
                    {
                        if (context.ServerLibraryVersion != null)
                        {
                            serverLibraryVersion = context.ServerLibraryVersion.ToString();
                        }
                        if (context.ServerVersion != null)
                        {
                            serverVersion = context.ServerVersion.ToString();
                        }
                    }
                    catch { }
                }
                TelemetryClient = new TelemetryClient();
                TelemetryClient.InstrumentationKey = "a301024a-9e21-4273-aca5-18d0ef5d80fb";
                TelemetryClient.Context.Session.Id = Guid.NewGuid().ToString();
                TelemetryClient.Context.Cloud.RoleInstance = "PnPPowerShell";
                TelemetryClient.Context.Device.OperatingSystem = Environment.OSVersion.ToString();
                TelemetryClient.Context.Properties.Add("ServerLibraryVersion", serverLibraryVersion);
                TelemetryClient.Context.Properties.Add("ServerVersion", serverVersion);

                var coreAssembly = Assembly.GetExecutingAssembly();

                TelemetryClient.Context.Properties.Add("Version", ((AssemblyFileVersionAttribute)coreAssembly.GetCustomAttribute(typeof(AssemblyFileVersionAttribute))).Version.ToString());
#if SP2013
            TelemetryClient.Context.Properties.Add("Platform", "SP2013");
#elif SP2016
            TelemetryClient.Context.Properties.Add("Platform", "SP2016");
#elif SP2019
            TelemetryClient.Context.Properties.Add("Platform", "SP2019");
#else
                TelemetryClient.Context.Properties.Add("Platform", "SPO");
#endif
                TelemetryClient.TrackEvent("Connect-PnPOnline");
            }
        }
    }
}
