// Copyright (c) 2016 The btcsuite developers
// Copyright (c) 2016 The Decred developers
// Licensed under the ISC license.  See LICENSE file in the project root for full license information.

using Paymetheus.Decred;
using System;

namespace Paymetheus
{
    class ProcessArguments
    {
        public BlockChainIdentity IntendedNetwork { get; }
        public bool SearchPathForWalletProcess { get; }
        public string ExtraWalletArgs { get; }

        private ProcessArguments(BlockChainIdentity intendedNetwork, bool searchPathForWalletProcess, string extraWalletArgs)
        {
            IntendedNetwork = intendedNetwork;
            SearchPathForWalletProcess = searchPathForWalletProcess;
            ExtraWalletArgs = extraWalletArgs;
        }

        public static ProcessArguments ParseArguments(string[] args)
        {
            var intendedNetwork = BlockChainIdentity.MainNet;
            var searchPathForWalletProcess = false;
            var extraWalletArgs = "";

            string rest;

            foreach (var arg in args)
            {
                if (arg == "-testnet")
                    intendedNetwork = BlockChainIdentity.TestNet;
                else if (arg == "-searchpath")
                    searchPathForWalletProcess = true;
                else if (TryTrimPrefix(arg, "-extrawalletargs=", out rest))
                    extraWalletArgs = rest;
                else
                    throw new Exception($"Unrecognized argument `{arg}`");
            }

            return new ProcessArguments(intendedNetwork, searchPathForWalletProcess, extraWalletArgs);
        }

        private static bool TryTrimPrefix(string s, string prefix, out string rest)
        {
            if (s.StartsWith(prefix))
            {
                rest = s.Substring(prefix.Length);
                return true;
            }
            else
            {
                rest = null;
                return false;
            }
        }
    }
}
