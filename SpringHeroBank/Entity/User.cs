namespace SpringHeroBank.Entity;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    
    public string Phone { get; set; }
    
    public string Email { get; set; }
    public bool IsAdmin { get; set; }
    public int Status { get; set; }
}