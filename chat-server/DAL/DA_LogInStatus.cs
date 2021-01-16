using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chat_server.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models;
using Models.interfaces;

namespace DAL
{
	public class DA_LogInStatus : MySqlCommands, IGenericService<tblLogedinStatus>
    {
        public DA_LogInStatus() 
        {
        }
        public Task<Result> Delete(long id)
		{
			throw new NotImplementedException();
		}

		public async Task<Result> Entry(tblLogedinStatus model)
		{
            Result result = DefaultResult("Successfully!");
            try
            {
                if (model.IsLoged)
                {
                    //long loginStatusId = await getMaxRow("LoginStatusId", "LogInStatus") + 1;
                    DateTime dt = DateTime.Now;
                    string query = @"INSERT INTO LogInStatus(LoginStatusId,IpAddress,UserId,IsLoged,Date,Time) VALUES
((SELECT 1 + coalesce(max(LoginStatusId), 0) FROM LogInStatus),'" + model.IpAddress + "','" + model.UserId + "', true,GetDate(),'"
                        + dt.ToShortTimeString() + "')";
                    result = await InsertOrUpdateOrDelete(query);
                }
                else
                {
                    await Update(model);
                }

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "Error:" + ex.InnerException;
            }

            return result;
        }

		public Task<Result> Get(tblLogedinStatus model)
		{
			throw new NotImplementedException();
		}

		public async Task<Result> Update(tblLogedinStatus model)
		{
            Result result = DefaultResult("Successfully!");
            try
            {
                DateTime dt = DateTime.Now;
                string query = "UPDATE LogInStatus SET IsLoged = false,Date=GetDate(),Time = '"
                    + dt.ToShortTimeString() + "' WHERE IpAddress='" + model.IpAddress 
                    + "' AND UserId = " + model.UserId;
                result = await InsertOrUpdateOrDelete(query);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "Error:" + ex.InnerException;
            }

            return result;
        }

		
    }
}
