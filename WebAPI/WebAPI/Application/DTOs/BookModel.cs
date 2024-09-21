using System.ComponentModel.DataAnnotations;

namespace WebAPI.Application.DTOs
{
    public class BookModel
    {
        public int BookID { get; set; }

        [Required(ErrorMessage = "ISBN is required")]
        [RegularExpression(@"^\d{3}-\d{1}-\d{4}-\d{4}-\d{1}$",
            ErrorMessage = "ISBN must be in format: xxx-x-xxxx-xxxx-x, x - number")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Title length can't be more than 100 and less than 2.")]
        public string BookTitle { get; set; }

        [Required(ErrorMessage = "Genre is required")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Title length can't be more than 30 and less than 3.")]
        public string Genre { get; set; }

        [StringLength(300, ErrorMessage = "Description length can't be more than 300.")]
        public string? Description { get; set; }
        public int? AuthorID { get; set; }
        public DateTime? PickUpTime { get; set; }
        public DateTime? ReturnTime { get; set; }
        public byte[]? Image { get; set; }
        public int IsAvailable { get; set; } = 1;
        public int? UserID { get; set; }
    }
}
