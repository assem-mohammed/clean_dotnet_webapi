namespace API.Configurations
{
    public class SupportedCultureOptions
    {
        public static string CONFIG_KEY { get; set; } = "SupportedCulture";
        public List<string> Cultures { get; set; } = default!;
    }
}
