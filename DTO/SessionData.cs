namespace DTO
{
    public class SessionData
    {
        public int UserId { get; set; }

        public string Status{ get; set; }

        public DateTime? LoginTime { get; set; }

        public DateTime? LogoutTime { get; set; }

    }
}
