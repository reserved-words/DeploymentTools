using System;

namespace DeploymentTools
{
    public class PostDeploymentService : DeploymentService
    {
        private readonly string _connectionString;
        private readonly string _databaseName;
        private readonly string _schemaName;

        public PostDeploymentService(string appName, string connectionString, string databaseName, string schemaName)
            : base(appName)
        {
            _connectionString = connectionString;
            _databaseName = databaseName;
            _schemaName = schemaName;
        }

        public void UpdateDatabase()
        {
            try
            {
                Database.Migrate(_connectionString, _schemaName);
            }
            catch (Exception ex)
            {
                Log(ex);
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
                Log(ex);
            }
        }

        private void GrantPermission(string userName, string permission, string objectName = null)
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
    }
}
