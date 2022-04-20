using System;
using Profile.Core.Languages;
using Profile.Core.Proficiencies;
using Profile.Core.SharedKernel;

namespace Profile.Core.Users
{
    public class UserLanguageProficiency : IAuditable
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string LanguageCode { get; set; }
        public int ProficiencyId { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public User User { get; set; }
        public Language Language { get; set; }
        public Proficiency Proficiency { get; set; }
    }
}
