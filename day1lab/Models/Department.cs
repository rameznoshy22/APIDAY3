using day1lab.CustomValidators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace day1lab.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Required]
        [Deptuniquename]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^(EG|US)$", ErrorMessage = "Location must be 'EG' or 'US'.")]
        public string location { get; set; }
        public string Manager { get; set; }

        public virtual List<Student>? Students { get; set; }=new List<Student>();
    }
}
