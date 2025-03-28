using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace day1lab.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public DateOnly DOB { get; set; }

        [ForeignKey("Department")]
        public int? DeptID { get; set; }

        public virtual Department? Department { get; set; }
    }
}
