using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MediaMarkup.Api;
using MediaMarkup.Core;
using MediaMarkup.Api.Models;
using Newtonsoft.Json.Linq;

namespace MediaMarkup
{
    /// <summary>
    /// API Client for accessing Media Markup API
    /// </summary>
    public class ApiClient : IApiClient
    {
        /// <summary>
        /// Settings, passed in or loaded from appSettings
        /// </summary>
        private Settings ClientSettings { get; }

        /// <summary>
        /// HttpClient for making API calls
        /// </summary>
        private HttpClient HttpClient { get; set; }

        /// <inheritdoc/>
        public string AccessToken { get; set; }

        /// <inheritdoc/>
        public IApprovals Approvals { get; private set; }

        /// <inheritdoc/>
        public IUsers Users { get; private set; }

        /// <summary>
        /// Creates the Media Markup API Client with the specified settings
        /// </summary>
        /// <param name="clientSettings"></param>
        /// <param name="token"></param>
        public ApiClient(Settings clientSettings, string token = null)
        {
            ClientSettings = clientSettings;

            CheckSettings();

            AccessToken = token;

            if (!ClientSettings.ApiBaseUrl.EndsWith("/"))
            {
                ClientSettings.ApiBaseUrl = $"{ClientSettings.ApiBaseUrl}/";
            }
        }

        private void CheckSettings()
        {
            if (string.IsNullOrWhiteSpace(ClientSettings.ClientId))
            {
                throw new Exception("ClientId setting not set");
            }

            if (string.IsNullOrWhiteSpace(ClientSettings.SecretKey))
            {
                throw new Exception("SecretKey setting not set");
            }

            if (string.IsNullOrWhiteSpace(ClientSettings.ApiBaseUrl))
            {
                throw new Exception("ApiBaseUrl setting not set");
            }

            if (ClientSettings.UseRetryLogic == true)
            {
                if (string.IsNullOrWhiteSpace(ClientSettings.RetryStatusCodes))
                {
                    throw new Exception("RetryStatusCodes not set");
                }

                try
                {
                    if (!ClientSettings.RetryStatusCodes.Any())
                    {
                        throw new Exception("RetryStatusCodes not set");
                    }
                }
                catch
                {
                    throw new Exception("RetryStatusCodes not valid");
                }
            }
        }

        /// <inheritdoc/>
        public async Task<string> InitializeAsync()
        {
            if (string.IsNullOrWhiteSpace(AccessToken) || !AccessTokenPayloadIsValid())
            {
                AccessToken = await GetAccessToken();
            }

            HttpClient = GetHttpClient();

            Approvals = new Approvals(HttpClient);

            Users = new Users(HttpClient);

            return AccessToken;
        }

        /// <inheritdoc/>
        public async Task<bool> Authenticated()
        {
            var response = await HttpClient.PostAsync($"{ClientSettings.ApiBaseUrl}Authentication/Authenticated/", null);

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Gets the Http Client for making the API calls, uses the supplied token or gets a new one if payload is invlaid/expired
        /// </summary>
        /// <returns></returns>
        private HttpClient GetHttpClient()
        {
            HttpClient apiClient;

            if (ClientSettings.UseRetryLogic == true && ClientSettings.RetryStatusCodesList.Any())
            {
                apiClient = new HttpClient(new HttpClientRetryHandler(new HttpClientHandler(), ClientSettings.RetryStatusCodesList)) {BaseAddress = new Uri(ClientSettings.ApiBaseUrl)};
            }
            else
            {
                apiClient = new HttpClient{BaseAddress = new Uri(ClientSettings.ApiBaseUrl)};
            }

            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            
            return apiClient;
        }

        private async Task<string> GetAccessToken()
        {
            // Get new httpclient here for connection without authorization using client is and secret
            var apiClient = new HttpClient {BaseAddress = new Uri(ClientSettings.ApiBaseUrl)};

            var response = await apiClient.PostAsJsonAsync($"{ClientSettings.ApiBaseUrl}connect/token", new AccessTokenRequestParameters
            {
                ClientId = ClientSettings.ClientId,
                SecretKey = ClientSettings.SecretKey
            });

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                AccessToken = JObject.Parse(responseContent)["access_token"].ToString();
                return AccessToken;
            }

            AccessToken = null;

            throw new ApiException("ApiClient.GetAccessToken", response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Validates token payload, checks sub (client id) & exp (expiry date)
        /// </summary>
        /// <returns></returns>
        private bool AccessTokenPayloadIsValid()
        {
            var jwtTokenData = Jwt.GetTokenData(AccessToken);

            if (!string.IsNullOrWhiteSpace(jwtTokenData?.Payload))
            {
                dynamic payload = JsonConvert.DeserializeObject(jwtTokenData.Payload);

                string subject = (payload["sub"] ?? "").ToString();
                string expiry = (payload["exp"] ?? "").ToString();

                if (!string.IsNullOrWhiteSpace(subject) && !string.IsNullOrWhiteSpace(expiry))
                {
                    var expiryDate = Jwt.UnixTimeStampToDateTime(expiry);

                    if (subject.IndexOf(ClientSettings.ClientId, StringComparison.OrdinalIgnoreCase) >= 0 || expiryDate > DateTime.UtcNow.AddMinutes(1))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}