using System;
using System.ComponentModel;
using System.Configuration.Install;

namespace NBF.Qubica.Pollservice
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            string[] args = Environment.GetCommandLineArgs();
            foreach (string arg in args)
            {
                if (arg.StartsWith("/ServiceName", StringComparison.InvariantCultureIgnoreCase))
                    this.serviceInstaller.ServiceName = arg.Split('=')[1];
                if (arg.StartsWith("/DisplayName", StringComparison.InvariantCultureIgnoreCase))
                    this.serviceInstaller.DisplayName = arg.Split('=')[1];
            };
        }

    }
}
