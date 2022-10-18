using GeekShopping.OrderAPI.Messages;
using GeekShopping.OrderAPI.Model;
using GeekShopping.OrderAPI.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GeekShopping.OrderAPI.MessageConsumer
{
    public class RabbitMQCheckoutConsumer : BackgroundService
    {
        private readonly OrderRepository _orderRepository;
        private IConnection _connection;
        private IModel _channel;
        private readonly string QueueName = "checkoutqueue";

        public RabbitMQCheckoutConsumer(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                VirtualHost = "main",
                UserName = "mc",
                Password = "mc2",
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(QueueName, false, false, false);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (channel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                CheckoutHeaderVO vo = JsonSerializer.Deserialize<CheckoutHeaderVO>(content);
                ProcessOrder(vo).GetAwaiter().GetResult();
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume(QueueName, false, consumer);
            return Task.CompletedTask;
        }

        private async Task ProcessOrder(CheckoutHeaderVO model)
        {
            OrderHeader order = new()
            {
                UserId = model.UserId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                OrderDetails = new List<OrderDetail>(),
                CardNumber = model.CardNumber,
                CouponCode = model.CouponCode,
                CVV = model.CVV,
                DiscountAmount = model.DiscountAmount,
                Email = model.Email,
                ExpiryMonthYear = model.ExpiryMonthYear,
                OrderTime = DateTime.Now,
                PurchaseAmount = model.PurchaseAmount,
                PaymentStatus = false,
                Phone = model.Phone,
                DateTime = model.DateTime
            };

            foreach (var details in model.CartDetails)
            {
                OrderDetail detail = new()
                {
                    ProductId = details.ProductId,
                    ProductName = details.Product.Name,
                    Price = details.Product.Price,
                    Count = details.Count
                };

                order.CartTotalItens += details.Count;
                order.OrderDetails.Add(detail);
            }

            await _orderRepository.AddOrder(order);
        }
    }
}
