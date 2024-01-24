namespace Domain.OwenTypes;

public class SiteDescription
{
    public SiteDescription()
    {

    }

    public SiteDescription(string siteCode, string siteName)
    {
        SiteCode = siteCode;
        SiteName = siteName;
    }
   public string SiteCode { get; set; } = default!;
    public string SiteName { get; set; } = default!;
}
