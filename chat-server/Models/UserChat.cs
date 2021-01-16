using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
	public class UserChat
	{
		public long ChatId { get; set; }
		public long SenderId { get; set; }
		public long ReciverId { get; set; }
		public string Message { get; set; }
		public DateTime date { get; set; }
	}
}
