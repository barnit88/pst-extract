namespace core.NDBLayer.Pages.Base
{
    public class Page
    {
        public virtual short PageSize { get; set; } = 512;//512 bytes of data
        public PageTrailer PageTrailer { get; set; }
    }
}
