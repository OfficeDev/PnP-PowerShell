using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json;

namespace SharePointPnP.PowerShell.Commands.Utilities.REST
{
    internal static class RestHelper
    {
        public static T ExecuteGetRequest<T>(ClientContext context, string url, string select = null, string filter = null, string expand = null, uint? top = null)
        {
            var returnValue = ExecuteGetRequest(context, url, select, filter, expand);

            var returnObject = JsonConvert.DeserializeObject<T>(returnValue);
            return returnObject;
        }

        public static string ExecuteGetRequest(ClientContext context, string endPointUrl, string select = null, string filter = null, string expand = null, uint? top = null)
        {
            var url = endPointUrl;
            if (!url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                url = context.Url + "/_api/" + endPointUrl;
            }
            var restparams = new System.Collections.Generic.List<string>();
            if (!string.IsNullOrEmpty(select))
            {
                restparams.Add($"$select={select}");
            }
            if (!string.IsNullOrEmpty(filter))
            {
                restparams.Add($"$filter={filter}");
            }
            if (!string.IsNullOrEmpty(expand))
            {
                restparams.Add($"$expand={expand}");
            }
            if (top.HasValue)
            {
                restparams.Add($"$top={top}");
            }
            if (restparams.Any())
            {
                url += $"?{string.Join("&", restparams)}";
            }
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", context.GetAccessToken());
            var returnValue = client.GetStringAsync(url).GetAwaiter().GetResult();
            return returnValue;
        }

        public static T ExecutePostRequest<T>(ClientContext context, string url, string select = null, string filter = null, string expand = null, Dictionary<string, string> additionalHeaders = null)
        {
            var returnValue = ExecutePostRequestInternal(context, url, null, select, filter, expand);
            return JsonConvert.DeserializeObject<T>(returnValue.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }

        public static T ExecutePostRequest<T>(ClientContext context, string url, string content, string select = null, string filter = null, string expand = null, Dictionary<string, string> additionalHeaders = null, string contentType = "application/json", uint? top = null)
        {
            HttpContent stringContent = null;
            if (content != null)
            {
                stringContent = new StringContent(content);
                if (contentType != null)
                {
                    stringContent.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse(contentType);
                }
            }

            var returnValue = ExecutePostRequestInternal(context, url, stringContent, select, filter, expand, additionalHeaders, top);
            return JsonConvert.DeserializeObject<T>(returnValue.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }

        public static T ExecutePostRequest<T>(ClientContext context, string url, byte[] content, string select = null, string filter = null, string expand = null, Dictionary<string, string> additionalHeaders = null, uint? top = null)
        {
            var byteArrayContent = new ByteArrayContent(content);
            var returnValue = ExecutePostRequestInternal(context, url, byteArrayContent, select, filter, expand, additionalHeaders, top);
            return JsonConvert.DeserializeObject<T>(returnValue.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }

        public static HttpResponseMessage ExecutePostRequest(ClientContext context, string endPointUrl, string select = null, string filter = null, string expand = null, Dictionary<string, string> additionalHeaders = null, uint? top = null)
        {
            return ExecutePostRequestInternal(context, endPointUrl, null, select, filter, expand, additionalHeaders, top);
        }

        public static HttpResponseMessage ExecutePostRequest(ClientContext context, string endPointUrl, string content, string select = null, string filter = null, string expand = null, Dictionary<string, string> additionalHeaders = null, string contentType = "application/json", uint? top = null)
        {
            HttpContent stringContent = null;
            if (!string.IsNullOrEmpty(content))
            {
                stringContent = new StringContent(content);
            }
            if (stringContent != null && contentType != null)
            {
                stringContent.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse(contentType);
            }
            return ExecutePostRequestInternal(context, endPointUrl, stringContent, select, filter, expand, additionalHeaders, top);
        }

        public static HttpResponseMessage ExecutePostRequest(ClientContext context, string endPointUrl, byte[] content, string select = null, string filter = null, string expand = null, Dictionary<string, string> additionalHeaders = null)
        {
            var byteArrayContent = new ByteArrayContent(content);
            return ExecutePostRequestInternal(context, endPointUrl, byteArrayContent, select, filter, expand, additionalHeaders);
        }

        private static HttpResponseMessage ExecutePostRequestInternal(ClientContext context, string endPointUrl, HttpContent content, string select = null, string filter = null, string expand = null, Dictionary<string, string> additionalHeaders = null, uint? top = null)
        {
            var url = endPointUrl;
            if (!url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                url = context.Url + "/_api/" + endPointUrl;
            }
            var restparams = new System.Collections.Generic.List<string>();
            if (!string.IsNullOrEmpty(select))
            {
                restparams.Add($"$select={select}");
            }
            if (!string.IsNullOrEmpty(filter))
            {
                restparams.Add($"$filter=({filter})");
            }
            if (!string.IsNullOrEmpty(expand))
            {
                restparams.Add($"$expand={expand}");
            }
            if (top.HasValue)
            {
                restparams.Add($"$top={top}");
            }
            if (restparams.Any())
            {
                url += $"?{string.Join("&", restparams)}";
            }

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", context.GetAccessToken());
            client.DefaultRequestHeaders.Add("X-RequestDigest", context.GetRequestDigest().GetAwaiter().GetResult());

            if (additionalHeaders != null)
            {
                foreach (var key in additionalHeaders.Keys)
                {
                    client.DefaultRequestHeaders.Add(key, additionalHeaders[key]);
                }
            }
            var returnValue = client.PostAsync(url, content).GetAwaiter().GetResult();
            returnValue.EnsureSuccessStatusCode();
            return returnValue;
        }

        #region PUT
        public static T ExecutePutRequest<T>(ClientContext context, string url, string content, string select = null, string filter = null, string expand = null, Dictionary<string, string> additionalHeaders = null, string contentType = null)
        {
            HttpContent stringContent = new StringContent(content);
            if (contentType != null)
            {
                stringContent.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse(contentType);
            }

            var returnValue = ExecutePutRequestInternal(context, url, stringContent, select, filter, expand);
            return JsonConvert.DeserializeObject<T>(returnValue.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }

        public static T ExecutePutRequest<T>(ClientContext context, string url, byte[] content, string select = null, string filter = null, string expand = null, Dictionary<string, string> additionalHeaders = null)
        {
            var byteArrayContent = new ByteArrayContent(content);
            var returnValue = ExecutePutRequestInternal(context, url, byteArrayContent, select, filter, expand);
            return JsonConvert.DeserializeObject<T>(returnValue.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }


        public static HttpResponseMessage ExecutePutRequest(ClientContext context, string endPointUrl, string select = null, string filter = null, string expand = null, Dictionary<string, string> additionalHeaders = null)
        {
            return ExecutePutRequestInternal(context, endPointUrl, null, select, filter, expand, additionalHeaders);
        }

        public static HttpResponseMessage ExecutePutRequest(ClientContext context, string endPointUrl, string content, string select = null, string filter = null, string expand = null, Dictionary<string, string> additionalHeaders = null, string contentType = null)
        {
            HttpContent stringContent = new StringContent(content);
            if (contentType != null)
            {
                stringContent.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse(contentType);
            }
            return ExecutePutRequestInternal(context, endPointUrl, stringContent, select, filter, expand, additionalHeaders);
        }

        public static HttpResponseMessage ExecutePutRequest(ClientContext context, string endPointUrl, byte[] content, string select = null, string filter = null, string expand = null, Dictionary<string, string> additionalHeaders = null)
        {
            var byteArrayContent = new ByteArrayContent(content);
            return ExecutePutRequestInternal(context, endPointUrl, byteArrayContent, select, filter, expand, additionalHeaders);
        }

        private static HttpResponseMessage ExecutePutRequestInternal(ClientContext context, string endPointUrl, HttpContent content, string select = null, string filter = null, string expand = null, Dictionary<string, string> additionalHeaders = null)
        {
            var url = endPointUrl;
            if (!url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                url = context.Url + "/_api/" + endPointUrl;
            }
            var restparams = new System.Collections.Generic.List<string>();
            if (!string.IsNullOrEmpty(select))
            {
                restparams.Add($"$select={select}");
            }
            if (!string.IsNullOrEmpty(filter))
            {
                restparams.Add($"$filter=({filter})");
            }
            if (!string.IsNullOrEmpty(expand))
            {
                restparams.Add($"$expand={expand}");
            }
            if (restparams.Any())
            {
                url += $"?{string.Join("&", restparams)}";
            }

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", context.GetAccessToken());
            client.DefaultRequestHeaders.Add("X-RequestDigest", context.GetRequestDigest().GetAwaiter().GetResult());

            if (additionalHeaders != null)
            {
                foreach (var key in additionalHeaders.Keys)
                {
                    client.DefaultRequestHeaders.Add(key, additionalHeaders[key]);
                }
            }
            var returnValue = client.PutAsync(url, content).GetAwaiter().GetResult();
            return returnValue;
        }
        #endregion

        #region MERGE
        public static T ExecuteMergeRequest<T>(ClientContext context, string url, string content, string select = null, string filter = null, string expand = null, Dictionary<string, string> additionalHeaders = null, string contentType = null)
        {
            HttpContent stringContent = new StringContent(content);
            if (contentType != null)
            {
                stringContent.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse(contentType);
            }

            var returnValue = ExecuteMergeRequestInternal(context, url, stringContent, select, filter, expand);
            return JsonConvert.DeserializeObject<T>(returnValue.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }

        public static T ExecuteMergeRequest<T>(ClientContext context, string url, byte[] content, string select = null, string filter = null, string expand = null, Dictionary<string, string> additionalHeaders = null)
        {
            var byteArrayContent = new ByteArrayContent(content);
            var returnValue = ExecuteMergeRequestInternal(context, url, byteArrayContent, select, filter, expand);
            return JsonConvert.DeserializeObject<T>(returnValue.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }


        public static HttpResponseMessage ExecuteMergeRequest(ClientContext context, string endPointUrl, string select = null, string filter = null, string expand = null, Dictionary<string, string> additionalHeaders = null)
        {
            return ExecuteMergeRequestInternal(context, endPointUrl, null, select, filter, expand, additionalHeaders);
        }

        public static HttpResponseMessage ExecuteMergeRequest(ClientContext context, string endPointUrl, string content, string select = null, string filter = null, string expand = null, Dictionary<string, string> additionalHeaders = null, string contentType = null)
        {
            HttpContent stringContent = new StringContent(content);
            if (contentType != null)
            {
                stringContent.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse(contentType);
            }
            return ExecuteMergeRequestInternal(context, endPointUrl, stringContent, select, filter, expand, additionalHeaders);
        }

        public static HttpResponseMessage ExecuteMergeRequest(ClientContext context, string endPointUrl, byte[] content, string select = null, string filter = null, string expand = null, Dictionary<string, string> additionalHeaders = null)
        {
            var byteArrayContent = new ByteArrayContent(content);
            return ExecuteMergeRequestInternal(context, endPointUrl, byteArrayContent, select, filter, expand, additionalHeaders);
        }

        private static HttpResponseMessage ExecuteMergeRequestInternal(ClientContext context, string endPointUrl, HttpContent content, string select = null, string filter = null, string expand = null, Dictionary<string, string> additionalHeaders = null)
        {
            var url = endPointUrl;
            if (!url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                url = context.Url + "/_api/" + endPointUrl;
            }
            var restparams = new System.Collections.Generic.List<string>();
            if (!string.IsNullOrEmpty(select))
            {
                restparams.Add($"$select={select}");
            }
            if (!string.IsNullOrEmpty(filter))
            {
                restparams.Add($"$filter=({filter})");
            }
            if (!string.IsNullOrEmpty(expand))
            {
                restparams.Add($"$expand={expand}");
            }
            if (restparams.Any())
            {
                url += $"?{string.Join("&", restparams)}";
            }

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", context.GetAccessToken());
            client.DefaultRequestHeaders.Add("IF-MATCH", "*");
            client.DefaultRequestHeaders.Add("X-RequestDigest", context.GetRequestDigest().GetAwaiter().GetResult());
            client.DefaultRequestHeaders.Add("X-HTTP-Method", "MERGE");
            if (additionalHeaders != null)
            {
                foreach (var key in additionalHeaders.Keys)
                {
                    client.DefaultRequestHeaders.Add(key, additionalHeaders[key]);
                }
            }
            var returnValue = client.PostAsync(url, content).GetAwaiter().GetResult();
            return returnValue;
        }
        #endregion

        #region DELETE

        public static HttpResponseMessage ExecuteDeleteRequest(ClientContext context, string endPointUrl, string select = null, string filter = null, string expand = null, Dictionary<string, string> additionalHeaders = null)
        {
            return ExecuteDeleteRequestInternal(context, endPointUrl, select, filter, expand, additionalHeaders);
        }

        private static HttpResponseMessage ExecuteDeleteRequestInternal(ClientContext context, string endPointUrl, string select = null, string filter = null, string expand = null, Dictionary<string, string> additionalHeaders = null)
        {
            var url = endPointUrl;
            if (!url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                url = context.Url + "/_api/" + endPointUrl;
            }
            var restparams = new System.Collections.Generic.List<string>();
            if (!string.IsNullOrEmpty(select))
            {
                restparams.Add($"$select={select}");
            }
            if (!string.IsNullOrEmpty(filter))
            {
                restparams.Add($"$filter=({filter})");
            }
            if (!string.IsNullOrEmpty(expand))
            {
                restparams.Add($"$expand={expand}");
            }
            if (restparams.Any())
            {
                url += $"?{string.Join("&", restparams)}";
            }

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", context.GetAccessToken());
            client.DefaultRequestHeaders.Add("X-RequestDigest", context.GetRequestDigest().GetAwaiter().GetResult());
            client.DefaultRequestHeaders.Add("X-HTTP-Method", "DELETE");
            if (additionalHeaders != null)
            {
                foreach (var key in additionalHeaders.Keys)
                {
                    client.DefaultRequestHeaders.Add(key, additionalHeaders[key]);
                }
            }
            var returnValue = client.DeleteAsync(url).GetAwaiter().GetResult();
            return returnValue;
        }
        #endregion
    }

}