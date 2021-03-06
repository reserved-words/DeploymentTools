﻿using System;
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
            var powershellLogging = _logFile.Replace(".log", "-ps.log");

            Log("Creating service");
            RunPowershell("ServiceSetUp", _domainName, _appName, TaskUserName, password, powershellLogging);
            Log("Service created");
        }

        public void CreateApi()
        {
            Log("Creating API");
            RunPowershell("IIS-AppSetUp", _domainName, ApiName);
            Log("API created");
        }

        public void CreateWebApp()
        {
            Log("Creating web app");
            RunPowershell("IIS-AppSetUp", _domainName, _appName);
            Log("Web app created");
        }

        private void RunPowershell(string script, params string[] parameters)
        {
            try
            {
                Log($"Running script {script}");
                Powershell.RunScript(_logFile, script, parameters);
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
        private string TaskUserName => $"{_appName}User";

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
            File.AppendAllText(_logFile, $"{DateTime.Now.ToShortTimeString()} {message}{Environment.NewLine}");
        }
    }
}
