using System.IO;

class Program
{
    static void Main()
    {
        string path = @"E:\GIT\SchoolSAAS\SchoolWeb_Api\School_API\Extensions\ServiceCollection.cs";
        string content = File.ReadAllText(path);
        content = content.Replace("using School.Infrastructure.Repositories.IRepositories.Hr.LeaveManagement;", "");
        content = content.Replace("using School.Infrastructure.Repositories.IRepositories.Hr.Attendance;", "");
        content = content.Replace("using School.Infrastructure.Repositories.IRepositories.Hr.Timesheet;", "");
        File.WriteAllText(path, content);
        
        path = @"E:\GIT\SchoolSAAS\SchoolWeb_Api\School_API\Controllers\Hr\DesignationController.cs";
        content = File.ReadAllText(path);
        content = content.Replace("global::School.Domain.Designation", "global::School.Domain.Hr.Designation");
        File.WriteAllText(path, content);
    }
}
