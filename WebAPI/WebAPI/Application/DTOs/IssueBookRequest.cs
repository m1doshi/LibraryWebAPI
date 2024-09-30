namespace WebAPI.Application.DTOs
{
    public class IssueBookRequest
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
