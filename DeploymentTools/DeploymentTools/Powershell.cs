using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;

namespace DeploymentTools
{
    public static class Powershell
    {
        public static void RunScript(string script, params string[] parameters)
        {
            var scriptPath = "";

            var runspaceConfiguration = RunspaceConfiguration.Create();

            using (var runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration))
            {
                runspace.Open();

                var scriptInvoker = new RunspaceInvoke(runspace);

                var pipeline = runspace.CreatePipeline();

                var command = new Command(scriptPath);

                for (var i = 0; i < parameters.Count(); i = i + 2)
                {
                    var key = parameters[i];
                    var value = parameters[i + 1];
                    command.Parameters.Add(new CommandParameter(key, value));
                }

                pipeline.Commands.Add(command);

                pipeline.Invoke();

                runspace.Close();
            }
        }
    }
}
