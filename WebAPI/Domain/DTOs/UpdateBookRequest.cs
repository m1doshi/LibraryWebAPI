namespace Core.DTOs
{
    public class UpdateBookRequest
    {
        public int BookID { get; set; }
        public string ISBN { get; set; }
        public string BookTitle { get; set; }
        public string Genre { get; set; }
        public string? Description { get; set; }
        public int? AuthorID { get; set; }
        public DateTime? PickUpTime { get; set; }
        public DateTime? ReturnTime { get; set; }
        public byte[]? Image { get; set; }
        public int IsAvailable { get; set; } = 1;
        public int? UserID { get; set; }
    }
}
