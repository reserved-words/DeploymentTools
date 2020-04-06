using System;

namespace PostDeploymentTools
{
    public class PostDeploymentService
    {
        private readonly string _appName;
        private readonly string _connectionString;
        private readonly string _databaseName;
        private readonly string _schemaName;
        private readonly string _domainName;

        private readonly Action<Exception> _handleError;

        public PostDeploymentService(
            string domainName,
            string appName, 
            string connectionString, 
            string databaseName, 
            string schemaName, 
            Action<Exception> handleError = null)
        {
            _appName = appName;
            _connectionString = connectionString;
            _databaseName = databaseName;
            _schemaName = schemaName;
            _handleError = handleError;
            _domainName = domainName;
        }

        public void UpdateDatabase(Func<MigratableDbContext> dbContextFactory)
        {
            try
            {
                Database.Migrate(dbContextFactory);
            }
            catch (Exception ex)
            {
                if (!HandleError(ex))
                {
                    throw;
                }
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
                if (!HandleError(ex))
                {
                    throw;
                }
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
                if (!HandleError(ex))
                {
                    throw;
                }
            }
        }

        private string ApiName => $"{_appName}Api";
        private string AppUserName => $@"IIS APPPOOL\{_appName}";
        private string ApiUserName => $@"IIS APPPOOL\{ApiName}";
        private string TaskUserName => $@"{_domainName}\{_appName}User";

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
