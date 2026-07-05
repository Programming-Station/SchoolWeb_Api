
namespace School.Utilities.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class CommonResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CommonResource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("School.Utilities.Resources.CommonResource", typeof(CommonResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to add data..
        /// </summary>
        public static string AddFailed {
            get {
                return ResourceManager.GetString("AddFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Data added successfully..
        /// </summary>
        public static string AddSuccess {
            get {
                return ResourceManager.GetString("AddSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} {1} already exists in the selected state..
        /// </summary>
        public static string AlreadyExists {
            get {
                return ResourceManager.GetString("AlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A {0} with {1} &apos;{2}&apos; already exists..
        /// </summary>
        public static string AlreadyExistsRecord {
            get {
                return ResourceManager.GetString("AlreadyExistsRecord", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unfortunately, the {0} assignment has failed. Please try again later.
        /// </summary>
        public static string AssignFailed {
            get {
                return ResourceManager.GetString("AssignFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} has been assigned successfully!.
        /// </summary>
        public static string AssignSuccess {
            get {
                return ResourceManager.GetString("AssignSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} is available.
        /// </summary>
        public static string Available {
            get {
                return ResourceManager.GetString("Available", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to delete data..
        /// </summary>
        public static string DeleteFailed {
            get {
                return ResourceManager.GetString("DeleteFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Data deleted successfully..
        /// </summary>
        public static string DeleteSuccess {
            get {
                return ResourceManager.GetString("DeleteSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to fetch records..
        /// </summary>
        public static string FetchFailed {
            get {
                return ResourceManager.GetString("FetchFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Successfully fetch records..
        /// </summary>
        public static string FetchSuccess {
            get {
                return ResourceManager.GetString("FetchSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Internal Server Error! Please try after some time or contact admin..
        /// </summary>
        public static string InternalServerError {
            get {
                return ResourceManager.GetString("InternalServerError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Something went wrong please try again after some time..
        /// </summary>
        public static string PleaseTryAgain {
            get {
                return ResourceManager.GetString("PleaseTryAgain", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No record found.
        /// </summary>
        public static string RecordNotFound {
            get {
                return ResourceManager.GetString("RecordNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Service not found.
        /// </summary>
        public static string ServiceNotFound {
            get {
                return ResourceManager.GetString("ServiceNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to update data..
        /// </summary>
        public static string UpdateFailed {
            get {
                return ResourceManager.GetString("UpdateFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Data updated successfully..
        /// </summary>
        public static string UpdateSuccess {
            get {
                return ResourceManager.GetString("UpdateSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} already exists in this account.
        /// </summary>
        public static string UserNameAlreadyExists {
            get {
                return ResourceManager.GetString("UserNameAlreadyExists", resourceCulture);
            }
        }
    }
}
