using JetBrains.Annotations;

namespace Canocorean.Infrastructure.EntityFramework.Entities
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class Country
    {
        private Country()
        {

        }
        public Country(string isoCode)
        {
            ISOCode = isoCode;
        }
        public string ISOCode { get; private set; }
    }
}
