// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using MagicCouponApi.Models;

namespace MagicCouponApi.Data;

public static class CouponStore
{
    private static readonly List<Coupon> s_coupons = new()
    {
        new Coupon(Id:1,Name:"10CFF",Percent:10,IsActive:true,Created: DateTime.Now,LastUpdated:DateTime.Now),
        new Coupon(Id:2,Name:"10BRF",Percent:10,IsActive:true,Created: DateTime.Now,LastUpdated:DateTime.Now),
        new Coupon(Id:3,Name:"10AFF",Percent:10,IsActive:true,Created: DateTime.Now,LastUpdated:DateTime.Now)
    };

    public static List<Coupon> CouponList() => s_coupons;
}
