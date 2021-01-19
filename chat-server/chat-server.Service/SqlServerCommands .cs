using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using chat_server.Entity;
using chat_server.Entity.interfaces;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace chat_server.Service
{
	public class SqlServerCommands
	{
        private readonly string _connectionString;
        DataTable dataTable;
        public SqlServerCommands()
        {
            _connectionString = ConnectionStringManager.GetConnectionString();
        }
        public Result DefaultResult(string message)
        {
            Result result = new Result();
            result.Message = message;
            result.IsSuccess = true;
            result.Data = null;

            return result;
        }
        public async Task<Result> InsertOrUpdateOrDelete(string query)
		{
            Result result = DefaultResult("");
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, sql))
                    {
                        cmd.CommandType = CommandType.Text;
                        if (sql.State == ConnectionState.Closed)
                            await sql.OpenAsync();

                        long exResult = 0;
                        if (query.Contains("output"))
                        {
                            exResult = (long)cmd.ExecuteScalar();
                            result.Message = exResult.ToString();
                        }
                        else
                        {
                            exResult = cmd.ExecuteNonQuery();
                        }
                        if (exResult > 0)
                        {
                            result.IsSuccess = true;
                        }
                        else
                        {
                            result.IsSuccess = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //exceptionHandel("Connection", "GetData", ex.ToString());
                result.IsSuccess = false;
            }
            finally
            {
            }
            return result;
		}
		public async Task<DataTable> GetData(string query)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, sql))
                    {
                        cmd.CommandType = CommandType.Text;
                        if (sql.State == ConnectionState.Closed)
                            await sql.OpenAsync();
                        using (var reader = await cmd.ExecuteReaderAsync())
						{
                            dataTable = new DataTable();
                            dataTable.Load(reader);
						}
                    }
                }
                
               
                return dataTable;
            }
            catch (Exception ex)
            {
                //exceptionHandel("Connection", "GetData", ex.ToString());
                return null;
            }
            finally
            {
            }
        }
	}
}
