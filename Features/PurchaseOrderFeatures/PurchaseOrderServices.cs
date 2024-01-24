using Contracts.PurchaseOrderFeatures;
using Contracts.Shared;
using Contracts.ViewModels.PurchaseOrder;
using Domain.Entities;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Throw;

namespace Features.PurchaseOrderFeatures;

public class PurchaseOrderServices : IPurchaseOrderServices
{
    private readonly ApplicationDbContext _context;

    public PurchaseOrderServices(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<object?> GetLastPurchaseOrder(CancellationToken ct)
    {
        return await _context.PurchaseOrders.OrderByDescending(x => x.DateCreated).FirstOrDefaultAsync(x => true, ct);
    }

    public async Task<object?> GetPurchaseOrders(CancellationToken ct)
    {
        return await _context.PurchaseOrders.OrderByDescending(x => x.DateCreated).ToListAsync(ct);
    }

    public async Task<int> InsertBulk(CancellationToken ct)
    {
        List<PurchaseOrder> purchaseOrders = new();

        if (!await _context.Vendors.AnyAsync(ct))
            await _context.Vendors.AddAsync(new()
            {
                DateCreated = DateTime.Now,
                Id = "V1",
                IsActive = true,
                IsDeleted = false,
                ModifiedBy = "Seeding"
            }, ct);

        await _context.SaveChangesAsync(ct);

        for (int i = 0; i < 100000; i++)
        {
            purchaseOrders.Add(new()
            {
                ApprovalStatus = $"ApprovalStatus_{i}",
                Date = DateTime.Now,
                DateCreated = DateTime.Now,
                DocType = new($"docCode_{i}", $"docName_{i}"),
                IsActive = true,
                IsDeleted = false,
                ModifiedBy = "Seeding",
                Organization = new($"orgCode_{i}", $"orgName_{i}"),
                TotalValue = i,
                VendorCode = "V1",
                Id = $"PO{i.ToString().PadLeft(8, '0')}"
            });
        }

        await _context.AddRangeAsync(purchaseOrders, ct);

        return await _context.SaveChangesAsync(ct);
    }

    public async Task<int> InsertPO(CreatePurchaseOrderDto request, CancellationToken ct)
    {
        request.Id = $"PO{request.Index.ToString().PadLeft(8, '0')}";

        PurchaseOrder? purchaseOrder = await _context.PurchaseOrders.FindAsync(request.Id);

        purchaseOrder?.Throw(x => new BusinessException("PO already exist")).IfNotNull(x => x);

        purchaseOrder = new()
        {
            ApprovalStatus = $"ApprovalStatus_{request.Index}",
            Date = DateTime.Now,
            DateCreated = DateTime.Now,
            DocType = new($"docCode_{request.Index}", $"docName_{request.Index}"),
            IsActive = true,
            IsDeleted = false,
            ModifiedBy = "Seeding",
            Organization = new($"orgCode_{request.Index}", $"orgName_{request.Index}"),
            TotalValue = request.Index,
            VendorCode = "V1",
            Id = request.Id
        };

        await _context.AddAsync(purchaseOrder, ct);

        return await _context.SaveChangesAsync(ct);
    }
}
