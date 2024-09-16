namespace Shop_ex.Models
{
    public class CartOrderModel
    {
        public Cart Cart { get; set; } = new Cart();
        public Order Order { get; set; } = new Order();
        public RegistrationModel RegistrationUser { get; set; } = new RegistrationModel();
        public ResetPasswordViewModel ResetPassword { get; set; } = new ResetPasswordViewModel();
    }
}
