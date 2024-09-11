namespace WebAPI.Models
{
    public class UpdateAuthorRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly Birthday { get; set; }
        public string Country { get; set; }
    }
}
