namespace ExampleLoginJWT.Common.Constants
{
    public class SystemConstants
    {
        public class Claims
        {
            public const string Permissions = "permissions";
        }

        public class UserClaim
        {
            public const string Roles = "Roles";
            public const string Id = "Id";
            public const string Permissions = "permissions";
            public const string FullName = "fullName";
        }

        public class Roles
        {
            public const string Admin = "Admin";
        }
    }
}