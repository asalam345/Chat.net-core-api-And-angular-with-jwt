using System;
using System.Collections.Generic;
using System.Text;

namespace chat_server.Entity
{
	public class Result
	{
		public string Message { get; set; }
		public bool IsSuccess { get; set; }
		public object Data { get; set; }
	}
}
