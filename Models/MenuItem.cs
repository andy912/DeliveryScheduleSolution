namespace DeliveryScheduleSolution.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public int? ParentId { get; set; }

        // 子選單
        public List<MenuItem> Children { get; set; } = new List<MenuItem>();
    }
}
