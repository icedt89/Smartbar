﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JanHafner.Smartbar.Common.UserInterface.Localization {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class MenuItems {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal MenuItems() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("JanHafner.Smartbar.Common.UserInterface.Localization.MenuItems", typeof(MenuItems).Assembly);
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
        ///   Looks up a localized string similar to Create.
        /// </summary>
        public static string ApplicationButtonCreate {
            get {
                return ResourceManager.GetString("ApplicationButtonCreate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Image files (.bmp, .gif, .jpg, .png, .tiff).
        /// </summary>
        public static string FrameworkSupportedImageFilesFilterText {
            get {
                return ResourceManager.GetString("FrameworkSupportedImageFilesFilterText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to From icon....
        /// </summary>
        public static string ProcessApplicationButtonChangeDisplayImageFromIcon {
            get {
                return ResourceManager.GetString("ProcessApplicationButtonChangeDisplayImageFromIcon", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to From built-in icon library....
        /// </summary>
        public static string ProcessApplicationButtonChangeDisplayImageFromIconPackResource {
            get {
                return ResourceManager.GetString("ProcessApplicationButtonChangeDisplayImageFromIconPackResource", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to From file....
        /// </summary>
        public static string ProcessApplicationButtonChangeDisplayImageFromImageFile {
            get {
                return ResourceManager.GetString("ProcessApplicationButtonChangeDisplayImageFromImageFile", resourceCulture);
            }
        }
    }
}
