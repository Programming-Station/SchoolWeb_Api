using School.Domain;
using School.Utilities.Enums;

namespace School.Infrastructure.Seeds
{
    public static class DefaultStatusList
    {
        public static List<Status> StatusList()
        {
            return new List<Status>()
            {
                new Status
                {
                    Name = DefaultStatus.Created.ToString()
                },
                new Status
                {
                    Name = DefaultStatus.Pending.ToString()
                },
                new Status
                {
                    Name = DefaultStatus.Verified.ToString()
                },
                new Status
                {
                    Name = DefaultStatus.Active.ToString()
                },
                new Status
                {
                    Name = DefaultStatus.Inactive.ToString()
                },
                new Status
                {
                    Name = DefaultStatus.Blocked.ToString()
                },
                new Status
                {
                    Name = DefaultStatus.Success.ToString()
                },
                new Status
                {
                    Name = DefaultStatus.Approved.ToString()
                },
                new Status
                {
                    Name = DefaultStatus.Rejected.ToString()
                },
                new Status
                {
                    Name = DefaultStatus.Assigned.ToString()
                },
                new Status
                {
                    Name = DefaultStatus.UnAssigned.ToString()
                },
                new Status
                {
                    Name = DefaultStatus.Incomplete.ToString()
                },
                new Status
                {
                    Name = DefaultStatus.Completed.ToString()
                },
                new Status
                {
                    Name = DefaultStatus.New.ToString()
                },
                new Status
                {
                    Name = DefaultStatus.Read.ToString()
                },
                new Status
                {
                    Name = DefaultStatus.Replied.ToString()
                },
                new Status
                {
                    Name = DefaultStatus.Closed.ToString()
                },
            };
        }
    }
}
