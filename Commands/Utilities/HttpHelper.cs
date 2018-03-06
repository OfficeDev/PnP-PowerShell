using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace SharePointPnP.PowerShell.Commands.Utilities
{
    /// <summary>
    /// Static class full of helper methods to make HTTP requests
    /// </summary>
    public static class HttpHelper
    {
        /// <summary>
        /// This helper method makes an HTTP GET request and returns the result as a String
        /// </summary>
        /// <param name="requestUrl">The URL of the request</param>
        /// <param name="accessToken">The OAuth 2.0 Access Token for the request, if authorization is required</param>
        /// <returns>The String value of the result</returns>
        public static string MakeGetRequestForString(string requestUrl,
            string accessToken = null)
        {
            return (MakeHttpRequest("GET",
                requestUrl, 
                accessToken,
                resultPredicate: r => r.Content.ReadAsStringAsync().Result));
        }

        /// <summary>
        /// This helper method makes an HTTP GET request and returns the result as a String
        /// </summary>
        /// <param name="requestUrl">The URL of the request</param>
        /// <param name="accessToken">The OAuth 2.0 Access Token for the request, if authorization is required</param>
        /// <param name="accept">The accept header for the response</param>
        /// <returns>The Stream  of the result</returns>
        public static System.IO.Stream MakeGetRequestForStream(string requestUrl,
            string accept,
            string accessToken = null,
            string referer = null)
        {
            return (MakeHttpRequest<System.IO.Stream>("GET",
                requestUrl, 
                accessToken,
                referer : referer,
                resultPredicate: r => r.Content.ReadAsStreamAsync().Result));
        }

        /// <summary>
        /// This helper method makes an HTTP GET request and returns the result as a String
        /// </summary>
        /// <param name="requestUrl">The URL of the request</param>
        /// <param name="responseHeaders">The response headers of the HTTP request (output argument)</param>
        /// <param name="accessToken">The OAuth 2.0 Access Token for the request, if authorization is required</param>
        /// <param name="accept">The accept header for the response</param>
        /// <returns>The Stream  of the result</returns>
        public static System.IO.Stream MakeGetRequestForStreamWithResponseHeaders(string requestUrl,
            string accept,
            out HttpContentHeaders contentHeaders,
            string accessToken = null)
        {
            return (MakeHttpRequest("GET",
                requestUrl,
                out contentHeaders,
                accessToken,
                resultPredicate: r => r.Content.ReadAsStreamAsync().Result));
        }

        /// <summary>
        /// This helper method makes an HTTP POST request without a response
        /// </summary>
        /// <param name="requestUrl">The URL of the request</param>
        /// <param name="accessToken">The OAuth 2.0 Access Token for the request, if authorization is required</param>
        /// <param name="content">The content of the request</param>
        /// <param name="contentType">The content/type of the request</param>
        public static void MakePostRequest(string requestUrl,
            object content = null,
            string contentType = null,
            string accessToken = null)
        {
            MakeHttpRequest<string>("POST",
                requestUrl,
                accessToken: accessToken,
                content: content,
                contentType: contentType);
        }

        /// <summary>
        /// This helper method makes an HTTP POST request and returns the result as a String
        /// </summary>
        /// <param name="requestUrl">The URL of the request</param>
        /// <param name="accessToken">The OAuth 2.0 Access Token for the request, if authorization is required</param>
        /// <param name="content">The content of the request</param>
        /// <param name="contentType">The content/type of the request</param>
        /// <returns>The String value of the result</returns>
        public static string MakePostRequestForString(string requestUrl,
            object content = null,
            string contentType = null,
            string accessToken = null)
        {
            return (MakeHttpRequest("POST",
                requestUrl,
                accessToken: accessToken,
                content: content,
                contentType: contentType,
                resultPredicate: r => r.Content.ReadAsStringAsync().Result));
        }

        /// <summary>
        /// This helper method makes an HTTP PUT request without a response
        /// </summary>
        /// <param name="requestUrl">The URL of the request</param>
        /// <param name="accessToken">The OAuth 2.0 Access Token for the request, if authorization is required</param>
        /// <param name="content">The content of the request</param>
        /// <param name="contentType">The content/type of the request</param>
        public static void MakePutRequest(string requestUrl,
            object content = null,
            string contentType = null,
            string accessToken = null)
        {
            MakeHttpRequest<string>("PUT",
                requestUrl,
                accessToken: accessToken,
                content: content,
                contentType: contentType);
        }

        /// <summary>
        /// This helper method makes an HTTP PUT request and returns the result as a String
        /// </summary>
        /// <param name="requestUrl">The URL of the request</param>
        /// <param name="accessToken">The OAuth 2.0 Access Token for the request, if authorization is required</param>
        /// <param name="content">The content of the request</param>
        /// <param name="contentType">The content/type of the request</param>
        /// <returns>The String value of the result</returns>
        public static string MakePutRequestForString(string requestUrl,
            object content = null,
            string contentType = null,
            string accessToken = null)
        {
            return (MakeHttpRequest("PUT",
                requestUrl,
                accessToken: accessToken,
                content: content,
                contentType: contentType,
                resultPredicate: r => r.Content.ReadAsStringAsync().Result));
        }

        /// <summary>
        /// This helper method makes an HTTP PATCH request and returns the result as a String
        /// </summary>
        /// <param name="requestUrl">The URL of the request</param>
        /// <param name="accessToken">The OAuth 2.0 Access Token for the request, if authorization is required</param>
        /// <param name="content">The content of the request</param>
        /// <param name="contentType">The content/type of the request</param>
        /// <returns>The String value of the result</returns>
        public static string MakePatchRequestForString(string requestUrl,
            object content = null,
            string contentType = null,
            string accessToken = null)
        {
            return (MakeHttpRequest("PATCH",
                requestUrl,
                accessToken: accessToken,
                content: content,
                contentType: contentType,
                resultPredicate: r => r.Content.ReadAsStringAsync().Result));
        }

        /// <summary>
        /// This helper method makes an HTTP DELETE request
        /// </summary>
        /// <param name="requestUrl">The URL of the request</param>
        /// <param name="accessToken">The OAuth 2.0 Access Token for the request, if authorization is required</param>
        /// <returns>The String value of the result</returns>
        public static void MakeDeleteRequest(string requestUrl,
            string accessToken = null)
        {
            MakeHttpRequest<string>("DELETE", requestUrl, accessToken);
        }

        /// <summary>
        /// This helper method makes an HTTP request and eventually returns a result
        /// </summary>
        /// <param name="httpMethod">The HTTP method for the request</param>
        /// <param name="requestUrl">The URL of the request</param>
        /// <param name="accessToken">The OAuth 2.0 Access Token for the request, if authorization is required</param>
        /// <param name="accept">The content type of the accepted response</param>
        /// <param name="content">The content of the request</param>
        /// <param name="contentType">The content  type of the request</param>
        /// <param name="resultPredicate">The predicate to retrieve the result, if any</param>
        /// <typeparam name="TResult">The type of the result, if any</typeparam>
        /// <returns>The value of the result, if any</returns>
        private static TResult MakeHttpRequest<TResult>(
            string httpMethod,
            string requestUrl,
            string accessToken = null,
            string accept = null,
            object content = null,
            string contentType = null,
            string referer = null,
            Func<HttpResponseMessage, TResult> resultPredicate = null)
        {
            HttpContentHeaders contentHeaders;
            return (MakeHttpRequest(httpMethod,
                requestUrl,
                out contentHeaders,
                accessToken,
                accept,
                content,
                contentType,
                referer,
                resultPredicate));
        }

        /// <summary>
        /// This helper method makes an HTTP request and eventually returns a result
        /// </summary>
        /// <param name="httpMethod">The HTTP method for the request</param>
        /// <param name="requestUrl">The URL of the request</param>
        /// <param name="responseHeaders">The response headers of the HTTP request (output argument)</param>
        /// <param name="accessToken">The OAuth 2.0 Access Token for the request, if authorization is required</param>
        /// <param name="accept">The content type of the accepted response</param>
        /// <param name="content">The content of the request</param>
        /// <param name="contentType">The content  type of the request</param>
        /// <param name="resultPredicate">The predicate to retrieve the result, if any</param>
        /// <typeparam name="TResult">The type of the result, if any</typeparam>
        /// <returns>The value of the result, if any</returns>
        private static TResult MakeHttpRequest<TResult>(
            string httpMethod,
            string requestUrl,
            out HttpContentHeaders contentHeaders,
            string accessToken = null,
            string accept = null,
            object content = null,
            string contentType = null,
            string referer = null,
            Func<HttpResponseMessage, TResult> resultPredicate = null)
        {
            // Prepare the variable to hold the result, if any
            TResult result = default(TResult);
            contentHeaders = null;

            Uri requestUri = new Uri(requestUrl);

            // If we have the token, then handle the HTTP request
            HttpClientHandler handler = new HttpClientHandler();
            handler.AllowAutoRedirect = true;
            HttpClient httpClient = new HttpClient(handler, true);

            // Set the Authorization Bearer token
            if (!string.IsNullOrEmpty(accessToken))
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
            }

            if (!string.IsNullOrEmpty(referer))
            {
                httpClient.DefaultRequestHeaders.Referrer = new Uri(referer);
            }

            // If there is an accept argument, set the corresponding HTTP header
            if (!string.IsNullOrEmpty(accept))
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(accept));
            }

            // Prepare the content of the request, if any
            HttpContent requestContent = null;
            System.IO.Stream streamContent = content as System.IO.Stream;
            if (streamContent != null)
            {
                requestContent = new StreamContent(streamContent);
                requestContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            }
            else
            {
                requestContent =
                    (content != null) ?
                    new StringContent(JsonConvert.SerializeObject(content,
                        Formatting.None,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        }),
                    Encoding.UTF8, contentType) :
                    null;
            }

            // Prepare the HTTP request message with the proper HTTP method
            HttpRequestMessage request = new HttpRequestMessage(
                new HttpMethod(httpMethod), requestUrl);

            // Set the request content, if any
            if (requestContent != null)
            {
                request.Content = requestContent;
            }
       
            // Fire the HTTP request
            HttpResponseMessage response = httpClient.SendAsync(request).Result;

            if (response.IsSuccessStatusCode)
            {
                // If the response is Success and there is a
                // predicate to retrieve the result, invoke it
                if (resultPredicate != null)
                {
                    result = resultPredicate(response);
                }

                // Get any content header and put it in the answer
                contentHeaders = response.Content.Headers;
            }
            else
            {
                throw new ApplicationException(
                    string.Format("Exception while invoking endpoint {0}.", requestUrl),
#if !NETSTANDARD2_0
                    new HttpException(
                        (int)response.StatusCode,
                        response.Content.ReadAsStringAsync().Result));
#else
                    new Exception(response.Content.ReadAsStringAsync().Result));
#endif
            }

            return (result);
        }
    }
}
