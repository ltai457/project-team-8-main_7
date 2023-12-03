namespace Project_Authentication.Model
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Role { get; set; }    = string.Empty; 
        public bool IsAuthenticated { get; set; }  
        public string? UserName { get; set; }    
        public int UserID { get; set; }  
    }
}
