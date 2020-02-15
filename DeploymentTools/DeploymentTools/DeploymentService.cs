using System;
using System.IO;
using System.Text;

namespace DeploymentTools
{
    public class DeploymentService
    {
        protected readonly string AppName;

        public DeploymentService(string appName)
        {
            AppName = appName;
        }

        protected string ApiName => $"{AppName}Api";
        protected string AppUserName => $@"IIS APPPOOL\{AppName}";
        protected string ApiUserName => $@"IIS APPPOOL\{ApiName}";
        protected string TaskUserName => $"{AppName}TaskUser";

        protected void Log(Exception ex)
        {
            var logFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "ReservedWords",
                AppName);

            Directory.CreateDirectory(logFolder);

            var str = new StringBuilder();

            str.Append(DateTime.Now.ToString(@"dd/MM/yy HH:mm:ss"));
            str.Append(Environment.NewLine);

            while (ex != null)
            {
                str.Append(ex.Message);
                str.Append(Environment.NewLine);

                str.Append(ex.StackTrace);
                str.Append(Environment.NewLine);

                if (ex.Data != null)
                {
                    foreach (var key in ex.Data.Keys)
                    {
                        str.Append($"{key}: {ex.Data[key]}");
                        str.Append(Environment.NewLine);
                    }
                }

                ex = ex.InnerException;
            }

            str.Append(Environment.NewLine);
            str.Append(Environment.NewLine);

            var logFile = Path.Combine(logFolder, "errors.log");

            File.AppendAllText(logFile, str.ToString());
        }
    }
}
