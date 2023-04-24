namespace Books.Models
{
    public class EmailSettings
    {
        public string  FromAddress { get; set; }
        public string  FromPassword { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
    }
}
