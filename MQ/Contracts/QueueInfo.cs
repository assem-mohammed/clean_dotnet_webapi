using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Contracts;
internal interface IQueueInfo
{
    public string Name { get; set; }
    public int Messages { get; set; }
}