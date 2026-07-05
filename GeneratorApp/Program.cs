using System.IO;

class Program
{
    static void Main()
    {
        string path = @"E:\GIT\SchoolSAAS\SchoolWeb_Api\School_API\Extensions\ServiceCollection.cs";
        string content = File.ReadAllText(path);
        
        content = content.Replace(".AddScoped<IDepartmentService, DepartmentService>()", ".AddScoped<IDepartmentService, School.Services.DepartmentService>()");
        
        File.WriteAllText(path, content);
    }
}
