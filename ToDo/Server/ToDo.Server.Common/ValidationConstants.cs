namespace ToDo.Server.Common
{
    public static class ValidationConstants
    {
        //[MaxLength(25, ErrorMessage = "Email lenght must be between 7 and 25")]
        //[MinLength(7, ErrorMessage = "Email lenght must be between 7 and 25")]
        public const string ExpireDateFormat = "d MM yyyy";
        public const string PasswordProperty = "Password";
        public const int TitleMinLenght = 3;
        public const int TitleMaxLenght = 30;
        public const int ContentMinLenght = 1;
        public const int ContentMaxLenght = 100;
        public const int UsernameMinLenght = 1;
        public const int UsernameMaxLenght = 30;
        public const string EmailRegexPattern = @"(([^<>()\[\]\\.,;:\s@""]+(\.[^<>()\[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$";
    }
}
