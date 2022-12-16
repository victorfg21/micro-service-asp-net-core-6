using GeekShopping.Email.Messages;
using GeekShopping.Email.Model;
using GeekShopping.Email.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Email.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly DbContextOptions<MySQLContext> _context;

        public EmailRepository(DbContextOptions<MySQLContext> context)
        {
            _context = context;
        }

        public async Task LogEmail(UpdatePaymentResultMessage message)
        {
            EmailLog email = new EmailLog
            {
                Email = message.Email,
                SentDate = DateTime.Now,
                Log = $"Order - {message.OrderId}"
            };

            await using var db = new MySQLContext(_context);
            db.Emails.Add(email);
            await db.SaveChangesAsync();
        }
    }
}
