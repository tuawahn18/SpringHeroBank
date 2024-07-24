using MySql.Data.MySqlClient;
using SpringHeroBank.Entity;

namespace SpringHeroBank.Repository;

public class UserRepository : IUserRepository
{
    private const string MyConnectionString = "server=127.0.0.1;uid=root;pwd=;database=springherobank";
    public User Register(User user)
    {
        try
        {
            using (MySqlConnection conn = new MySqlConnection(MyConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO user (Username, Password, FullName, Phone, Email, Status, IsAdmin) VALUES (@Username, @Password, @FullName, @Phone, @Email, @Status, @IsAdmin)", conn);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@FullName", user.FullName);
                cmd.Parameters.AddWithValue("@Phone", user.Phone);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Status", user.Status);
                cmd.Parameters.AddWithValue("@IsAdmin", user.IsAdmin);

                cmd.ExecuteNonQuery();
                user.Id = (int)cmd.LastInsertedId;
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
        return user;
    }

    public User Authenticate(string username, string password)
    {
        User user = null;
        try
        {
            using (MySqlConnection conn = new MySqlConnection(MyConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM user WHERE Username = @Username AND Password = @Password", conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            Id = reader.GetInt32("Id"),
                            Username = reader.GetString("Username"),
                            Password = reader.GetString("Password"),
                            FullName = reader.GetString("FullName"),
                            Phone = reader.GetString("Phone"),
                            Email = reader.GetString("Email"),
                            Status = reader.GetInt32("Status"),
                            IsAdmin = reader.GetBoolean("IsAdmin")
                        };
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
        return user;
    }

    public User GetUserByUsername(string username)
    {
        User user = null;
        try
        {
            using (MySqlConnection conn = new MySqlConnection(MyConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM user WHERE Username = @Username", conn);
                cmd.Parameters.AddWithValue("@Username", username);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            Id = reader.GetInt32("Id"),
                            Username = reader.GetString("Username"),
                            Password = reader.GetString("Password"),
                            FullName = reader.GetString("FullName"),
                            Phone = reader.GetString("Phone"),
                            Email = reader.GetString("Email"),
                            Status = reader.GetInt32("Status"),
                            IsAdmin = reader.GetBoolean("IsAdmin")
                        };
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
        return user;
    }

    public bool UpdateUserInformation(User user)
    {
        try
        {
            using (MySqlConnection conn = new MySqlConnection(MyConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE user SET FullName = @FullName, Phone = @Phone, Email = @Email WHERE Username = @Username", conn);
                cmd.Parameters.AddWithValue("@FullName", user.FullName);
                cmd.Parameters.AddWithValue("@Phone", user.Phone);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Username", user.Username);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool UpdatePassword(string username, string currentPassword, string newPassword)
    {
        try
        {
            using (MySqlConnection conn = new MySqlConnection(MyConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE user SET Password = @NewPassword WHERE Username = @Username AND Password = @CurrentPassword", conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@CurrentPassword", currentPassword);
                cmd.Parameters.AddWithValue("@NewPassword", newPassword);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public List<User> GetAllUsers()
    {
        List<User> users = new List<User>();
        try
        {
            using (MySqlConnection conn = new MySqlConnection(MyConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM user", conn);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = reader.GetInt32("Id"),
                            Username = reader.GetString("Username"),
                            Password = reader.GetString("Password"),
                            FullName = reader.GetString("FullName"),
                            Phone = reader.GetString("Phone"),
                            Email = reader.GetString("Email"),
                            Status = reader.GetInt32("Status"),
                            IsAdmin = reader.GetBoolean("IsAdmin")
                        });
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
        return users;
    }

    public List<User> SearchUserByName(string name)
    {
        List<User> users = new List<User>();
        try
        {
            using (MySqlConnection conn = new MySqlConnection(MyConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM user WHERE FullName LIKE @Name", conn);
                cmd.Parameters.AddWithValue("@Name", "%" + name + "%");

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = reader.GetInt32("Id"),
                            Username = reader.GetString("Username"),
                            Password = reader.GetString("Password"),
                            FullName = reader.GetString("FullName"),
                            Phone = reader.GetString("Phone"),
                            Email = reader.GetString("Email"),
                            Status = reader.GetInt32("Status"),
                            IsAdmin = reader.GetBoolean("IsAdmin")
                        });
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
        return users;
    }

    public User SearchUserByAccountNumber(string accountNumber)
    {
        User user = null;
        try
        {
            using (MySqlConnection conn = new MySqlConnection(MyConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM user WHERE Username = @AccountNumber", conn);
                cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            Id = reader.GetInt32("Id"),
                            Username = reader.GetString("Username"),
                            Password = reader.GetString("Password"),
                            FullName = reader.GetString("FullName"),
                            Phone = reader.GetString("Phone"),
                            Email = reader.GetString("Email"),
                            Status = reader.GetInt32("Status"),
                            IsAdmin = reader.GetBoolean("IsAdmin")
                        };
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
        return user;
    }

    public User SearchUserByPhoneNumber(string phoneNumber)
    {
        User user = null;
        try
        {
            using (MySqlConnection conn = new MySqlConnection(MyConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM user WHERE Phone = @PhoneNumber", conn);
                cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            Id = reader.GetInt32("Id"),
                            Username = reader.GetString("Username"),
                            Password = reader.GetString("Password"),
                            FullName = reader.GetString("FullName"),
                            Phone = reader.GetString("Phone"),
                            Email = reader.GetString("Email"),
                            Status = reader.GetInt32("Status"),
                            IsAdmin = reader.GetBoolean("IsAdmin")
                        };
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
        return user;
    }

    public bool LockUser(string accountNumber)
    {
        try
        {
            using (MySqlConnection conn = new MySqlConnection(MyConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE user SET Status = 0 WHERE Username = @AccountNumber", conn);
                cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool UnlockUser(string accountNumber)
    {
        try
        {
            using (MySqlConnection conn = new MySqlConnection(MyConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE user SET Status = 1 WHERE Username = @AccountNumber", conn);
                cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public decimal GetBalance(string accountNumber)
    {
        decimal balance = 0;
        try
        {
            using (MySqlConnection conn = new MySqlConnection(MyConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT SUM(Amount) FROM transaction WHERE AccountNumber = @AccountNumber", conn);
                cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);

                var result = cmd.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    balance = Convert.ToDecimal(result);
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
        return balance;
    }
}
