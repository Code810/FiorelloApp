namespace FiorelloApp.Models
{
    public class BannerContent : BaseEntity
    {
        public string Content { get; set; }
        public int BannerId { get; set; }
        public Banner Banner { get; set; }
    }
}
