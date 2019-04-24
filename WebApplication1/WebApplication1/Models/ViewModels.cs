using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class BatchViewModels
    {
        [Required]
        [RegularExpression("^[12][0-9]{3}$", ErrorMessage = "Batch must be numeric")]
        public string BatchName { get; set; }
    }
    public class CourseViewModels
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9_]{1,10}$", ErrorMessage = "Input must be alphanumeric")]
        public string CourseName { get; set; }
    }

    public class PersonEmployeeViewModels
    {
        public int ID { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Input must be Alphabets")]
        public string Name { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Input must be Alphabets")]
        public string FatherName { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Input must be numeric")]
        public string CNIC { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [RegularExpression(@"^\(?([0-9]{4})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{4})$", ErrorMessage = "Invalid contact")]
        public string Contact { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Input must be Alphabets")]
        public string Designation { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Input must be numeric")]
        public string Salary { get; set; }

    }

    
    
}