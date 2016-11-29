// Copyright(c) 2016 The Decred developers
// Licensed under the ISC license.  See LICENSE file in the project root for full license information.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace Paymetheus.StakePoolIntegration
{
    sealed class PoolApiResponse
    {
        [JsonRequired]
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonRequired]
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "data")]
        public Dictionary<string, string> Data { get; set; }

        public void EnsureSuccess()
        {
            if (Status != "success")
                throw new PoolApiResponseException(Status, Message);
        }
    }
}
