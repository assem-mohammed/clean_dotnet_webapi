using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Contracts;
public class SlidingRabbitMQConfigurationOptions
{
    public string URL { get; set; } = default!;
    public string HostName { get; set; } = default!;
    public int PortNumber { get; set; }
    public string VirtualHost { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
}