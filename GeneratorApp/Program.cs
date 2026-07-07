using System.IO;

class Program
{
    static void Main()
    {
        string path = @"C:\Users\HP\.gemini\antigravity-ide\brain\79c32308-eaa6-4ef8-ba90-419ac9f2de71\task.md";
        string content = File.ReadAllText(path);
        
        content = content.Replace("- [/] **Attendance Logic:**", "- [x] **Attendance Logic:**");
        content = content.Replace("  - [ ] Implement PunchInAsync and PunchOutAsync in AttendanceService.cs.", "  - [x] Implement PunchInAsync and PunchOutAsync in AttendanceService.cs.");
        content = content.Replace("  - [ ] Expose these endpoints in AttendanceController.cs.", "  - [x] Expose these endpoints in AttendanceController.cs.");
        content = content.Replace("- [ ] **Timesheet Logic:**", "- [/] **Timesheet Logic:**");
        
        File.WriteAllText(path, content);
    }
}
