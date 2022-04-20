using System.Collections.Generic;
using MediatR;

namespace Profile.Core.Extra.GetUserPreferences
{
    public class GetUserPreferencesQuery : IRequest<IEnumerable<UserPreference>>
    {
    }
}