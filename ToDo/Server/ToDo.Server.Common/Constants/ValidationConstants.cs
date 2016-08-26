namespace ToDo.Server.Common.Constants
{
    public static class ValidationConstants
    {
        public const string ExpireDateFormat = "dd/MM/yyyy HH:mm";
        public const string PasswordProperty = "Password";
        public const string DefaultBackground = "~/Images/DefaultBackground.b";
        public const string CustomBackgroundFileName = "~/Images/{0}background.b";
        public const int TitleMinLenght = 3;
        public const int TitleMaxLenght = 30;
        public const int ContentMinLenght = 1;
        public const int ContentMaxLenght = 100;
        public const int DefaultPageSize = 10;
        public const int UsernameMinLenght = 1;
        public const int UsernameMaxLenght = 30;
        public const int PasswordMinLenght = 6;
        public const int PasswordMaxLenght = 40;
        public const int FullNameMinLenght = 6;
        public const int FullNameMaxLenght = 30;
        public const int MinutesToAdd = 29;
        public const int MinAge = 4;
        public const int MaxAge = 117;
        public const string EmailRegexPattern = @"(([^<>()\[\]\\.,;:\s@""]+(\.[^<>()\[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$";
    }
}
