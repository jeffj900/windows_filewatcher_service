namespace FileWatcherService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.fileWatcherServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.fileWatcherServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // fileWatcherServiceProcessInstaller
            // 
            this.fileWatcherServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.fileWatcherServiceProcessInstaller.Password = null;
            this.fileWatcherServiceProcessInstaller.Username = null;
            // 
            // fileWatcherServiceInstaller
            // 
            this.fileWatcherServiceInstaller.DisplayName = "FileWatcherService";
            this.fileWatcherServiceInstaller.ServiceName = "FileWatcherService";
            this.fileWatcherServiceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.fileWatcherServiceInstaller_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.fileWatcherServiceProcessInstaller,
            this.fileWatcherServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller fileWatcherServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller fileWatcherServiceInstaller;
    }
}