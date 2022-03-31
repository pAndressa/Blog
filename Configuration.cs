namespace Blog
{
    public static class Configuration
    {
        public static string JwtKey = "dGABzagi8kySMgJULJBmsw==";
        public static string ApiKeyName = "api_name";
        public static string ApiKey = "curso_testexyz01256w==";
        public static SmtpConfiguration Smtp = new();

        public class SmtpConfiguration
        {
            public string Host { get; set; }
            public int Porta { get; set; } = 25;
            public string UserName { get; set; }
            public string Password { get; set; }
        }
    }
}