using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
	public class FriendRequest
	{
		[Key]
        public string RequestId { get; set; } 

        [ForeignKey("Sender")]
		[DisplayName("Sender")]
		public string SenderId { get; set; }
		public User Sender { get; set; }

        [ForeignKey("Receiver")]
        [DisplayName("Receiver")]
        public string ReceiverId { get; set; }
		public User Receiver { get; set; }
		public bool IsAccepted { get; set; }

        public FriendRequest()
        {

        }

        public FriendRequest(User sender, User receiver)
        {
            RequestId = Guid.NewGuid().ToString();
            Sender = sender;
            Receiver = receiver;
        }
    }

}
