// Copyright(c) 2016 The Decred developers
// Licensed under the ISC license.  See LICENSE file in the project root for full license information.

using Newtonsoft.Json;
using System.IO;

namespace Paymetheus.StakePoolIntegration
{
    class PoolApiResponse
    {
        [JsonRequired]
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonRequired]
        [JsonProperty(PropertyName = "code")]
        public StatusCode Code { get; set; }

        [JsonRequired]
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        public void EnsureSuccess()
        {
            if (Code != StatusCode.Ok)
                throw new PoolApiResponseException(Code, Status, Message);
        }
    }

    class PoolApiResponse<T> : PoolApiResponse
    {
        // This is NOT required -- if the request fails (e.g. bad token), we want to discover
        // that when checking the status code rather than throwing a parsing error.  Use
        // EnsureHasData to perform that check.
        [JsonProperty(PropertyName = "data")]
        public T Data { get; set; }

        public void EnsureHasData()
        {
            if (Data == null)
                throw new InvalidDataException("Missing data message in response.");
        }
    }
}
