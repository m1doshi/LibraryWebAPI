using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Core.Entities
{
    [Table("Authors")]
    public class Author
    {
        [Key]
        [Column("AuthorID")]
        public int AuthorID { get; set; }

        [Column("FirstName")]
        public string FirstName { get; set; }

        [Column("LastName")]
        public string LastName { get; set; }

        [Column("Birthday")]
        public DateOnly? Birthday { get; set; }

        [Column("Country")]
        public string? Country { get; set; }
    }
}
