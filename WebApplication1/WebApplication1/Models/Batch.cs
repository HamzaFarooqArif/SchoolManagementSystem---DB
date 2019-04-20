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
        [RegularExpression("^[12][0-9]{3}$", ErrorMessage = "UPRN must be numeric")]
        public string BatchName { get; set; }
    }
}