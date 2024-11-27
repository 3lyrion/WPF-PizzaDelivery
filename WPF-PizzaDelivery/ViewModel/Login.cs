using System;

namespace PizzaDelivery.ViewModel
{
    public class LoginFormData
    {
        public string PhoneNumber;
        public string Password;
    }

    public class RegistrationFormData : LoginFormData
    {
        public string PasswordConfirmation;
    }
}
