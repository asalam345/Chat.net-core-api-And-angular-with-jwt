using System;

namespace Models
{
    public partial class UserVM
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool ForLogin { get; set; }
    }
}
