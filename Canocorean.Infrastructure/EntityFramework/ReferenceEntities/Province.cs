using JetBrains.Annotations;

namespace Canocorean.Infrastructure.EntityFramework.ReferenceEntities
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class Province
    {
        private Province()
        {

        }
        public string ISOCode { get; private set; }
        public string Name { get; private set; }
        public string CountryISOCode { get; private set; }
        public Country Country { get; private set; }
    }
}