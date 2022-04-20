using System;
using System.Globalization;
using Profile.Core.SharedKernel;

namespace Profile.Tests.Base
{
    public class UserTestContext : IProfileUserContext
    {
        public Guid UserId => Guid.Parse("f629b8e5-6286-45cd-b203-d2340c61a91d");
        public string Email => "test@test.com";
        public bool HasUserId() => true;
        public void SetCulture(string culture)
        {
            CultureInfo.CurrentUICulture = new CultureInfo(culture);
        }
        public string Culture => "en";
    }
}