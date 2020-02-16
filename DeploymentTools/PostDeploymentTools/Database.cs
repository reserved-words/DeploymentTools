using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PostDeploymentTools
{
    internal static class Database
    {
        public static void Migrate(Func<MigratableDbContext> dbContextFactory)
        {
            using (var dbContext = dbContextFactory())
            {
                dbContext.Database.Migrate();
            }
        }

        public static void CreateUser(string connectionString, string databaseName, string schemaName, string userName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@DatabaseName", databaseName),
                    new SqlParameter("@SchemaName", schemaName),
                    new SqlParameter("@UserName", userName)
                };

                using (var command = new SqlCommand(SqlScripts.CreateUser, connection))
                {
                    command.Parameters.AddRange(parameters.ToArray());
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public static void GrantSchemaPermission(string connectionString, string permission, string schemaName, string userName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Permission", permission),
                    new SqlParameter("@SchemaName", schemaName),
                    new SqlParameter("@UserName", userName)
                };

                using (var command = new SqlCommand(SqlScripts.GrantSchemaPermission, connection))
                {
                    command.Parameters.AddRange(parameters.ToArray());
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public static void GrantObjectPermission(string connectionString, string permission, string schemaName, string objectName, string userName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Permission", permission),
                    new SqlParameter("@SchemaName", schemaName),
                    new SqlParameter("@ObjectName", objectName),
                    new SqlParameter("@UserName", userName)
                };

                using (var command = new SqlCommand(SqlScripts.GrantObjectPermission, connection))
                {
                    command.Parameters.AddRange(parameters.ToArray());
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }
}
