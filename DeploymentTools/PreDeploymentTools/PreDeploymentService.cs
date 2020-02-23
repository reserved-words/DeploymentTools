using System;

namespace PreDeploymentTools
{
    public class PreDeploymentService
    {
        private readonly string _appName;
        private readonly string _domainName;

        public PreDeploymentService(string appName, string domainName)
        {
            _appName = appName;
            _domainName = domainName;
        }

        public void CreateService(string password)
        {
            RunPowershell("ServiceSetUp", _domainName, _appName, TaskUserName, password);
        }

        public void CreateApi()
        {
            RunPowershell("WebAppSetUp", _domainName, ApiName);
        }

        public void CreateWebApp()
        {
            RunPowershell("WebAppSetUp", _domainName, _appName);
        }

        private void RunPowershell(string script, params string[] parameters)
        {
            try
            {
                Powershell.RunScript(script, parameters);
            }
            catch (Exception ex)
            {
                // TO DO
                // Log(ex);
            }
        }

        private string ApiName => $"{_appName}Api";
        private string TaskUserName => $"{_appName}TaskUser";
    }
}
