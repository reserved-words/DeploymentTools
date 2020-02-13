using System.Diagnostics;

namespace DeploymentTools
{
    internal static class Powershell
    {
        public static void RunScript(string scriptPath, params string[] parameters)
        {
            var paramString = string.Join(" ", parameters);
            var commandString = string.Format("-File \"{0}\" {1}", scriptPath, paramString);

            Process.Start("powershell.exe", commandString);
        }
    }
}
