﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    public class Product_CommnetMetaData
    {
        [Key]
        public int CommnetID { get; set; }
        public int ProductID { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Name { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        public string Email { get; set; }

        [Display(Name = "وب سایت")]
        public string WebSite { get; set; }

        [Display(Name = "متن نظر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Commnet { get; set; }
    }
}