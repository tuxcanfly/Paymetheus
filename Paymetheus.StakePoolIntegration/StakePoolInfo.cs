// Copyright (c) 2016 The Decred developers
// Licensed under the ISC license.  See LICENSE file in the project root for full license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Paymetheus.StakePoolIntegration
{
    public sealed class StakePoolInfo
    {
        [JsonRequired]
        [JsonProperty(PropertyName = "url")]
        public Uri Uri { get; set; }

        [JsonRequired]
        [JsonProperty(PropertyName = "Immature")]
        public int ImmatureTicketCount { get; set; }

        [JsonRequired]
        [JsonProperty(PropertyName = "Live")]
        public int LiveTicketCount { get; set; }

        [JsonRequired]
        [JsonProperty(PropertyName = "Voted")]
        public int TotalVotes { get; set; }

        [JsonRequired]
        [JsonProperty(PropertyName = "Missed")]
        public int TotalMissedTickets { get; set; }

        [JsonProperty(PropertyName = "PoolFees")]
        [JsonConverter(typeof(DecimalPercentageConverter))]
        public decimal Fee { get; set; }

        [JsonRequired]
        public decimal ProportionLive { get; set; }

        [JsonRequired]
        public int UserCount { get; set; }

        private class DecimalPercentageConverter : JsonConverter
        {
            public override bool CanRead => true;

            public override bool CanConvert(Type objectType)
            {
                throw new NotImplementedException();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return new JValue(reader.Value).ToObject<decimal>() / 100;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
    }
}
