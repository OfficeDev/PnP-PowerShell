using PnP.PowerShell.Commands.Model.Teams;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PnP.PowerShell.Commands.Utilities.REST
{
    internal static class GraphHelper
    {
        public static bool TryGetGraphException(HttpResponseMessage responseMessage, out GraphException exception)
        {
            if (responseMessage == null)
            {
                exception = null;
                return false;
            }
            var content = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(content))
            {
                exception = null;
                return false;
            }
            try
            {
                exception = JsonSerializer.Deserialize<GraphException>(content, new JsonSerializerOptions() { IgnoreNullValues = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                return true;
            }
            catch
            {
                exception = null;
                return false;
            }
        }

        private static HttpRequestMessage GetMessage(string url, HttpMethod method, string accessToken, HttpContent content = null)
        {
            if (url.StartsWith("/"))
            {
                url = url.Substring(1);
            }

            var message = new HttpRequestMessage();
            message.Method = method;
            message.RequestUri = !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ? new Uri($"https://graph.microsoft.com/{url}") : new Uri(url);
            message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            if (method == HttpMethod.Post || method == HttpMethod.Put || method.Method == "PATCH")
            {
                message.Content = content;
            }

            return message;
        }

        public static async Task<string> GetAsync(HttpClient httpClient, string url, string accessToken)
        {
            var message = GetMessage(url, HttpMethod.Get, accessToken);
            return await SendMessageAsync(httpClient, message);
        }

        public static async Task<T> GetAsync<T>(HttpClient httpClient, string url, string accessToken, bool camlCasePolicy = true)
        {
            var stringContent = await GetAsync(httpClient, url, accessToken);
            if (stringContent != null)
            {
                var options = new JsonSerializerOptions() { IgnoreNullValues = true };
                if (camlCasePolicy)
                {
                    options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                }
                try
                {
                    return JsonSerializer.Deserialize<T>(stringContent, options);
                }
                catch (Exception ex)
                {
                    return default(T);
                }
            }
            return default(T);
        }

        public static async Task<HttpResponseMessage> PostAsync(HttpClient httpClient, string url, string accessToken, HttpContent content)
        {
            var message = GetMessage(url, HttpMethod.Post, accessToken, content);
            return await GetResponseMessageAsync(httpClient, message);
        }

        public static async Task<HttpResponseMessage> PutAsync(HttpClient httpClient, string url, string accessToken, HttpContent content)
        {
            var message = GetMessage(url, HttpMethod.Put, accessToken, content);
            return await GetResponseMessageAsync(httpClient, message);
        }



        public static async Task<T> PatchAsync<T>(HttpClient httpClient, string accessToken, string url, T content)
        {
            var requestContent = new StringContent(JsonSerializer.Serialize(content, new JsonSerializerOptions() { IgnoreNullValues = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
            requestContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var message = GetMessage(url, new HttpMethod("PATCH"), accessToken, requestContent);
            var returnValue = await SendMessageAsync(httpClient, message);
            if (!string.IsNullOrEmpty(returnValue))
            {
                return JsonSerializer.Deserialize<T>(returnValue, new JsonSerializerOptions() { IgnoreNullValues = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
            else
            {
                return default;
            }
        }



        public static async Task<T> PostAsync<T>(HttpClient httpClient, string url, HttpContent content, string accessToken)
        {
            return await PostInternalAsync<T>(httpClient, url, accessToken, content);
        }

        public static async Task<T> PutAsync<T>(HttpClient httpClient, string url, string accessToken, HttpContent content)
        {
            var message = GetMessage(url, HttpMethod.Put, accessToken, content);
            var stringContent = await SendMessageAsync(httpClient, message);
            if (stringContent != null)
            {
                try
                {
                    return JsonSerializer.Deserialize<T>(stringContent);
                }
                catch
                {
                    return default;
                }
            }
            return default;
        }

        public static async Task<T> PostAsync<T>(HttpClient httpClient, string url, T content, string accessToken)
        {
            var requestContent = new StringContent(JsonSerializer.Serialize(content, new JsonSerializerOptions() { IgnoreNullValues = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
            requestContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            return await PostInternalAsync<T>(httpClient, url, accessToken, requestContent);
        }

        public static async Task<T> PostAsync<T>(HttpClient httpClient, string url, string accessToken)
        {
            return await PostInternalAsync<T>(httpClient, url, accessToken, null);
        }

        private static async Task<T> PostInternalAsync<T>(HttpClient httpClient, string url, string accessToken, HttpContent content)
        {
            var message = GetMessage(url, HttpMethod.Post, accessToken, content);
            var stringContent = await SendMessageAsync(httpClient, message);
            if (stringContent != null)
            {
                try
                {
                    return JsonSerializer.Deserialize<T>(stringContent, new JsonSerializerOptions() { IgnoreNullValues = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                }
                catch
                {
                    return default;
                }
            }
            return default;
        }

        public static async Task<T> PutAsync<T>(HttpClient httpClient, string url, T content, string accessToken)
        {
            var requestContent = new StringContent(JsonSerializer.Serialize(content, new JsonSerializerOptions() { IgnoreNullValues = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
            requestContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var message = GetMessage(url, HttpMethod.Put, accessToken, requestContent);
            var returnValue = await SendMessageAsync(httpClient, message);
            if (!string.IsNullOrEmpty(returnValue))
            {
                return JsonSerializer.Deserialize<T>(returnValue, new JsonSerializerOptions() { IgnoreNullValues = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
            else
            {
                return default;
            }
        }

        public static async Task<HttpResponseMessage> DeleteAsync(HttpClient httpClient, string url, string accessToken)
        {
            var message = GetMessage(url, HttpMethod.Delete, accessToken);
            var response = await GetResponseMessageAsync(httpClient, message);
            return response;
        }

        private static async Task<string> SendMessageAsync(HttpClient httpClient, HttpRequestMessage message)
        {
            var response = await httpClient.SendAsync(message);
            while (response.StatusCode == (HttpStatusCode)429)
            {
                // throttled
                var retryAfter = response.Headers.RetryAfter;
                Thread.Sleep(retryAfter.Delta.Value.Seconds * 1000);
                response = await httpClient.SendAsync(CloneMessage(message));
            }
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                var exception = JsonSerializer.Deserialize<GraphException>(errorContent, new JsonSerializerOptions() { IgnoreNullValues = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                throw exception;
            }
        }



        public static async Task<HttpResponseMessage> GetResponseMessageAsync(HttpClient httpClient, HttpRequestMessage message)
        {
            var response = await httpClient.SendAsync(message);
            while (response.StatusCode == (HttpStatusCode)429)
            {
                // throttled
                var retryAfter = response.Headers.RetryAfter;
                Thread.Sleep(retryAfter.Delta.Value.Seconds * 1000);
                response = await httpClient.SendAsync(CloneMessage(message));
            }
            return response;
        }

        private static HttpRequestMessage CloneMessage(HttpRequestMessage req)
        {
            HttpRequestMessage clone = new HttpRequestMessage(req.Method, req.RequestUri);

            clone.Content = req.Content;
            clone.Version = req.Version;

            foreach (KeyValuePair<string, object> prop in req.Properties)
            {
                clone.Properties.Add(prop);
            }

            foreach (KeyValuePair<string, IEnumerable<string>> header in req.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return clone;
        }
    }
}
