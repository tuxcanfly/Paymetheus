// Copyright (c) 2017 The Decred developers
// Licensed under the ISC license.  See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paymetheus.Framework
{
    /// <summary>
    /// INotifyCompletion allows an object (such as a dialog frame) to notify the calling code that
    /// created the object that a task has been completed.  The object must implement a
    /// TaskCompletionSource getter property that can be used to signal the completion.  The calling
    /// code should await on the completion source's task.  This is only used to notify completion,
    /// and the generic TaskCompletionSource type paramter should always be null.
    /// </summary>
    public interface INotifyCompletion
    {
        TaskCompletionSource<object> NotifyCompletionSource { get; }
    }
}
