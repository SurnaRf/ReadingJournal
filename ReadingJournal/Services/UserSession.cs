using BusinessLayer;

namespace ReadingJournal.Services
{
    public class UserSession
    {
        public string UserName { get; set; }

        public Role Role { get; set; }

        public string UserId { get; set; }
    }
}
