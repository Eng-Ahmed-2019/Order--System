namespace OrderSystem.Application.Validators
{
    public static class PasswordValidator
    {
        public static bool IsStrongPassword(string password, out string errorMessage)
        {
            errorMessage = "";

            if (password.Length < 8)
            {
                errorMessage = "Password must be 8 characters";
                return false;
            }

            if (!password.Any(char.IsUpper))
            {
                errorMessage = "Password Must has an upper character";
                return false;
            }

            if (!password.Any(char.IsLower))
            {
                errorMessage = "Password must has an lower character";
                return false;
            }

            if (!password.Any(char.IsDigit))
            {
                errorMessage = "Password must has a number";
                return false;
            }

            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                errorMessage = "The password must contain a special symbol such as !@#$%";
                return false;
            }

            return true;
        }
    }
}