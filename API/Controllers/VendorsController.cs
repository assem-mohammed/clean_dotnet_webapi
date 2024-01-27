﻿using Contracts.VendorFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        private readonly IVendorServices _vendorServices;

        public VendorsController(IVendorServices vendorServices)
        {
            _vendorServices = vendorServices;
        }

        [HttpGet]
        public async Task<object> GetAllVendors(CancellationToken ct)
            => await _vendorServices.GetAllVendors(ct);

        [HttpGet("{id}")]
        public async Task<object> GetVendor(string id, CancellationToken ct)
            => await _vendorServices.GetVendorById(id, ct);

        [HttpPost]
        public async Task<object> InsertBulk(CancellationToken ct)
            => await _vendorServices.InsertBulk(ct);
    }
}
