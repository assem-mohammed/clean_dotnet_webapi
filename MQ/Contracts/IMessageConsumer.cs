using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Contracts;
public interface IMessageConsumer
{
    Task Register(Action<string, string> callback, string postfix);
    void Deregister();
    //Task StreamRegister(Action<string, string, ulong> callback, ulong offset = 0);
}
