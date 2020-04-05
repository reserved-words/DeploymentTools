using System;
using System.Diagnostics;
using System.IO;

namespace PreDeploymentTools
{
    internal static class Powershell
    {
        public static void RunScript(string logFile, string scriptPath, params string[] parameters)
        {
            Log(logFile, "Starting run script method");
            var paramString = string.Join(" ", parameters);
            var commandString = string.Format("-File \"{0}\" {1}", scriptPath, paramString);

            Log(logFile, $"Command string: {commandString}");
            Process.Start("powershell.exe", commandString);
        }

        private static void Log(string logFile, string message)
        {
            File.AppendAllText(logFile, $"{DateTime.Now.ToShortTimeString()} {message}{Environment.NewLine}");
        }
    }
}
