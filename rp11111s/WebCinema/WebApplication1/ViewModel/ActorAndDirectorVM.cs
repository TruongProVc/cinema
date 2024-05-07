using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.ViewModel
{
    public class ActorAndDirectorVM
    {
        [Required]
        public string name { set; get; }
        [Required]
        public bool gender { set; get; }
        [Required]
        public DateTime dateOfBirth { set; get; }
        public HttpPostedFileBase imageAvatar { set; get; }
    }
}