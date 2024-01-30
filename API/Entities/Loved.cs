namespace API.Entities
{
    public class Loved
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public List<LovedItem> Items { get; set; } = new();

        public void AddItem(Product product)
        {
            if (Items.All(item => item.ProductId != product.Id))
            {
                Items.Add(new LovedItem { Product = product });
            }
        }

        public void RemoveItem(int productId)
        {
            var item = Items.FirstOrDefault(item => item.ProductId == productId);
            if (item == null) return;
            Items.Remove(item);
        }
    }
}
