using Microsoft.Extensions.DependencyInjection;
using MQ.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Contracts;

public static class SlidingRabbitMQExtensions
{
    public static IServiceCollection ConfigureSlidingRabbitMQ(this IServiceCollection services, Action<SlidingRabbitMQConfigurationOptions> options)
    {
        if (options == null)
            throw new ArgumentNullException(nameof(options), "Please Provide RabbitMQ Settings");

        services.Configure(options);

        services.AddTransient<IMessageProducer, MessageProducerService>();
        services.AddSingleton<IMessageConsumer, MessageConsumerService>();

        return services;
    }
}