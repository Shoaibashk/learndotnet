// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace MagicCouponApi.Models;

public sealed record Coupon(int Id, string? Name, int Percent, bool IsActive, DateTime? Created, DateTime? LastUpdated);
