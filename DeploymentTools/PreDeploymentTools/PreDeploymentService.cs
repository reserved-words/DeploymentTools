using DeploymentTools;
using System;

namespace PreDeploymentTools
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
            RunPowershell("ServiceSetUp", _domainName, AppName, TaskUserName, password);
        }

        public void CreateApi()
        {
            RunPowershell("WebAppSetUp", _domainName, ApiName);
        }

        public void CreateWebApp()
        {
            RunPowershell("WebAppSetUp", _domainName, AppName);
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
