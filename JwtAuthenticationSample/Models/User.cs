namespace JwtAuthenticationSample.Models
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public string Email { get; set; }

        public string BirthDate { get; set; }

        public SexEnum Sex { get; set; }

    }
    public enum SexEnum { Male, Female, Unknown }
}
