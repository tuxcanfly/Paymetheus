// Copyright(c) 2016 The Decred developers
// Licensed under the ISC license.  See LICENSE file in the project root for full license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Paymetheus.StakePoolIntegration
{
    public sealed class PoolApiClient
    {
        const string Version = "v0.1";

        static Uri RequestUri(Uri poolUri, string request) => new Uri(poolUri, $"api/{Version}/{request}");

        static string GetDataValue(Dictionary<string, string> data, string key)
        {
            string value;
            data.TryGetValue(key, out value);
            if (data.TryGetValue(key, out value) && value != null)
                return value;
            else
                throw new InvalidDataException($"API response missing expected value for key `{key}`.");
        }

        readonly Uri _poolUri;
        readonly HttpClient _httpClient;
        readonly CookieContainer _cookieContainer;
        readonly JsonSerializer _jsonSerializer = new JsonSerializer();

        string CsrfToken => _cookieContainer.GetCookies(_poolUri)?["XSRF-TOKEN"]?.Value ?? "";

        PoolApiResponse UnmarshalResponse(Stream responseStream)
        {
            using (var streamReader = new StreamReader(responseStream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                return _jsonSerializer.Deserialize<PoolApiResponse>(jsonReader);
            }
        }

        PoolApiClient(Uri poolUri, HttpClient httpClient, CookieContainer cookieContainer)
        {
            _poolUri = poolUri;
            _httpClient = httpClient;
            _cookieContainer = cookieContainer;
        }

        public static async Task<PoolApiClient> StartSessionAsync(Uri poolUri)
        {
            var cookieContainer = new CookieContainer();
            var httpClient = new HttpClient(new HttpClientHandler { CookieContainer = cookieContainer });
            var requestUri = RequestUri(poolUri, "startsession");
            var httpResponse = await httpClient.GetAsync(requestUri);
            httpResponse.EnsureSuccessStatusCode();
            var apiClient = new PoolApiClient(poolUri, httpClient, cookieContainer);
            using (var stream = await httpResponse.Content.ReadAsStreamAsync())
            {
                apiClient.UnmarshalResponse(stream).EnsureSuccess();
            }
            return apiClient;
        }

        public async Task SignUpAsync(string email, string password)
        {
            if (email == null)
                throw new ArgumentNullException(nameof(email));
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            var requestUri = RequestUri(_poolUri, "signup");
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["csrf_token"] = CsrfToken,
                ["email"] = email,
                ["password"] = password,
                ["passwordrepeat"] = password, // why? validation has to be done on the client side
            });
            var httpResponse = await _httpClient.PostAsync(requestUri, content);
            httpResponse.EnsureSuccessStatusCode();
            using (var stream = await httpResponse.Content.ReadAsStreamAsync())
            {
                UnmarshalResponse(stream).EnsureSuccess();
            }
        }

        public async Task SignInAsync(string email, string password)
        {
            if (email == null)
                throw new ArgumentNullException(nameof(email));
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            var requestUri = RequestUri(_poolUri, "signin");
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["csrf_token"] = CsrfToken,
                ["email"] = email,
                ["password"] = password,
            });
            var httpResponse = await _httpClient.PostAsync(requestUri, content);
            httpResponse.EnsureSuccessStatusCode();
            using (var stream = await httpResponse.Content.ReadAsStreamAsync())
            {
                UnmarshalResponse(stream).EnsureSuccess();
            }
        }

        public async Task CreateVotingAddressAsync(string pubKeyAddress)
        {
            if (pubKeyAddress == null)
                throw new ArgumentNullException(nameof(pubKeyAddress));

            var requestUri = RequestUri(_poolUri, "address");
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["csrf_token"] = CsrfToken,
                ["UserPubKeyAddr"] = pubKeyAddress,
            });
            var httpResponse = await _httpClient.PostAsync(requestUri, content);
            httpResponse.EnsureSuccessStatusCode();
            using (var stream = await httpResponse.Content.ReadAsStreamAsync())
            {
                UnmarshalResponse(stream).EnsureSuccess();
            }
        }

        public async Task<PoolUserInfo> GetPurchaseInfoAsync()
        {
            var requestUri = RequestUri(_poolUri, "getPurchaseInfo");
            var httpResponse = await _httpClient.GetAsync(requestUri);
            httpResponse.EnsureSuccessStatusCode();

            PoolApiResponse apiResponse;
            using (var stream = await httpResponse.Content.ReadAsStreamAsync())
            {
                apiResponse = UnmarshalResponse(stream);
            }
            apiResponse.EnsureSuccess();

            return new PoolUserInfo
            {
                FeeAddress = GetDataValue(apiResponse.Data, "pooladdress"),
                Fee = decimal.Parse(GetDataValue(apiResponse.Data, "poolfees")) / 100,
                RedeemScriptHex = GetDataValue(apiResponse.Data, "script"),
                VotingAddress = GetDataValue(apiResponse.Data, "ticketaddress"),
            };
        }
    }
}
