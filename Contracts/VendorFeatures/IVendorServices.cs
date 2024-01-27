using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.VendorFeatures
{
    public interface IVendorServices
    {
        Task<object> GetVendorById(string id, CancellationToken ct);
        Task<object> GetAllVendors(CancellationToken ct);
        Task<object> InsertBulk(CancellationToken ct);
    }
}
