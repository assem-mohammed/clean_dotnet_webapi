using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts;

public class BasePagedModel
{
    public int Size { get; set; }
    public int Length { get; set; }
}
