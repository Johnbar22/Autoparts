using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Shop_ex.Models
{
    public class User
    {
		[Display(Name = "Введите ФИО")]
		[StringLength(30, MinimumLength = 5, ErrorMessage = "Длина строки должна быть не менее 5 символов")]
		[Required(ErrorMessage = "Поле необходимо заполнить")]
		public string Username { get; set; }

		[Display(Name = "Введите адрес эл.почты")]
		[DataType(DataType.EmailAddress)]
		[StringLength(30, MinimumLength = 5, ErrorMessage = "Длина строки быть не менее 5 символов")]
		[Required(ErrorMessage = "Поле необходимо заполнить")]
		[EmailAddress(ErrorMessage = "Введите корректный адрес электронной почты")]
		public string Email { get; set; }

        public string Password { get; set; }

        public int Id { get; set; }

        [Display(Name = "Введите номер телефона")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(12, MinimumLength = 10, ErrorMessage = "Формат для ввода: 89994441133")]
        [Required(ErrorMessage = "Поле необходимо заполнить")]
        [Phone(ErrorMessage = "Допускается ввод только цифр")]
        public string? Telephone { get; set; }
        public int RoleId { get; set; }
        public Role? Role { get; set; }
        public ICollection<Order>? Orders { get; set; }

        public string? ResetPasswordToken {  get; set; }
        public DateTime? ResetPasswordTokenExpiry { get; set; }

    }
}
