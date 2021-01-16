using Models;
using Models.interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using chat_server.Service;
using System.Data;

namespace DAL
{
	public class DA_User : MySqlCommands, IGenericService<UserVM>
	{
		public DA_User()
		{
		}
		public async Task<Result> Delete(long id)
		{
			Result result = DefaultResult("Success");
			try
			{
				string query = "Delete FROM tblUser WHERE UserId = " + id;
				result.Message = (await InsertOrUpdateOrDelete(query)).IsSuccess ? "Delete Successfullly" : "User Not Found!";
				result.Data = null;
			}
			catch (Exception ex)
			{
				result.Message = "UnSuccess";
				result.IsSuccess = false;
				result.Data = ex.Message;
			}

			return await Task.FromResult<Result>(result); 
		}

		public async Task<Result> Entry(UserVM _model)
		{
			Result result = DefaultResult("Registered Successfully!");
			
			try
			{
					UserVM ui = await isExists(_model.Email);
					if (ui != null)
					{
						result.Message = "Email Alreay Exists!";
						result.IsSuccess = false;
						return await Task.FromResult<Result>(result);
					}
					ui = await isExists(_model.FirstName, _model.LastName);
					if (ui != null)
					{
						result.Message = "Name Alreay Exists!";
						result.IsSuccess = false;
						return await Task.FromResult<Result>(result);
					}
					//long userId = await getMaxRow("UserId", "tblUser") + 1;				

					string query = @"INSERT INTO tblUser(UserId, email,firstname,lastName) VALUES
								((SELECT 1 + coalesce(max(UserId), 0) FROM tblUser),'" 
					+ _model.Email + "','" + _model.FirstName + "', '" + _model.LastName + "')";
					await InsertOrUpdateOrDelete(query);
			}
			catch (Exception ex)
			{
				result.Message = "Error:" + ex.InnerException;
				result.IsSuccess = false;
				result.Data = ex.Message;
			}
			return await Task.FromResult<Result>(result);
		}
		async Task<UserVM> isExists(string email)
		{
			try
			{
				string query = "SELECT * FROM tblUser WITH(NOLOCK) WHERE Email = '" + email + "'";
				DataTable dataTable = await GetData(query);
				DataRow dr = dataTable.Rows[0];
				UserVM user = Converter.GetItem<UserVM>(dr);
				return user;
			}
			catch(Exception ex)
			{
				return null;
			}
		}
		async Task<UserVM> isExists(string fristName, string lastName)
		{
			try
			{
				string query = "SELECT * FROM tblUser WITH(NOLOCK) WHERE FirstName = '" + fristName + "' AND LastName = '" + fristName + "'";
				DataTable dataTable = await GetData(query);
				DataRow dr = dataTable.Rows[0];
				UserVM user = Converter.GetItem<UserVM>(dr);
				return user;
			}
			catch (Exception ex)
			{
				return null;
			}
		}
		
		public async Task<Result> Get(UserVM model)
		{
			Result result = DefaultResult("Success");

			try
			{
				IEnumerable<UserVM> users;

				if (model != null)
				{
					if (model.ForLogin)
					{
						UserVM user = await isExists(model.Email);
						result.Data = user;
					}
					else
					{
						string query = "SELECT * FROM tblUser WITH(NOLOCK) WHERE USERID != " + model.UserId;
						DataTable dataTable = await GetData(query);
						users = Converter.ConvertDataTable<UserVM>(dataTable);
						result.Data = users;
					}
				}
				else
				{
					string query = "SELECT * FROM tblUser WITH(NOLOCK)";
					DataTable dataTable = await GetData(query);
					users = Converter.ConvertDataTable<UserVM>(dataTable);
					result.Data = users;
				}
			}
			catch (Exception ex)
			{
				result.Message = "UnSuccess";
				result.IsSuccess = false;
				result.Data = ex.Message;
			}

			return await Task.FromResult<Result>(result);
		}

		public async Task<Result> Update(UserVM model)
		{
			Result result = DefaultResult("Success");
			try
			{
					
			}
			catch (Exception ex)
			{
				result.Message = "UnSuccess";
				result.IsSuccess = false;
				result.Data = ex.Message;
			}

			return await Task.FromResult<Result>(result);
		}
	}
}
