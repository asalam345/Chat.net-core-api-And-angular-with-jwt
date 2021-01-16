using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
	public class MessageVM
	{
		public long ChatId { get; set; }
		public long SenderId { get; set; }
		public long ReceiverId { get; set; }
		public string Message { get; set; }
		public DateTime Date { get; set; }
		public string Time{ get; set; }
		public bool IsDeleteFromReceiver { get; set; }
		public bool IsDeleteFromSender { get; set; }
	}
}
