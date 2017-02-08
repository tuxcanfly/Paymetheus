// Copyright (c) 2017 The Decred developers
// Licensed under the ISC license.  See LICENSE file in the project root for full license information.

using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Paymetheus.StakePoolIntegration
{
    public class StakePoolUserConfig
    {
        public class Entry
        {
            public string Host { get; set; }
            public string ApiKey { get; set; }
            public string MultisigVoteScript { get; set; }
            public uint VotingAccount { get; set; }
        }

        public Entry[] Entries { get; set; }

        public static async Task<StakePoolUserConfig> ReadConfig(JsonSerializer serializer, StreamReader sr)
        {
            using (var jsonReader = new JsonTextReader(new StringReader(await sr.ReadToEndAsync())))
            {
                return serializer.Deserialize<StakePoolUserConfig>(jsonReader);
            }
        }

        public async Task WriteConfig(JsonSerializer serializer, StreamWriter stream)
        {
            using (var sw = new StringWriter())
            {
                using (var jsonWriter = new JsonTextWriter(sw))
                {
                    serializer.Serialize(jsonWriter, this);
                }
                await stream.WriteAsync(sw.ToString());
            }
        }
    }
}
