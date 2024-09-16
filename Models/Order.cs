using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shop_ex.Models
{
    public class Order
    {
        [BindNever]
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int CartId {  get; set; }
        public Cart? Cart { get; set; }

        [Display(Name = "Введите ФИО")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Длина строки должна быть не менее 5 символов")]
        [Required(ErrorMessage = "Поле необходимо заполнить")]
        public string? UserName { get; set; }


        [Display(Name = "Введите номер телефона")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(12, MinimumLength = 10, ErrorMessage = "Формат для ввода: 89994441133")]
        [Required(ErrorMessage = "Поле необходимо заполнить")]
        [Phone(ErrorMessage = "Допускается ввод только цифр")]
        public string UserPhone { get; set; }


        [Display(Name = "Введите адрес эл.почты")]
        [DataType(DataType.EmailAddress)]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Длина строки быть не менее 5 символов")]
        [Required(ErrorMessage = "Поле необходимо заполнить")]
        [EmailAddress(ErrorMessage = "Введите корректный адрес электронной почты")]
        public string UserEmail { get; set; }


        public int? UserId { get; set; }
        public User? Users { get; set; }

    }
}
