// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mediator;

namespace MagicCouponApi;

public class PingReq : IRequest<PongRes>
{
    public string? Name { get; set; }
}

public sealed record PongRes(string Value);

public sealed class PingHandler : IRequestHandler<PingReq, PongRes>
{
    public ValueTask<PongRes> Handle(PingReq request, CancellationToken cancellationToken)
    {
        return new ValueTask<PongRes>(new PongRes(request.Name!));
    }
}
