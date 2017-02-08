// Copyright (c) 2016 The Decred developers
// Licensed under the ISC license.  See LICENSE file in the project root for full license information.

using Newtonsoft.Json;
using System;

namespace Paymetheus.StakePoolIntegration
{
    public sealed class StakePoolInfo
    {
        [JsonRequired]
        [JsonProperty(PropertyName = "APIEnabled")]
        public bool ApiEnabled { get; set; }

        [JsonRequired]
        [JsonProperty(PropertyName = "APIVersionsSupported")]
        public uint[] SupportedApiVersions { get; set; }

        [JsonRequired]
        [JsonProperty(PropertyName = "LastAttempt")]
        public long LastAttempt { get; set; }

        [JsonRequired]
        [JsonProperty(PropertyName = "LastUpdated")]
        public long LastUpdated { get; set; }

        [JsonRequired]
        [JsonProperty(PropertyName = "Network")]
        public string Network { get; set; }

        [JsonRequired]
        [JsonProperty(PropertyName = "URL")]
        public Uri Uri { get; set; }

        // The following members are scraped from HTML and might be missing:

        [JsonProperty(PropertyName = "Immature")]
        public int? ImmatureTicketCount { get; set; }

        [JsonProperty(PropertyName = "Live")]
        public int? LiveTicketCount { get; set; }

        [JsonProperty(PropertyName = "Voted")]
        public int? TotalVotes { get; set; }

        [JsonProperty(PropertyName = "Missed")]
        public int? TotalMissedTickets { get; set; }

        [JsonProperty(PropertyName = "PoolFees")]
        [JsonConverter(typeof(DecimalPercentageConverter))]
        public decimal? Fee { get; set; }

        [JsonProperty(PropertyName = "ProportionLive")]
        public decimal? ProportionLive { get; set; }

        [JsonProperty(PropertyName = "UserCount")]
        public int? UserCount { get; set; }
    }
}
