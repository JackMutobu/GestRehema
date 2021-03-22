namespace GestRehema.Entities
{
    public class ArticleSupplier
    {


        public int ArticleId { get; set; }
        public Article? Article { get; set; }

        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
        
    }
}
