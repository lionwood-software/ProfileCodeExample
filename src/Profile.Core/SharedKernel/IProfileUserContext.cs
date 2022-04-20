
namespace Profile.Core.SharedKernel
{
    public interface IProfileUserContext : IUserContext
    {
        public string Culture { get; }
        public string Email { get; }
        bool HasUserId();

        public void SetCulture(string culture);
    }
}