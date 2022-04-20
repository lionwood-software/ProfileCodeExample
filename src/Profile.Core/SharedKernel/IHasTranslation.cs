using System.Collections.Generic;

namespace Profile.Core.SharedKernel
{
    public interface IHasTranslation<TTranslation>
    {
        public ICollection<TTranslation> Translations { get; set; }
    }
}