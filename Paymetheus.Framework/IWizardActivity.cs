// Copyright (c) 2016 The Decred developers
// Licensed under the ISC license.  See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace Paymetheus.Framework
{
    public interface IWizardActivity
    {
        Task RunActivityAsync();
    }
}
