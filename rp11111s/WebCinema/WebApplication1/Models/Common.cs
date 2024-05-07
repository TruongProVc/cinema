using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;
namespace WebApplication1.Models
{
    public class Common
    {
        static WebsiteCinemaEntities db = new WebsiteCinemaEntities();
        public static string CreateID()
        {
            string result = "";
            Random random = new Random();
            int id ;
            for (int i = 0; i < 10; i++)
            {
                id = random.Next(9);
                if(i==1 && id == 0)
                {
                    id = 1;
                }
                result += id;

                    
            }
            return result;
        }
    }
}