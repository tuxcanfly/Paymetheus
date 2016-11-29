// Copyright (c) 2016 The Decred developers
// Licensed under the ISC license.  See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paymetheus.StakePoolIntegration
{
    public sealed class PoolUserInfo
    {
        public string FeeAddress { get; set; }

        public decimal Fee { get; set; }

        public string RedeemScriptHex { get; set; }

        public string VotingAddress { get; set; }
    }
}
