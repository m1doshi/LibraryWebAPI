namespace WebAPI.Models
{
    public class AuthorModel
    {
        public int AuthorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly Birthday { get; set; }
        public string Country { get; set; }
    }
}
