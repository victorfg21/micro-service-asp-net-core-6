﻿using GeekShopping.OrderAPI.Model;
using GeekShopping.OrderAPI.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.OrderAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContextOptions<MySQLContext> _context;

        public OrderRepository(DbContextOptions<MySQLContext> context)
        {
            _context = context;
        }

        public async Task<bool> AddOrder(OrderHeader header)
        {
            if (header == null)
                return false;

            await using var db = new MySQLContext(_context);
            db.Headers.Add(header);
            await db.SaveChangesAsync();

            return true;
        }

        public async Task UpdateOrderPaymentStatus(long orderHeaderId, bool status)
        {
            await using var db = new MySQLContext(_context);
            var header = await db.Headers.FirstOrDefaultAsync(x => x.Id == orderHeaderId);

            if (header != null)
            {
                header.PaymentStatus = status;
                await db.SaveChangesAsync();
            }
        }
    }
}
