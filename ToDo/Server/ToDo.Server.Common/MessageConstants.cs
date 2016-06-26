namespace ToDo.Server.Common
{
    public static class MessageConstants
    {
        public const string CreateNoteMessage = "Note created!";
        public const string NoteDoesNotExistsMessage = "Requested note does not exists!";
        public const string RemoveNoteMessage = "Note removed!";
        public const string TitleChangeMessage = "Title changed!";
        public const string ContextChangeMessage = "Context changed!";
        public const string InvalidDateMessage = "Invalid date!";
        public const string SetDateMessage = "Sucessfully set expire date!";
        public const string ChangeDateMessage = "Expire date changed!";
        public const string CreateUserMessage = "Successfuly created user!";
        public const string LogoutMessage = "Logout successful!";
        public const string UserIsNotAddInDbMessage = "Error while creating user!";
    }
}
