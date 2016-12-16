// Copyright (c) 2016 The Decred developers
// Licensed under the ISC license.  See LICENSE file in the project root for full license information.

using Newtonsoft.Json;

namespace Paymetheus.StakePoolIntegration
{
    public sealed class PoolUserInfo
    {
        [JsonRequired]
        [JsonProperty(PropertyName = "PoolAddress")]
        public string FeeAddress { get; set; }

        [JsonRequired]
        [JsonProperty(PropertyName = "PoolFees")]
        [JsonConverter(typeof(DecimalPercentageConverter))]
        public decimal Fee { get; set; }

        [JsonRequired]
        [JsonProperty(PropertyName = "Script")]
        public string RedeemScriptHex { get; set; }

        [JsonRequired]
        [JsonProperty(PropertyName = "TicketAddress")]
        public string VotingAddress { get; set; }
    }
}
