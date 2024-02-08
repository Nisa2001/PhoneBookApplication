using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace PhoneBookApplication.Models
{

    [Table("Contact")]
    public class Contact
    {

        [Key]
        [Required]
        public int ContactId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public string Department { get; set; }

        public string Titles { get; set; }

    }
}
