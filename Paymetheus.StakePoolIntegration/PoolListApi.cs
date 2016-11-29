// Copyright (c) 2016 The Decred developers
// Licensed under the ISC license.  See LICENSE file in the project root for full license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Paymetheus.StakePoolIntegration
{
    public static class PoolListApi
    {
        private static readonly Uri StakePoolInfoUri = new Uri("https://decred.org/api/?c=gsd");

        public static async Task<Dictionary<string, StakePoolInfo>> QueryStakePoolInfoAsync(HttpClient client, JsonSerializer serializer)
        {
            using (var stream = await client.GetStreamAsync(StakePoolInfoUri))
            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                return serializer.Deserialize<Dictionary<string, StakePoolInfo>>(jsonReader);
            }
        }
    }
}
