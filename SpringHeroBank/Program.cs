using MySql.Data.MySqlClient;
using SpringHeroBank;
using SpringHeroBank.Entity;
using SpringHeroBank.Repository;
class Program
    {
        static IUserRepository _userRepository;
        static ITransactionRepository _transactionRepository;

        static void Main(string[] args)
        {
            string connectionString = "Server=localhost;Database=SpringHeroBank;User=root;Password=yourpassword;";
            _userRepository = new UserRepository();
            _transactionRepository = new TransactionRepository();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to Spring Hero Bank!");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("0. Exit");
                Console.Write("Please select an option: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Register();
                        break;
                    case "2":
                        Login();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid option! Please try again.");
                        break;
                }
            }
        }

        static void Register()
        {
            Console.Clear();
            Console.WriteLine("Register a new account");
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();
            Console.Write("Full Name: ");
            string fullName = Console.ReadLine();
            Console.Write("Phone: ");
            string phone = Console.ReadLine();
            Console.Write("Email: ");
            string email = Console.ReadLine();

            User user = new User
            {
                Username = username,
                Password = password,
                FullName = fullName,
                Phone = phone,
                Email = email,
                Status = 1,
                IsAdmin = false
            };

            if (_userRepository.Register(user) != null)
            {
                Console.WriteLine("Registration successful!");
            }
            else
            {
                Console.WriteLine("Registration failed!");
            }
            Console.ReadLine();
        }

        static void Login()
        {
            Console.Clear();
            Console.WriteLine("Login to your account");
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();

            User user = _userRepository.Authenticate(username, password);
            if (user != null)
            {
                Console.WriteLine("Login successful!");
                if (user.IsAdmin)
                {
                    AdminMenu(user);
                }
                else
                {
                    UserMenu(user);
                }
            }
            else
            {
                Console.WriteLine("Login failed! Invalid username or password.");
            }
            Console.ReadLine();
        }

        static void AdminMenu(User admin)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Admin Menu");
                Console.WriteLine("1. View all users");
                Console.WriteLine("2. Search user by name");
                Console.WriteLine("3. Lock user");
                Console.WriteLine("4. Unlock user");
                Console.WriteLine("0. Logout");
                Console.Write("Please select an option: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ViewAllUsers();
                        break;
                    case "2":
                        SearchUserByName();
                        break;
                    case "3":
                        LockUser();
                        break;
                    case "4":
                        UnlockUser();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid option! Please try again.");
                        break;
                }
            }
        }

        static void UserMenu(User user)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("User Menu");
                Console.WriteLine("1. View balance");
                Console.WriteLine("2. View transactions");
                Console.WriteLine("3. Transfer money");
                Console.WriteLine("4. Change password");
                Console.WriteLine("5. Update information");
                Console.WriteLine("0. Logout");
                Console.Write("Please select an option: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ViewBalance(user);
                        break;
                    case "2":
                        ViewTransactions(user);
                        break;
                    case "3":
                        TransferMoney(user);
                        break;
                    case "4":
                        ChangePassword(user);
                        break;
                    case "5":
                        UpdateInformation(user);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid option! Please try again.");
                        break;
                }
            }
        }

        static void ViewAllUsers()
        {
            var users = _userRepository.GetAllUsers();
            Console.Clear();
            Console.WriteLine("All Users:");
            foreach (var user in users)
            {
                Console.WriteLine($"Username: {user.Username}, FullName: {user.FullName}, Email: {user.Email}, Phone: {user.Phone}, Status: {(user.Status == 1 ? "Active" : "Locked")}");
            }
            Console.ReadLine();
        }

        static void SearchUserByName()
        {
            Console.Clear();
            Console.Write("Enter name to search: ");
            string name = Console.ReadLine();
            var users = _userRepository.SearchUserByName(name);
            Console.WriteLine("Search Results:");
            foreach (var user in users)
            {
                Console.WriteLine($"Username: {user.Username}, FullName: {user.FullName}, Email: {user.Email}, Phone: {user.Phone}, Status: {(user.Status == 1 ? "Active" : "Locked")}");
            }
            Console.ReadLine();
        }

        static void LockUser()
        {
            Console.Clear();
            Console.Write("Enter username to lock: ");
            string username = Console.ReadLine();
            if (_userRepository.LockUser(username))
            {
                Console.WriteLine("User locked successfully!");
            }
            else
            {
                Console.WriteLine("Failed to lock user!");
            }
            Console.ReadLine();
        }

        static void UnlockUser()
        {
            Console.Clear();
            Console.Write("Enter username to unlock: ");
            string username = Console.ReadLine();
            if (_userRepository.UnlockUser(username))
            {
                Console.WriteLine("User unlocked successfully!");
            }
            else
            {
                Console.WriteLine("Failed to unlock user!");
            }
            Console.ReadLine();
        }

        static void ViewBalance(User user)
        {
            var balance = _userRepository.GetBalance(user.Username);
            Console.Clear();
            Console.WriteLine($"Your balance is: {balance}");
            Console.ReadLine();
        }

        static void ViewTransactions(User user)
        {
            Console.Clear();
            Console.Write("Enter page number: ");
            int pageNumber = int.Parse(Console.ReadLine());
            Console.Write("Enter page size: ");
            int pageSize = int.Parse(Console.ReadLine());
            var transactions = _transactionRepository.GetTransactionsByAccountNumber(user.Username, pageNumber, pageSize);
            Console.WriteLine("Transactions:");
            foreach (var transaction in transactions)
            {
                Console.WriteLine($"Type: {transaction.TransactionType}, Amount: {transaction.Amount}, Date: {transaction.TransacAmounttionDate}");
            }
            Console.ReadLine();
        }

        static void TransferMoney(User user)
        {
            Console.Clear();
            Console.Write("Enter recipient username: ");
            string recipientUsername = Console.ReadLine();
            Console.Write("Enter amount to transfer: ");
            decimal amount = decimal.Parse(Console.ReadLine());
            
            if (_userRepository.GetBalance(user.Username) >= amount)
            {
                var recipient = _userRepository.GetUserByUsername(recipientUsername);
                if (recipient != null)
                {
                    var transaction = new Transaction
                    {
                        AccountNumber = user.Username,
                        TransactionType = "Debit",
                        Amount = -amount,
                        TransacAmounttionDate = DateTime.Now
                    };
                    _transactionRepository.AddTransaction(transaction);

                    var recipientTransaction = new Transaction
                    {
                        AccountNumber = recipientUsername,
                        TransactionType = "Credit",
                        Amount = amount,
                        TransacAmounttionDate = DateTime.Now
                    };
                    _transactionRepository.AddTransaction(recipientTransaction);

                    Console.WriteLine("Transfer successful!");
                }
                else
                {
                    Console.WriteLine("Recipient not found!");
                }
            }
            else
            {
                Console.WriteLine("Insufficient balance!");
            }
            Console.ReadLine();
        }

        static void ChangePassword(User user)
        {
            Console.Clear();
            Console.Write("Enter current password: ");
            string currentPassword = Console.ReadLine();
            Console.Write("Enter new password: ");
            string newPassword = Console.ReadLine();

            if (_userRepository.UpdatePassword(user.Username, currentPassword, newPassword))
            {
                Console.WriteLine("Password changed successfully!");
            }
            else
            {
                Console.WriteLine("Failed to change password!");
            }
            Console.ReadLine();
        }

        static void UpdateInformation(User user)
        {
            Console.Clear();
            Console.Write("Enter new full name: ");
            string fullName = Console.ReadLine();
            Console.Write("Enter new phone: ");
            string phone = Console.ReadLine();
            Console.Write("Enter new email: ");
            string email = Console.ReadLine();

            user.FullName = fullName;
            user.Phone = phone;
            user.Email = email;

            if (_userRepository.UpdateUserInformation(user))
            {
                Console.WriteLine("Information updated successfully!");
            }
            else
            {
                Console.WriteLine("Failed to update information!");
            }
            Console.ReadLine();
        }
    }