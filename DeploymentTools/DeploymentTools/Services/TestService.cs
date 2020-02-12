using System;
using System.Collections.Generic;
using System.Text;

namespace DeploymentTools
{
    public class TestService : DeploymentService
    {
        public TestService() : base("TestApp")
        {
        }

        public void Test()
        {
            try
            {
                Powershell.RunScript(@"PowershellScripts\Test.ps1", "DirName1", "Dir1", "DirName2", "Dir2");
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }
    }
}
