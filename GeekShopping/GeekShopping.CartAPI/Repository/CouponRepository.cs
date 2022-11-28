﻿using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Utils;
using System.Net;
using System.Text.Json;

namespace GeekShopping.CartAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly HttpClient _client;
        public const string BasePath = "api/v1/coupon";

        public CouponRepository(HttpClient client)
        {
            _client = client;
        }

        public async Task<CouponVO> GetCoupon(string couponCode, string token)
        {
            _client.SetHeaderRequestToken(token);
            var response = await _client.GetAsync($"{BasePath}/{couponCode}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK) 
                return new CouponVO();

            return JsonSerializer.Deserialize<CouponVO>(
                    content, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
        }
    }
}