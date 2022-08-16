using GeekShopping.CartAPI.Data.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CartAPI.Model.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; } = null!;

        public DbSet<CartDetail> CartDetails { get; set; } = null!;

        public DbSet<CartHeader> CartHeaders { get; set; } = null!;
    }
}
