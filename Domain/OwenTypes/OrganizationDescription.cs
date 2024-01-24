namespace Domain.OwenTypes;

public class OrganizationDescription
{
    public OrganizationDescription()
    {

    }
    public OrganizationDescription(string organizationCode, string organizationName)
    {
        OrganizationCode = organizationCode;
        OrganizationName = organizationName;
    }

    public string OrganizationCode { get; set; } = default!;
    public string OrganizationName { get; set; } = default!;
}
