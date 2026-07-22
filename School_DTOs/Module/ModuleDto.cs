namespace School_DTOs.Module
{
    public class ModuleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Route { get; set; } = null!;
        public string? Icon { get; set; }
        public int CategoryModuleId { get; set; }
        public string? CategoryModuleName { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class ModulePermissionDto
    {
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = null!;
        public string? ModuleIcon { get; set; }
        public string UserId { get; set; } = null!;
        public string? UserName { get; set; }
        public string? RoleId { get; set; }
        public string? RoleName { get; set; }
        public bool IsActive { get; set; }
    }

    public class UserModulesDto
    {
        public string UserId { get; set; } = null!;
        public string? UserName { get; set; }
        public List<ModuleDto> Modules { get; set; } = new List<ModuleDto>();
    }
}

