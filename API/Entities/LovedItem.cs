using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("LovedItems")]
    public class LovedItem
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int LovedId { get; set; }
        public Loved Loved { get; set; }
    }
}
