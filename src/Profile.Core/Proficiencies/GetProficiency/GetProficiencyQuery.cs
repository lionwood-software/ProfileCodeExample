using System.Collections.Generic;
using MediatR;
using Profile.Core.SharedKernel;

namespace Profile.Core.Proficiencies.GetProficiency
{
    public class GetProficienciesQuery : IRequest<List<ListItem>>
    {
    }
}
