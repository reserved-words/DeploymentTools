using System;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace DeploymentTools
{
    public class PreDeploymentService : DeploymentService
    {
        private readonly string _domainName;

        public PreDeploymentService(string appName, string domainName)
            :base(appName)
        {
            _domainName = domainName;
        }

        public void CreateService(string password)
        {
            RunPowershell("ServiceSetUp", "-DomainName", _domainName, "-AppName", AppName, "-UserName", TaskUserName, "-Password", password);
        }

        public void CreateApi()
        {
            RunPowershell("WebAppSetUp", "-DomainName", _domainName, "-AppName", ApiName);
        }

        public void CreateWebApp()
        {
            RunPowershell("WebAppSetUp", "-DomainName", _domainName, "-AppName", AppName);
        }

        private void RunPowershell(string script, params string[] parameters)
        {
            try
            {
                Powershell.RunScript(script, parameters);
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }
    }
}
