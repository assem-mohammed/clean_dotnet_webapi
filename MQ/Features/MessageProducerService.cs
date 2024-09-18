using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQ.Contracts;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;

namespace MQ.Features;

public class MessageProducerService(ILogger<MessageProducerService> logger, IOptions<SlidingRabbitMQConfigurationOptions> options) : IMessageProducer
{
    private readonly SlidingRabbitMQConfigurationOptions _options = options.Value;
    private IConnection Connection { get; set; } = default!;
    private IModel Channel { get; set; } = default!;
    private ConnectionFactory Factory { get; set; } = default!;

    public void SendMessage<T>(T message, string routingKey, string exchange = "", string queue = "")
    {
        logger.LogInformation("Start Connecting to queue for message publish");

        Factory = new ConnectionFactory
        {
            HostName = _options.HostName,
            Port = _options.PortNumber,
            VirtualHost = _options.VirtualHost,
            UserName = _options.UserName,
            Password = _options.Password
        };

        Connection = Factory.CreateConnection();

        Channel = Connection.CreateModel();

        logger.LogInformation("Attempting Binding to Queue " + queue);

        using (Connection)
        {
            Channel.QueueBind(queue, exchange, routingKey, new Dictionary<string, object>());

            Channel.BasicAcks += (sender, eventArgs) =>
            {
                logger.LogInformation("Message published successfully!");
                //implement ack handle
            };

            Channel.ConfirmSelect();

            logger.LogInformation("Successfully connected to Queue: " + queue + " Exchange: " + exchange + " on Host: " + Factory.HostName);

            var json = JsonSerializer.Serialize(message);

            var body = Encoding.UTF8.GetBytes(json);

            logger.LogInformation("Publishing JSON message: " + json);

            Channel.BasicPublish(exchange: exchange, routingKey: routingKey, body: body);

            Channel.WaitForConfirmsOrDie();
        }
    }
}