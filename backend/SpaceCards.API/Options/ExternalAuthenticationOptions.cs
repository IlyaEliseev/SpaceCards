namespace SpaceCards.API.Options
{
    public class ExternalAuthenticationOptions
    {
        public Google Google { get; set; }

        public MailRu MailRu { get; set; }
    }

    public class Google
    {
        public string ClientSecret { get; set; }

        public string ClientId { get; set; }
    }

    public class MailRu
    {
        public string ClientSecret { get; set; }

        public string ClientId { get; set; }
    }
}
