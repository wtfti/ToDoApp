namespace ToDo.Server.Common.Constants
{
    public static class MessageConstants
    {
        public const string CreateNote = "Note created!";
        public const string NoteDoesNotExist = "Requested note does not exists!";
        public const string RemoveNote = "Note removed!";
        public const string InvalidDate = "Invalid date! Minimum possible date is today and shoul be at 30 minutes from now!";
        public const string CreateUser = "Successfully created user!";
        public const string Logout = "Logout successful!";
        public const string EmailAlreadyTaken = "Email is already taken!";
        public const string EmptyRequest = "Request cannot be empty";
        public const string InvalidEmail = "Invalid Email!";
        public const string ChangedPasswordSuccessful = "Password has been changed successfully";
        public const string CompletedNoteSucessful = "Completed!";
        public const string ChangedNoteParamsSuccessful = "Note has been changed successfully";
        public const string InvalidJsonFormat = "Wrong JSON format";
        public const string UnknownAttribute = "Unknown attribute";
        public const string InvalidPage = "Invalid Page";
        public const string ProfileChangedSuccessful = "Profile settings are changed successful";
        public const string NewPasswordIsNotSameAsConfirmPassword = "New password is not same as confirm password";
        public const string CurrentUserCannotRemoveThisNote = "Removing denied! Only creator can remove this note!";
        public const string CurrentUserCannotEditThisNote = "Operation denied! Only creator can edit this note!";
        public const string CurrentUserCannotCompleteThisNote = "Operation denied! Only creator can complete this note!";
        public const string NoteIsExpired = "Note is expired!";
        public const string NoteIsCompleted = "Note is completed!";
    }
}
