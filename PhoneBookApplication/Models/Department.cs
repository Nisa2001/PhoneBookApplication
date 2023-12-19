using System.ComponentModel.DataAnnotations;

namespace PhoneBookApplication.Models
{
    public class Department
    {

        [Key]
        [Required]
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public int ContactId { get; set; }
        public virtual Contact Contact { get; set; }
    }
}
