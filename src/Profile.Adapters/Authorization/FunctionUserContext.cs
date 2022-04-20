using System;
using System.Globalization;
using Profile.Core.SharedKernel;

namespace Profile.Adapters.Authorization
{
    public class FunctionUserContext : IProfileUserContext
    {
        public Guid UserId => Guid.Parse("11111111-1111-1111-1111-111111111111");

        public string Culture => "en";

        public string Email => "profileazurefunction@mail.com";

        public bool HasUserId() => true;

        public void SetCulture(string culture)
        {
            CultureInfo.CurrentUICulture = new CultureInfo(culture);
        }
    }
}