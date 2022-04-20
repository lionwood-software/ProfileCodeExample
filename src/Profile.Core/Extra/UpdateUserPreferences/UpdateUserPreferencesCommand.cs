using System.Collections.Generic;
using MediatR;

namespace Profile.Core.Extra.UpdateUserPreferences
{
    public class UpdateUserPreferencesCommand : IRequest
    {
        public IEnumerable<UpdateUserPreferenceRecord> Preferences { get; set; }
    }
}
