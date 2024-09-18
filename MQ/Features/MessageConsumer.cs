using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQ.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Stream.Client;
using RabbitMQ.Stream.Client.Reliable;

namespace MQ.Features;
public class MessageConsumerService(ILogger<MessageConsumerService> logger, IOptions<SlidingRabbitMQConfigurationOptions> options) : IMessageConsumer
{

    private readonly ILogger<MessageConsumerService> _logger = logger;
    private readonly SlidingRabbitMQConfigurationOptions _options = options.Value;
    private ConnectionFactory? Factory { get; set; }
    private IConnection? Connection { get; set; }
    private IModel? Channel { get; set; }

    private async Task<List<IQueueInfo>> GetAvailableQueues(string postfix)
    {
        using HttpClient client = new();

        var byteArray = Encoding.ASCII.GetBytes($"{_options.UserName}:{_options.Password}");

        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

        HttpResponseMessage response = await client.GetAsync(_options.URL);

        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();

            List<IQueueInfo> queues = JsonSerializer.Deserialize<List<IQueueInfo>>(content) ?? [];

            return queues.Where(q => q.Name.ToLower().EndsWith(postfix)).ToList();
        }

        return [];
    }

    public async Task Register(Action<string, string> callback, string postfix)
    {
        try
        {
            _logger.LogInformation("Start Queue Register");

            List<IQueueInfo> queueInfos = await GetAvailableQueues(postfix);

            _logger.LogInformation($"Found {queueInfos.Count} queues");

            if (queueInfos.Count <= 0)
                return;

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

            foreach (var queue in queueInfos)
            {
                _logger.LogInformation($"Start Queue Register {_options.HostName}-{_options.PortNumber}-{_options.UserName}-{queue.Name}");

                var consumer = new EventingBasicConsumer(Channel);

                consumer.Received += (model, eventArgs) =>
                {
                    var body = eventArgs.Body.ToArray();

                    var message = Encoding.UTF8.GetString(body);

                    var routingKey = eventArgs.RoutingKey;

                    _logger.LogInformation($"Received message on queue {eventArgs.RoutingKey}: {message}");

                    callback(routingKey, message);
                };

                Channel.BasicConsume(queue: queue.Name, autoAck: true, consumer: consumer);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
        }
    }

    //public async Task StreamRegister(Action<string, string, ulong> callback, ulong offset = 0)
    //{
    //    try
    //    {
    //        _logger.LogInformation("Start Stream Register " + _options.StreamHostname + " " + _options.StreamPortNumber + " " + _options.UserName);

    //        StreamSystemConfig streamSystemConfig = new()
    //        {
    //            UserName = _options.UserName,
    //            Password = _options.Password,
    //            Endpoints = [new IPEndPoint(IPAddress.Parse(_options.StreamHostname), _options.StreamPortNumber)]
    //        };

    //        var streamSystem = await StreamSystem.Create(streamSystemConfig)
    //            .ConfigureAwait(false);

    //        _logger.LogInformation("Start Stream Consuming from offset " + offset);

    //        ConsumerConfig consumerConfig = new(streamSystem, _options.StreamName)
    //        {
    //            OffsetSpec = new OffsetTypeOffset(offset),
    //            MessageHandler = async (stream, consumer, context, message) =>
    //            {
    //                string routingKey = Convert.ToString(message.Annotations.ContainsKey("x-routing-key") ? message.Annotations["x-routing-key"] : "") ?? "";

    //                _logger.LogInformation("routing key " + routingKey);

    //                await consumer.StoreOffset(context.Offset);

    //                string receivedMessage = Encoding.Default.GetString(message.Data.Contents.ToArray());

    //                callback(routingKey, receivedMessage, context.Offset);

    //                await Task.CompletedTask.ConfigureAwait(false);
    //            }
    //        };

    //        var streamconsumer = await Consumer.Create(consumerConfig)
    //            .ConfigureAwait(false);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex.ToString());
    //    }
    //}

    public void Deregister()
    {
        Connection?.Close();
    }
}
