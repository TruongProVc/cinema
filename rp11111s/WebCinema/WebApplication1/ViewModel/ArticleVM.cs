using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.ViewModel
{
    public class ArticleVM
    {
        [Required]
        public string articleName { set; get; }
        [Required]
        public string summaryContent { set; get; }
        [Required]
        public string content { set; get; }
        [Required]
        public HttpPostedFileBase avatarArticle { set; get; }
    }
}