using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Job
    {
        public int id { get; set; }
        [Display(Name ="Job Name")]
        public string JobTitle { get; set; }
        [Display(Name ="Job Content")]
        public string JobContent { get; set; }
        [Display(Name ="Job Image")]
        public string JobImage { get; set; }

        [Display(Name ="Category Name")]
        public int CategoryId { get; set; }
        public string publisherId { get; set; }

        public virtual Category category { get; set; }
        public virtual ApplicationUser publisher { get; set; }

    }
}