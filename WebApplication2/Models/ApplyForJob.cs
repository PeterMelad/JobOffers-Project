using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class ApplyForJob
    // el relation between class job w user hya many-to-many , f 3mlna el class da 'ApplyForJob' .. el 'user' hwa el 48s ele hyt2dm l el job(applier)
    {
        public int id { get; set; }
        public string message { get; set; }
        public DateTime dateTime { get; set; }
        public int jobid { get; set; }
        public string userid { get; set; }
        public virtual Job job { get; set; }
        public virtual ApplicationUser user { get; set; }
    }
}