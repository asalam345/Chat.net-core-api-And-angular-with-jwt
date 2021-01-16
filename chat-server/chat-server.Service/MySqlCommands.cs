using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;

using Models;
using Models.interfaces;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace chat_server.Service
{
	public class MySqlCommands
	{
        private readonly string _connectionString;
        DataTable dataTable;
        public MySqlCommands()
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
        public async Task<long> getMaxRow(string columnName, string tableName)
        {
            try
            {
                string query = "SELECT MAX(" + columnName + ") FROM " + tableName + " WITH(NOLOCK)";
                DataTable dataTable = await GetData(query);
                long userId = dataTable.Rows.Count > 0 ? Convert.ToInt64(dataTable.Rows[0][0]) : 0;
                return userId;
            }
            catch (Exception ex)
            {
                return 0;
            }
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
		
		//public bool exceptionHandel(string className, string methodName, string ex)
		//{
		//    bool result = false;

		//    try
		//    {
		//        result = InsertOrUpdate("INSERT INTO Exception(ClassName, MethodName, Exp, DateOfEx, TimeOfEx) " +
		//            "VALUES('" + className + "', '" + methodName + "', '" + ex + "', '" +
		//            DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToLongTimeString() + "')");
		//    }
		//    catch (Exception e)
		//    {
		//        exceptionHandel("Connection", "exceptionHandel", e.ToString());
		//    }
		//    return result;
		//}
	}
}
