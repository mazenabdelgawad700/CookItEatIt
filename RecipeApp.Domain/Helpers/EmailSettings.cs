namespace RecipeApp.Domain.Helpers
{
    public class EmailSettings
    {
        public string ClientConnectionString { get; set; } = null!;
        public int ClientConnectionPort { get; set; }
        public bool ClientConnectionUseSSL { get; set; }
        public string AuthenticationEmail { get; set; } = null!;
        public string AuthenticationPassword { get; set; } = null!;
        public string BodyBuilderTextBody { get; set; } = null!;
        public string MailBoxSenderHeader { get; set; } = null!;
        public string MailBoxSubject { get; set; } = null!;
    }
}