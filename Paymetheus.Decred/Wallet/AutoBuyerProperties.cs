// Copyright (c) 2016 The btcsuite developers
// Copyright (c) 2016 The Decred developers
// Licensed under the ISC license.  See LICENSE file in the project root for full license information.

namespace Paymetheus.Decred.Wallet
{
    public sealed class AutoBuyerProperties
    {
        public byte[] Passphrase { get; set; }
        public uint Account { get; set; }
        public long BalanceToMaintain { get; set; }
        public long MaxFeePerKb { get; set; }
        public double MaxPriceRelative { get; set; }
        public long MaxPriceAbsolute { get; set; }
        public string VotingAddress { get; set; }
        public string PoolAddress { get; set; }
        public double PoolFees { get; set; }
        public long MaxPerBlock { get; set; }
    }
}
