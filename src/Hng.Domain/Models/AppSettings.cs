namespace Hng.Domain.Models
{
    public class AppSettings
    {
        public SettingsObject Settings { get; set; } = new SettingsObject();
        public ConnectionStringsObject ConnectionStrings { get; set; } = new ConnectionStringsObject();


        public class SettingsObject
        {
            public bool UseMockForDatabase { get; set; }
            public bool InTestMode { get; set; }
        }

        public class ConnectionStringsObject
        {
            public string DefaultConnectionString { get; set; } = null!;
        }
    }
}
