namespace DTO
{
    public class UserData
    {
        public int UserId { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string? FirstName { get; set; } 

        public string? LastName { get; set; }  

        public string? Gender { get; set; } 

        public string? PhoneNumber { get; set; } 

        public string? Address { get; set; }

        public string RecoveryKey { get; set; }

        public byte[]? ProfilePicture { get; set; }

        public string Role { get; set; }

        public DateTime CreatedAt { get; set; } 

        public DateTime? UpdatedAt { get; set; } 

    }
}