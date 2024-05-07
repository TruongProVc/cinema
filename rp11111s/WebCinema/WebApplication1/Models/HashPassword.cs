﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication1.Models
{
    public class HashPassword
    {
        public static string SHA512HashPass(string password)
        {
            string result = "";
            using(SHA512 sha512 = SHA512.Create())
            {
                //Chuyển chuỗi thành mảng byte
                //Mã hóa mảng byte convert gán vào hasrs
                //Chuyển đôỉ mảng byte thành chuỗi

                byte[] convert = Encoding.UTF8.GetBytes(password);
                byte[] hashpw = sha512.ComputeHash(convert);
                result = BitConverter.ToString(hashpw);
            }
            return result;
        }
    }
}