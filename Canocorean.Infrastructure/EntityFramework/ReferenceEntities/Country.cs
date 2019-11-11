using System.Collections.Generic;
using JetBrains.Annotations;

namespace Canocorean.Infrastructure.EntityFramework.ReferenceEntities
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class Country
    {
        private Country()
        {

        }
        public string ISOCode { get; private set; }
        public string Name { get; private set; }
        public ICollection<Province> Provinces { get; private set; }
    }
}
