using System.ComponentModel.DataAnnotations;

namespace WebAPI.Application.DTOs
{
    public class AuthorModel
    {
        public int AuthorID { get; set; }

        [Required(ErrorMessage = "First name is requierd.")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Firstname length can't be more than 20.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "First name is requierd.")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Lastname length can't be more than 20.")]
        public string LastName { get; set; }
        public DateOnly? Birthday { get; set; }

        [StringLength(20, ErrorMessage = "Country length can't be more than 20.")]
        public string? Country { get; set; }
    }
}
