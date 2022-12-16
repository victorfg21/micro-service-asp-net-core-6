using GeekShopping.OrderAPI.Messages;
using GeekShopping.OrderAPI.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GeekShopping.OrderAPI.MessageConsumer
{
    public class RabbitMQPaymentConsumer : BackgroundService
    {
        private readonly OrderRepository _orderRepository;
        private IConnection _connection;
        private IModel _channel;

        private const string ExchangeName = "DirectPaymentUpdateExchange";
        private const string PaymentOrderUpdateQueueName = "PaymentOrderUpdateQueueName";
        private const string RoutingKeyOrder = "PaymentOrder";

        public RabbitMQPaymentConsumer(OrderRepository orderRepository)
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
            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
            _channel.QueueDeclare(PaymentOrderUpdateQueueName, false, false, false, null);
            _channel.QueueBind(PaymentOrderUpdateQueueName, ExchangeName, RoutingKeyOrder);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (channel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                UpdatePaymentResultVO vo = JsonSerializer.Deserialize<UpdatePaymentResultVO>(content);
                UpdatePaymentStatus(vo).GetAwaiter().GetResult();
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume(PaymentOrderUpdateQueueName, false, consumer);
            return Task.CompletedTask;
        }

        private async Task UpdatePaymentStatus(UpdatePaymentResultVO vo)
        {
            try
            {
                await _orderRepository.UpdateOrderPaymentStatus(vo.OrderId, vo.Status);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
