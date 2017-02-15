﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace MoMA.Analyzer.MoMAWebService {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="MoMASubmitSoap", Namespace="http://mono-project.com/MoMASubmit/")]
    public partial class MoMASubmit : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback SubmitResultsOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetLatestDefinitionsVersionOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public MoMASubmit() {
            this.Url = "http://www.go-mono.com/moma/MoMASubmit.asmx";
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event SubmitResultsCompletedEventHandler SubmitResultsCompleted;
        
        /// <remarks/>
        public event GetLatestDefinitionsVersionCompletedEventHandler GetLatestDefinitionsVersionCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://mono-project.com/MoMASubmit/SubmitResults", RequestNamespace="http://mono-project.com/MoMASubmit/", ResponseNamespace="http://mono-project.com/MoMASubmit/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool SubmitResults(string results) {
            object[] results1 = this.Invoke("SubmitResults", new object[] {
                        results});
            return ((bool)(results1[0]));
        }
        
        /// <remarks/>
        public void SubmitResultsAsync(string results) {
            this.SubmitResultsAsync(results, null);
        }
        
        /// <remarks/>
        public void SubmitResultsAsync(string results, object userState) {
            if ((this.SubmitResultsOperationCompleted == null)) {
                this.SubmitResultsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSubmitResultsOperationCompleted);
            }
            this.InvokeAsync("SubmitResults", new object[] {
                        results}, this.SubmitResultsOperationCompleted, userState);
        }
        
        private void OnSubmitResultsOperationCompleted(object arg) {
            if ((this.SubmitResultsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SubmitResultsCompleted(this, new SubmitResultsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://mono-project.com/MoMASubmit/GetLatestDefinitionsVersion", RequestNamespace="http://mono-project.com/MoMASubmit/", ResponseNamespace="http://mono-project.com/MoMASubmit/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetLatestDefinitionsVersion() {
            object[] results = this.Invoke("GetLatestDefinitionsVersion", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetLatestDefinitionsVersionAsync() {
            this.GetLatestDefinitionsVersionAsync(null);
        }
        
        /// <remarks/>
        public void GetLatestDefinitionsVersionAsync(object userState) {
            if ((this.GetLatestDefinitionsVersionOperationCompleted == null)) {
                this.GetLatestDefinitionsVersionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetLatestDefinitionsVersionOperationCompleted);
            }
            this.InvokeAsync("GetLatestDefinitionsVersion", new object[0], this.GetLatestDefinitionsVersionOperationCompleted, userState);
        }
        
        private void OnGetLatestDefinitionsVersionOperationCompleted(object arg) {
            if ((this.GetLatestDefinitionsVersionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetLatestDefinitionsVersionCompleted(this, new GetLatestDefinitionsVersionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    public delegate void SubmitResultsCompletedEventHandler(object sender, SubmitResultsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SubmitResultsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SubmitResultsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    public delegate void GetLatestDefinitionsVersionCompletedEventHandler(object sender, GetLatestDefinitionsVersionCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetLatestDefinitionsVersionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetLatestDefinitionsVersionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591