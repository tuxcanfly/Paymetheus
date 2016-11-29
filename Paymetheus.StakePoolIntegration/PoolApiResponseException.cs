// Copyright(c) 2016 The Decred developers
// Licensed under the ISC license.  See LICENSE file in the project root for full license information.

using System;

namespace Paymetheus.StakePoolIntegration
{
    public class PoolApiResponseException : Exception
    {
        public PoolApiResponseException(string status, string message) : base(message)
        {
            Status = status;
        }

        public string Status { get; }
    }
}
