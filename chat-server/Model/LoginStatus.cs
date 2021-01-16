using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
	public class tblLogedinStatus
	{
		public long LoginStatusId { get; set; }
		public string IpAddress { get; set; }
		public bool IsLoged { get; set; }
		public long UserId { get; set; }
		public DateTime Date { get; set; }
		public string Time { get; set; }
		public DateTime? LogOutDate { get; set; }
		public string LogOutTime { get; set; }
	}
	public class ReqLoginStatus
	{
		public string IpAddress { get; set; }
		public bool IsLogin { get; set; }
		public long UserId { get; set; }
	}
}
