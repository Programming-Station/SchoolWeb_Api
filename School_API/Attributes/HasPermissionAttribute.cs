namespace School_API.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class HasPermissionAttribute : Attribute
    {
        public string ModuleName { get; }

        public HasPermissionAttribute(string moduleName)
        {
            ModuleName = moduleName;
        }
    }
}
