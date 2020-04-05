using System;

namespace PreDeploymentTools
{
    public class PreDeploymentService
    {
        private readonly string _appName;
        private readonly string _domainName;
        private readonly Action<Exception> _handleError;

        public PreDeploymentService(string appName, string domainName, Action<Exception> handleError = null)
        {
            _handleError = handleError;
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
                if (!HandleError(ex))
                {
                    throw;
                }
            }
        }

        private string ApiName => $"{_appName}Api";
        private string TaskUserName => $"{_appName}TaskUser";

        private bool HandleError(Exception ex)
        {
            if (_handleError == null)
                return false;

            try
            {
                _handleError(ex);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
