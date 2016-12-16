// Copyright(c) 2016 The Decred developers
// Licensed under the ISC license.  See LICENSE file in the project root for full license information.

using System;

namespace Paymetheus.StakePoolIntegration
{
    public class PoolApiResponseException : Exception
    {
        internal PoolApiResponseException(StatusCode code, string status, string message) : base($"{message} (code={code})")
        {
            Code = code;
            Status = status;
        }

        public StatusCode Code { get; }
        public string Status { get; }
    }
}
