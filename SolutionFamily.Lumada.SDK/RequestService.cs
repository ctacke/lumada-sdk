using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SolutionFamily.Lumada
{
    internal sealed class RequestService
    {
        private HttpClient m_client;
        private bool m_ignoreCertificateErrors;

        private string APIRoot { get; set; }
        private string APIVersion { get; set; }

        internal RequestService(string apiRoot, string apiVersion)
        {
            APIRoot = apiRoot;
            APIVersion = apiVersion;

            m_client = new HttpClient();
        }

        public bool IgnoreCertificateErrors
        {
            get { return m_ignoreCertificateErrors; }
            set
            {
                if (value == IgnoreCertificateErrors) return;

                if (value)
                {
                    var handler = new HttpClientHandler()
                    {
                        ClientCertificateOptions = ClientCertificateOption.Manual,
                        ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
                        {
                            return true;
                        }
                    };

                    m_client = new HttpClient(handler);
                }
                else
                {
                    m_client = new HttpClient();
                }

                m_ignoreCertificateErrors = value;
            }
        }

        internal async Task<Session> CreateSessionAsync(string username, string password, string clientID)
        {
            var path = string.Format("{0}security/oauth/token", APIRoot);

            var values = new Dictionary<string, string>();
            values.Add("grant_type", "password");
            values.Add("client_id", clientID);
            values.Add("username", username);
            values.Add("password", password);
            values.Add("scope", "all");
            var payload = GenerateUrlEncodedBody(values);

            try
            {
                var response = await PostAsync<AuthenticationResponse>(path, payload);

                var session = new Session(this, clientID, response.SessionID, response.AccessToken, response.RefreshToken, response.Expiry);

                return session;
            }
            catch (ServerUnavailableException sue)
            {
                throw sue;
            }
            catch (Exception ex)
            {
                // TODO: turn into meaninfule lumada exception
                throw;
            }
        }

        internal async Task<AuthenticationResponse> RefreshSessionAsync(string refreshToken, string clientID)
        {
            var path = string.Format("{0}security/oauth/token", APIRoot);

            var values = new Dictionary<string, string>();
            values.Add("grant_type", "refreshToken");
            values.Add("refresh_token", refreshToken);
            values.Add("scope", "all");
            var payload = GenerateUrlEncodedBody(values);

            var response = await PostAsync<AuthenticationResponse>(path, payload);

            return response;
        }

        internal async Task<AssetTypeResponse[]> GetAssetTypesAsync(string accessToken)
        {
            var path = string.Format("{0}asset-management/asset-types", APIRoot);
            var response = await GetAsync<AssetTypeResponseEnvelope>(path, accessToken);
            return response.AssetTypes;
        }

        internal async Task<AssetTypeResponse> AddAssetTypeAsync(AssetTypeRequest assetType, string accessToken)
        {
            var path = string.Format("{0}asset-management/asset-types", APIRoot);
            var payload = JsonConvert.SerializeObject(assetType);
            var response = await PostAsync<AssetTypeResponse>(path, payload, accessToken, "application/json");
            return response;
        }

        internal async Task DeleteAssetTypeAsync(string assetTypeID, string accessToken)
        {
            var path = string.Format("{0}asset-management/asset-types/{1}", APIRoot, assetTypeID);
            await DeleteAsync(path, accessToken);
        }

        internal async Task<AssetResponse[]> GetAssetsAsync(string accessToken)
        {
            var path = string.Format("{0}asset-management/assets", APIRoot);
            var response = await GetAsync<AssetResponseEnvelope>(path, accessToken);
            return response.Assets;
        }

        internal async Task<AssetResponse> GetAssetAsync(string assetID, string accessToken)
        {
            var path = string.Format("{0}asset-management/assets/{1}", APIRoot, assetID);

            var response = await GetAsync<AssetResponse>(path, accessToken);
            return response;
        }

        internal async Task<AssetResponse> AddAssetAsync(AssetRequest asset, string accessToken)
        {
            var path = string.Format("{0}asset-management/assets", APIRoot);
            var payload = JsonConvert.SerializeObject(asset);
            var response = await PostAsync<AssetResponse>(path, payload, accessToken, "application/json");
            return response;
        }

        internal async Task DeleteAssetAsync(string assetID, string accessToken)
        {
            var path = string.Format("{0}asset-management/assets/{1}", APIRoot, assetID);
            await DeleteAsync(path, accessToken);
        }

        internal async Task<AssetTokenResponse> GetAssetTokenAsync(string assetID, string accessToken)
        {
            var path = string.Format("{0}asset-management/assets/{1}/token", APIRoot, assetID);

            var response = await GetAsync<AssetTokenResponse>(path, accessToken);
            return response;
        }

        internal async Task UploadAssetStateAsync(string assetID, AssetToken token, object stateData)
        {
            var path = string.Format("{0}asset-connectivity/assets/{1}/state", APIRoot, assetID);
            var payload = JsonConvert.SerializeObject(stateData);
            var hash = token.AuthHash.Replace("DEVICEHASH ", string.Empty);

            await PostAsync(
                path,
                payload, 
                "application/json",
                new AuthenticationHeaderValue("devicehash", hash));
        }

        internal async Task UploadAssetStateAsync(string assetID, AssetToken token, Dictionary<string, object> stateData)
        {
            var path = string.Format("{0}asset-connectivity/assets/{1}/state", APIRoot, assetID);
            var payload = new JObject();
            var hash = token.AuthHash.Replace("DEVICEHASH ", string.Empty);

            foreach (var datum in stateData)
            {
                payload.Add(datum.Key, new JValue(datum.Value));
            }

            await PostAsync(
                path,
                payload.ToString(),
                "application/json",
                new AuthenticationHeaderValue("devicehash", hash));
        }

        private string GenerateUrlEncodedBody(Dictionary<string, string> values)
        {
            var sb = new StringBuilder();

            var count = 0;
            foreach (var value in values)
            {
                sb.AppendFormat("{0}={1}",
                    WebUtility.UrlEncode(value.Key),
                    WebUtility.UrlEncode(value.Value));

                if (++count < values.Count)
                {
                    sb.Append("&");
                }
            }

            return sb.ToString();
        }

        public async Task DeleteAsync(string path, string accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, path);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var task = await m_client.SendAsync(request)
                .ContinueWith(async (response) =>
                {
                    var json = await response.Result.Content.ReadAsStringAsync();
                });

            await task;
        }
        
        public async Task<T> GetAsync<T>(string path, string accessToken = null)
        {
            if (accessToken == null)
            {
                var task = await m_client.GetAsync(path)
                    .ContinueWith(async (response) =>
                    {
                        var json = await response.Result.Content.ReadAsStringAsync();
                        var entity = JsonConvert.DeserializeObject<T>(json);
                        return entity;
                    });

                return await task;
            }
            else
            {
                var request = new HttpRequestMessage(HttpMethod.Get, path);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var task = await m_client.SendAsync(request)
                    .ContinueWith(async (response) =>
                    {
                        var json = await response.Result.Content.ReadAsStringAsync();
                        var entity = JsonConvert.DeserializeObject<T>(json);
                        return entity;
                    });

                return await task;
            }
        }

        public async Task PostAsync (string path, string body, string contentType = null, AuthenticationHeaderValue authorization = null)
        {
            var content = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");
            if (contentType != null)
            {
                content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            }

            if (authorization == null)
            {
                var task = await m_client.PostAsync(path, content)
                .ContinueWith(async (response) =>
                {
                    var json = await response.Result.Content.ReadAsStringAsync();
                });

                await task;
            }
            else
            {
                var request = new HttpRequestMessage(HttpMethod.Post, path);
                request.Headers.Authorization = authorization;
                request.Content = content;

                var task = await m_client.SendAsync(request)
                    .ContinueWith(async (response) =>
                    {
                        var json = await response.Result.Content.ReadAsStringAsync();
                        Debug.WriteLine(json);
                    });

                await task;
            }
        }

        public async Task<T> PostAsync<T>(string path, string body, string accessToken = null, string contentType = null)
        {
            var content = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");
            if (contentType != null)
            {
                content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            }

            if (accessToken == null)
            {
                var task = await m_client.PostAsync(path, content)
                .ContinueWith(async (response) =>
                {
                    var json = await response.Result.Content.ReadAsStringAsync();
                    if (json.StartsWith("<html>", StringComparison.OrdinalIgnoreCase))
                    {
                        // the server returned an html error page, not actual JSON
                        // TODO: parse the HTML for more info?
                        throw new ServerUnavailableException();
                    }
                    var entity = JsonConvert.DeserializeObject<T>(json);
                    return entity;
                });

                return await task;
            }
            else
            {
                var request = new HttpRequestMessage(HttpMethod.Post, path);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                request.Content = content;

                var task = await m_client.SendAsync(request)
                    .ContinueWith(async (response) =>
                    {
                        var json = await response.Result.Content.ReadAsStringAsync();
                        var entity = JsonConvert.DeserializeObject<T>(json);
                        return entity;
                    });

                return await task;
            }
        }
    }
}
