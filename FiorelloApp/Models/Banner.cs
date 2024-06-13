namespace FiorelloApp.Models
{
    public class Banner : BaseEntity
    {
        public string VideoUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<BannerContent> BannerContents { get; set; }
    }
}
