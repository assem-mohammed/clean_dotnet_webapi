using Contracts.PurchaseOrderFeatures;
using FluentValidation;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Resources.ErrorLocalization;
using System.Globalization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IStringLocalizer<Error> _localizer;
        private readonly TimezoneHandler _timezoneHandler;

        public HomeController(IStringLocalizer<Error> localizer, TimezoneHandler timezoneHandler)
        {
            _localizer = localizer;
            _timezoneHandler = timezoneHandler;
        }

        [HttpGet("TestCulture")]
        public object TestCulture(CancellationToken ct)
        {
            var utcDate = DateTime.UtcNow;
            
            var localDate = TimeZoneInfo.ConvertTimeFromUtc(utcDate, TimeZoneInfo.Local);
            
            var timeZoneDate = TimeZoneInfo.ConvertTimeFromUtc(utcDate, TimeZoneInfo.FindSystemTimeZoneById(_timezoneHandler.TimezoneId));
            
            return new
            {
                Text = _localizer["Test"].Value,
                UTC = utcDate,
                Local = localDate,
                CultureDate = timeZoneDate,
                CultureInfo = CultureInfo.CurrentCulture.TextInfo.CultureName
            };
        }

    }
}
