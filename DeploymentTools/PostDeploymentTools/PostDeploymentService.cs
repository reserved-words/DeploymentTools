using System;

namespace PostDeploymentTools
{
    public class PostDeploymentService
    {
        private readonly string _appName;
        private readonly string _connectionString;
        private readonly string _databaseName;
        private readonly string _schemaName;
        private readonly Action<Exception> _handleError;

        public PostDeploymentService(string appName, string connectionString, string databaseName, string schemaName, Action<Exception> handleError = null)
        {
            _appName = appName;
            _connectionString = connectionString;
            _databaseName = databaseName;
            _schemaName = schemaName;
            _handleError = handleError;
        }

        public void UpdateDatabase(Func<MigratableDbContext> dbContextFactory)
        {
            try
            {
                Database.Migrate(dbContextFactory);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        public void CreateTaskUser()
        {
            CreateUser(TaskUserName);
        }

        public void CreateWebAppUser()
        {
            CreateUser(AppUserName);
        }

        public void CreateApiUser()
        {
            CreateUser(ApiUserName);
        }

        public void GrantTaskPermission(string permission, string objectName = null)
        {
            GrantPermission(TaskUserName, permission, objectName);
        }

        public void GrantWebAppPermission(string permission, string objectName = null)
        {
            GrantPermission(AppUserName, permission, objectName);
        }

        public void GrantApiPermission(string permission, string objectName = null)
        {
            GrantPermission(ApiUserName, permission, objectName);
        }

        private void CreateUser(string userName)
        {
            try
            {
                Database.CreateUser(_connectionString, _databaseName, _schemaName, userName);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void GrantPermission(string userName, string permission, string objectName = null)
        {
            try
            {
                if (objectName == null)
                {
                    Database.GrantSchemaPermission(_connectionString, permission, _schemaName, userName);
                }
                else
                {
                    Database.GrantObjectPermission(_connectionString, permission, _schemaName, objectName, userName);
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private string ApiName => $"{_appName}Api";
        private string AppUserName => $@"IIS APPPOOL\{_appName}";
        private string ApiUserName => $@"IIS APPPOOL\{ApiName}";
        private string TaskUserName => $"{_appName}TaskUser";

        private void HandleError(Exception ex)
        {
            if (_handleError == null)
                return;

            _handleError(ex);
        }
    }
}
