namespace API.DTOs
{
    public class LovedDto
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public List<LovedItemDto> Items { get; set; }
    }
}
