// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace Microsoft.IIS.Administration.Tests
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Net.Http;
    using System.Text;

    public static class HttpClientExtensions
    {
        public static bool Get(this HttpClient client, string uri, out string result)
        {
            HttpResponseMessage responseMessage = client.GetAsync(uri).Result;
            result = responseMessage.Content.ReadAsStringAsync().Result;
            return Globals.Success(responseMessage);
        }

        public static bool Post(this HttpClient client, string uri, string body, out string result)
        {
            HttpContent content = new StringContent(body, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(uri, content).Result;

            result = response.Content.ReadAsStringAsync().Result;

            return Globals.Success(response);
        }

        public static bool Post(this HttpClient client, string uri, object body, out string result)
        {
            return Post(client, uri, JsonConvert.SerializeObject(body), out result);
        }

        public static JObject Post(this HttpClient client, string uri, string body)
        {
            JObject res = null;
            string result = null;
            if (Post(client, uri, body, out result)) {
                res = JObject.Parse(result);
            }
            return res;
        }

        public static JObject Post(this HttpClient client, string uri, object body)
        {
            return Post(client, uri, JsonConvert.SerializeObject(body));
        }

        public static bool Patch(this HttpClient client, string uri, string body, out string result)
        {
            HttpContent content = new StringContent(body, Encoding.UTF8, "application/json");
            HttpRequestMessage requestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), uri) {
                Content = content
            };

            HttpResponseMessage response = client.SendAsync(requestMessage).Result;

            result = response.Content.ReadAsStringAsync().Result;

            return Globals.Success(response);
        }

        public static bool Patch(this HttpClient client, string uri, object body, out string result)
        {
            string sBody = JsonConvert.SerializeObject(body);
            return Patch(client, uri, sBody, out result);
        }

        public static JObject Patch(this HttpClient client, string uri, string body)
        {
            JObject res = null;
            string result = null;
            if (Patch(client, uri, body, out result)) {
                res = JObject.Parse(result);
            }
            return res;
        }

        public static JObject Patch(this HttpClient client, string uri, object body)
        {
            return Patch(client, uri, JsonConvert.SerializeObject(body));
        }

        public static bool Delete(this HttpClient client, string uri)
        {
            HttpResponseMessage response = client.DeleteAsync(uri).Result;

            return Globals.Success(response);
        }
    }
}
