namespace School.Utilities.Constants
{
    public static class Constants
    {
        public static List<string> LoginSupportedApps = ["WEB", "APP"];
        public static readonly string SuperAdmin = Guid.NewGuid().ToString();
        public static readonly string Admin = Guid.NewGuid().ToString();
        public static readonly string Student = Guid.NewGuid().ToString();
        public static readonly string Owner = Guid.NewGuid().ToString();
        public static readonly string Employee = Guid.NewGuid().ToString();

        public static readonly string SuperAdminUser = Guid.NewGuid().ToString();
        public static readonly string AdminUser = Guid.NewGuid().ToString();
        public static readonly string StudentUser = Guid.NewGuid().ToString();
        public static readonly string OwnerUser = Guid.NewGuid().ToString();
        public static readonly string EmployeeUser = Guid.NewGuid().ToString();
    }

}
