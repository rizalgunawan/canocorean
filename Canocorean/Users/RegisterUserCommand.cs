namespace Canocorean.Users
{
    public class RegisterUserCommand
    {
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public UserLocation Location { get; set; }
        public class UserLocation
        {
            public string CountryISOCode { get; set; }
            public string ProvinceISOCode { get; set; }
        }
    }
}