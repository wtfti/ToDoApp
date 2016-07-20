namespace ToDo.Services.Data.Contracts
{
    using ToDo.Data.Models;

    public interface IAccountService
    {
        ProfileDetails ProfileDetails(string userId);

        void Edit(string userId, string fullName, int? age, GenderType gender);
    }
}
