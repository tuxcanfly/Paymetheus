// Copyright(c) 2016 The Decred developers
// Licensed under the ISC license.  See LICENSE file in the project root for full license information.

namespace Paymetheus.StakePoolIntegration
{
    // Pool API uses same status codes and meanings as gRPC.
    // See https://godoc.org/google.golang.org/grpc/codes for documentation.
    public enum StatusCode
    {
        Ok = 0,
        Canceled = 1,
        Unknown = 2,
        InvalidArgument = 3,
        DeadlineExceeded = 4,
        NotFound = 5,
        AlreadyExists = 6,
        PermissionDenied = 7,
        Unauthenticated = 16,
        ResourceExhausted = 8,
        FailedPrecondition = 9,
        Aborted = 10,
        OutOfRange = 11,
        Unimplemented = 12,
        Internal = 13,
        Unavailable = 14,
        DataLoss = 15,
    }
}
