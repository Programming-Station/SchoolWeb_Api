using School.Domain.School;

namespace School.Infrastructure.Seeds
{
    public static class DefaultMasterData
    {
        public static List<SchoolMedium> SchoolMediumList()
        {
            return new List<SchoolMedium>
            {
                new SchoolMedium { Name = "English" },
                new SchoolMedium { Name = "Hindi" },
                new SchoolMedium { Name = "Gujarati" },
                new SchoolMedium { Name = "Marathi" },
                new SchoolMedium { Name = "Urdu" }
            };
        }

        public static List<SchoolType> SchoolTypeList()
        {
            return new List<SchoolType>
            {
                new SchoolType { Name = "Co-Education" },
                new SchoolType { Name = "Boys Only" },
                new SchoolType { Name = "Girls Only" }
            };
        }

        public static List<AffiliationBoard> AffiliationBoardList()
        {
            return new List<AffiliationBoard>
            {
                new AffiliationBoard { Name = "CBSE" },
                new AffiliationBoard { Name = "ICSE" },
                new AffiliationBoard { Name = "State Board" },
                new AffiliationBoard { Name = "IB" }
            };
        }
    }
}
