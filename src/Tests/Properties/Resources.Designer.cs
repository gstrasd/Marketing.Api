﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tests.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Tests.Properties.Resources", typeof(Resources).Assembly);
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
        ///&lt;Root&gt;
        ///	&lt;Patient FirstName=&quot;Mickey&quot; LastName=&quot;Mouse&quot; DOB=&quot;01/01/1894&quot; Gender=&quot;U&quot; AreaCode=&quot;630&quot; Prefix=&quot;770&quot; Suffix=&quot;1000&quot; Email=&quot;micky@disney.com&quot; PrimaryInsurance=&quot;Medicaid&quot; PrimaryPolicy=&quot;&quot; SecondaryInsurance=&quot;Yes&quot; SecondaryPolicy=&quot;&quot; /&gt;
        ///	&lt;Order Subject=&quot;New Order Notes&quot; SalesRep=&quot;&quot; ReferralId=&quot;&quot; DischargeId=&quot;&quot; Discharge=&quot;&quot; LeadSource=&quot;Halda&quot; SessionId=&quot;582895&quot; IntakeOrderId=&quot;&quot; DocType=&quot;Initial Documentation&quot; /&gt;
        ///	&lt;Items&gt;
        ///		&lt;Item ProductId=&quot;7085&quot; DomainId=&quot;50&quot; N [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string create_lead_data {
            get {
                return ResourceManager.GetString("create-lead-data", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] sample_scooter_submission {
            get {
                object obj = ResourceManager.GetObject("sample-scooter-submission", resourceCulture);
                return ((byte[])(obj));
            }
        }
    }
}
