namespace Wego.Core.Models.StripeClasses
{
    public class StripeSettings : BaseModel
    {
        public string SecretKey { get; set; }
        public string PublishableKey { get; set; }
    }
}
