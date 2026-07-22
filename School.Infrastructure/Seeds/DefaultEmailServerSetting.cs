using School.Domain.Email;
namespace School.Infrastructure.Seeds
{
    public static class DefaultEmailServerSetting
    {
        public static List<EmailServerSetting> GetAllEmailServerSetting()
        {

            return new List<EmailServerSetting>()
            {
                new EmailServerSetting
                {

                    CreatedDate = DateTime.Now,
                    DisplayName= "SchoolSaaas Support",
                    EnableSSL= true,
                    FromEmail="titwig365@gmail.com",
                    HostName= "smtp.gmail.com",
                    IsActive= true,
                    Password= "koglixeavvrnmsle",
                    Port=587,
                    UpdatedDate= DateTime.Now,
                    UseDefaultCredential= false,
                    UserName= "titwig365@gmail.com",
                },

            };
        }
    }
}
