﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CareerProUtil {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.4.0.0")]
    internal sealed partial class CareerProUtil : global::System.Configuration.ApplicationSettingsBase {
        
        private static CareerProUtil defaultInstance = ((CareerProUtil)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new CareerProUtil())));
        
        public static CareerProUtil Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("c:\\data")]
        public string LoadLocation {
            get {
                return ((string)(this["LoadLocation"]));
            }
            set {
                this["LoadLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("postgres")]
        public string dbUser {
            get {
                return ((string)(this["dbUser"]));
            }
            set {
                this["dbUser"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ballbearing")]
        public string dbPass {
            get {
                return ((string)(this["dbPass"]));
            }
            set {
                this["dbPass"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("careerpro1-instance-1.cgmk9ruhyhnz.us-east-1.rds.amazonaws.com")]
        public string hostname {
            get {
                return ((string)(this["hostname"]));
            }
            set {
                this["hostname"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5432")]
        public string port {
            get {
                return ((string)(this["port"]));
            }
            set {
                this["port"] = value;
            }
        }
    }
}