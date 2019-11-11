using JetBrains.Annotations;

namespace Canocorean.Users
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class User
    {
        private User()
        {

        }

        public User(RegisterUserCommand command)
        {
            Login = command.Login;
            PasswordHash = command.PasswordHash;
            Location = new UserLocation(command.Location);
        }

        public string Login { get; private set; }
        public string PasswordHash { get; private set; }
        public UserLocation Location { get; private set; }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        public class UserLocation
        {
            private UserLocation()
            {
                
            }
            public UserLocation(RegisterUserCommand.UserLocation location)
            {
                CountryISOCode = location.CountryISOCode;
                ProvinceISOCode = location.ProvinceISOCode;
            }

            public string CountryISOCode { get; private set; }
            public string ProvinceISOCode { get; private set; }
        }
    }
}
