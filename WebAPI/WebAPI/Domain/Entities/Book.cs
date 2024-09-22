using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Domain.Exceptions;

namespace WebAPI.Domain.Entities
{
    [Table("Books")]
    public class Book
    {
        [Key]
        [Column("BookID")]
        public int BookID { get; set; }

        [Column("ISBN")]
        public string ISBN { get; set; }

        [Column("BookTitle")]
        public string BookTitle { get; set; }

        [Column("Genre")]
        public string Genre { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("AuthorID")]
        public int? AuthorID { get; set; }

        [Column("PickUpTime")]
        public DateTime? PickUpTime { get; set; }

        [Column("ReturnTime")]
        public DateTime? ReturnTime { get; set; }

        [Column("Image")]
        public byte[]? Image { get; set; }

        [Column("IsAvailable")]
        public int IsAvailable { get; set; } = 1;

        [Column("UserID")]
        public int? UserID { get; set; }
    }
}
