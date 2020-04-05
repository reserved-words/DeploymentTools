using System;
using System.IO;

namespace PreDeploymentTools
{
    public class PreDeploymentService
    {
        private readonly string _appName;
        private readonly string _domainName;
        private readonly string _logFile;
        private readonly Action<Exception> _handleError;

        public PreDeploymentService(string appName, string domainName, string logFile, Action<Exception> handleError = null)
        {
            _logFile = logFile;
            _handleError = handleError;
            _appName = appName;
            _domainName = domainName;
        }

        public void CreateService(string password)
        {
            Log("Creating service");
            RunPowershell("ServiceSetUp", _domainName, _appName, TaskUserName, password, _logFile);
            Log("Service created");
        }

        public void CreateApi()
        {
            Log("Creating API");
            RunPowershell("WebAppSetUp", _domainName, ApiName);
            Log("API created");
        }

        public void CreateWebApp()
        {
            Log("Creating web app");
            RunPowershell("WebAppSetUp", _domainName, _appName);
            Log("Web app created");
        }

        private void RunPowershell(string script, params string[] parameters)
        {
            try
            {
                Log($"Running script {script}");
                Powershell.RunScript(script, parameters);
                Log($"Finished running script {script}");
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

        private void Log(string message)
        {
            File.AppendText($"{DateTime.Now.ToShortTimeString()} {message}");
        }
    }
}
