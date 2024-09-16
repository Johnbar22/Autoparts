using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Shop_ex.Models
{
    [Keyless]
    public class LoginModel
    {
        [Display(Name = "Введите адрес эл.почты")]
        [DataType(DataType.EmailAddress)]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Длина строки быть не менее 5 символов")]
        [Required(ErrorMessage = "Поле необходимо заполнить")]
        [EmailAddress(ErrorMessage = "Введите корректный адрес электронной почты")]
        public string? Login { get; set; }

        [Display(Name = "Введите пароль")]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Длина строки быть не менее 3 символов")]
        [Required(ErrorMessage = "Поле необходимо заполнить")]
        public string? Password { get; set; }
	}
}
