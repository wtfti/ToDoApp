namespace ToDo.Server.Common.Constants
{
    public static class MessageConstants
    {
        public const string CreateNoteMessage = "Note created!";
        public const string NoteDoesNotExistsMessage = "Requested note does not exists!";
        public const string RemoveNoteMessage = "Note removed!";
        public const string TitleChangeMessage = "Title changed!";
        public const string ContextChangeMessage = "Context changed!";
        public const string InvalidDateMessage = "Invalid date!";
        public const string SetDateMessage = "Successfully set expire date!";
        public const string ChangeDateMessage = "Expire date changed!";
        public const string CreateUserMessage = "Successfully created user!";
        public const string LogoutMessage = "Logout successful!";
        public const string UserIsNotAddInDbMessage = "Error while creating user!";
        public const string EmptyRequest = "Request cannot be empty";
    }
}
