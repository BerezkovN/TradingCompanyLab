/*var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("conFile.json")
    .Build();

string connectionString = configuration.GetConnectionString("Task1");
var connection = new SqlConnection(connectionString);*/


using System;
using System.Collections.Generic;
using DTO;
using DAL.Interface;
using DAL.AdoNet;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Extensions.Configuration;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the system!");

        var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("conFile.json")
    .Build();

        string connectionString = configuration.GetConnectionString("Task1");
        var connection = new SqlConnection(connectionString);


        IDatabase database = new Database(connectionString);
        IUserDal userDal = database.GetUserDal();
        IBankDetailDal bankDetailDal = database.GetBankDetailDal();
        ISessionDal sessionDal = database.GetSessionDal();

        while (true)
        {
            Console.WriteLine("\nPlease select an option:");
            Console.WriteLine("1. Log in");
            Console.WriteLine("Q. Quit");

            string choice = Console.ReadLine()?.Trim();

            if (choice?.ToUpper() == "Q")
            {
                Console.WriteLine("Goodbye!");
                break;
            }

            switch (choice)
            {
                case "1":
                    LogIn(userDal, bankDetailDal, sessionDal);
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    static void LogIn(IUserDal userDal, IBankDetailDal bankDetailDal, ISessionDal sessionDal)
    {
        Console.Write("Enter username: ");
        string username = Console.ReadLine()?.Trim();
        Console.Write("Enter password: ");
        string password = Console.ReadLine()?.Trim();

        UserData user = userDal.Login(username, password);

        if (user == null)
        {
            Console.WriteLine("Invalid username or password.");
            Console.Write("Do you want to recover your password? (yes/no): ");
            string recoverChoice = Console.ReadLine()?.Trim().ToLower();

            if (recoverChoice == "yes")
            {
                Console.Write("Enter your recovery key: ");
                string recoveryKey = Console.ReadLine()?.Trim();
                UserData recoveryUser = userDal.GetUserByUsername(username);

                if (recoveryUser != null && recoveryUser.RecoveryKey == recoveryKey)
                {
                    Console.Write("Enter a new password: ");
                    string newPassword = Console.ReadLine()?.Trim();
                    userDal.UpdateUser("Password", newPassword, recoveryUser.UserId);
                    Console.WriteLine("Your password has been updated. Please log in again.");
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid recovery key. Cannot recover password.");
                }
            }
            return;
        }

        // Створення сесії після успішного входу
        sessionDal.StartSession(user.UserId);
        Console.WriteLine($"Welcome, {user.Username}! Your role is {user.Role}.");

        if (user.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
        {
            AdminMenu(user, userDal, sessionDal, bankDetailDal);
        }
        else
        {
            UserMenu(user, userDal, bankDetailDal, sessionDal);
        }
    }



    static void AdminMenu(UserData admin, IUserDal userDal, ISessionDal sessionDal, IBankDetailDal bankDetailDal)
    {
        while (true)
        {
            Console.WriteLine("\nAdmin Menu:");
            Console.WriteLine("1. View all users");
            Console.WriteLine("2. View your profile");
            Console.WriteLine("3. Update your profile");
            Console.WriteLine("4. View detailed profile of a user");
            Console.WriteLine("5. Edit a user profile");
            Console.WriteLine("6. Delete a user profile");
            Console.WriteLine("7. View users sessions");
            Console.WriteLine("8. End a user session");
            Console.WriteLine("9. Log out");


            string choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                    List<UserData> users = userDal.GetAllUsers();
                    Console.WriteLine("\nUser List:");
                    foreach (var user in users)
                    {
                        Console.WriteLine($"ID: {user.UserId}, Username: {user.Username}, Role: {user.Role}");
                    }
                    break;

                case "2":
                    Console.WriteLine("\nYour Profile:");
                    Console.WriteLine($"Username: {admin.Username}");
                    Console.WriteLine($"Email: {admin.Email}");
                    Console.WriteLine($"Role: {admin.Role}");
                    Console.WriteLine($"FirstName: {admin.FirstName}");
                    Console.WriteLine($"LastName: {admin.LastName}");
                    Console.WriteLine($"Gender: {admin.Gender}");
                    Console.WriteLine($"PhoneNumber: {admin.PhoneNumber}");
                    Console.WriteLine($"Address: {admin.Address}");

                    if (admin.ProfilePicture != null && admin.ProfilePicture.Length > 0)
                    {
                        string outputFilePath = Path.Combine(Environment.CurrentDirectory, $"ProfilePicture_Admin_{admin.UserId}.jpg");
                        File.WriteAllBytes(outputFilePath, admin.ProfilePicture);
                        Console.WriteLine($"Profile picture exported to: {outputFilePath}");
                    }
                    else
                    {
                        Console.WriteLine("Profile Picture: Not uploaded");
                    }
                    break;

                case "3":
                    // Update admin's own profile
                    UpdateProfile(admin.UserId, userDal, bankDetailDal);
                    break;

                case "4":
                    // View detailed profile of another user
                    Console.Write("Enter the ID of the user to view their profile: ");
                    if (int.TryParse(Console.ReadLine(), out int detailedUserId))
                    {
                        UserData detailedUser = userDal.GetUser(detailedUserId);
                        if (detailedUser != null)
                        {
                            Console.WriteLine("\nUser Profile:");
                            Console.WriteLine($"Username: {detailedUser.Username}");
                            Console.WriteLine($"Email: {detailedUser.Email}");
                            Console.WriteLine($"Role: {detailedUser.Role}");
                            Console.WriteLine($"FirstName: {detailedUser.FirstName}");
                            Console.WriteLine($"LastName: {detailedUser.LastName}");
                            Console.WriteLine($"Gender: {detailedUser.Gender}");
                            Console.WriteLine($"PhoneNumber: {detailedUser.PhoneNumber}");
                            Console.WriteLine($"Address: {detailedUser.Address}");

                            if (detailedUser.ProfilePicture != null && detailedUser.ProfilePicture.Length > 0)
                            {
                                string outputFilePath = Path.Combine(Environment.CurrentDirectory, $"ProfilePicture_UserId_{detailedUser.UserId}.jpg");
                                File.WriteAllBytes(outputFilePath, detailedUser.ProfilePicture);
                                Console.WriteLine($"Profile picture exported to: {outputFilePath}");
                            }
                            else
                            {
                                Console.WriteLine("Profile Picture: Not uploaded");
                            }
                        }
                        else
                        {
                            Console.WriteLine("User not found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID. Please enter a valid user ID.");
                    }
                    break;

                case "5":
                    // Edit a user profile
                    Console.Write("Enter the ID of the user to edit their profile: ");
                    if (int.TryParse(Console.ReadLine(), out int userIdToEdit))
                    {
                        UserData userToEdit = userDal.GetUser(userIdToEdit);
                        if (userToEdit != null)
                        {
                            Console.WriteLine($"Editing profile for user: {userToEdit.Username}");
                            UpdateProfile(userIdToEdit, userDal, bankDetailDal);
                        }
                        else
                        {
                            Console.WriteLine("User not found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID. Please enter a valid user ID.");
                    }
                    break;

                case "6":
                    // Delete a user profile
                    Console.Write("Enter the ID of the user to delete: ");
                    if (int.TryParse(Console.ReadLine(), out int userIdToDelete))
                    {
                        UserData userToDelete = userDal.GetUser(userIdToDelete);
                        if (userToDelete != null)
                        {
                            Console.Write($"Are you sure you want to delete the profile of {userToDelete.Username}? (yes/no): ");
                            string confirmation = Console.ReadLine()?.ToLower();
                            if (confirmation == "yes")
                            {
                                userDal.DeleteUser(userIdToDelete);
                                Console.WriteLine("User profile deleted successfully.");
                            }
                            else
                            {
                                Console.WriteLine("User deletion canceled.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("User not found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID. Please enter a valid user ID.");
                    }
                    break; 
                case "7":
                    var userSessions = sessionDal.GetUserSessions();
                    Console.WriteLine("\nUser Sessions:");
                    foreach (var session in userSessions)
                    {
                        Console.WriteLine($"UserID: {session.UserId}, Username: {session.Username}, Status: {session.Status}, Login Time: {session.LoginTime}, Logout Time: {session.LogoutTime}");
                    }
                    break;
                case "8":
                    
                    Console.Write("Enter the ID of the user whose session you want to end: ");
                    if (int.TryParse(Console.ReadLine(), out int userIdToEndSession))
                    {
                        UserData userToEndSession = userDal.GetUser(userIdToEndSession);
                        if (userToEndSession != null)
                        {
                            Console.Write($"Are you sure you want to end the session for {userToEndSession.Username}? (yes/no): ");
                            string confirmation = Console.ReadLine()?.ToLower();
                            if (confirmation == "yes")
                            {
                                sessionDal.EndSession(userIdToEndSession);
                                Console.WriteLine($"Session for {userToEndSession.Username} has been ended.");
                            }
                            else
                            {
                                Console.WriteLine("Action canceled.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("User not found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID. Please enter a valid user ID.");
                    }
                    break;



                case "9":
                    sessionDal.EndSession(admin.UserId);
                    Console.WriteLine("Logged out");
                    return;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }



    static void UserMenu(UserData user, IUserDal userDal, IBankDetailDal bankDetailDal, ISessionDal sessionDal)
    {
        while (true)
        {
            Console.WriteLine("\nUser Menu:");
            Console.WriteLine("1. View profile");
            Console.WriteLine("2. Update profile");
            Console.WriteLine("3. Log out");

            string choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\nYour Profile:");
                    Console.WriteLine($"Username: {user.Username}");
                    Console.WriteLine($"Email: {user.Email}");
                    Console.WriteLine($"Role: {user.Role}");
                    Console.WriteLine($"FirstName: {user.FirstName}");
                    Console.WriteLine($"LastName: {user.LastName}");
                    Console.WriteLine($"Gender: {user.Gender}");
                    Console.WriteLine($"PhoneNumber: {user.PhoneNumber}");
                    Console.WriteLine($"Address: {user.Address}");

                    if (user.ProfilePicture != null && user.ProfilePicture.Length > 0)
                    {
                        Console.WriteLine("Profile Picture: Uploaded");

                        // Додати UserId до назви файлу
                        string outputFileName = $"ProfilePicture_UserId_{user.UserId}.jpg";
                        string outputFilePath = Path.Combine(Environment.CurrentDirectory, outputFileName);

                        // Зберегти файл
                        File.WriteAllBytes(outputFilePath, user.ProfilePicture);
                        Console.WriteLine($"Profile picture exported to: {outputFilePath}");
                    }
                    else
                    {
                        Console.WriteLine("Profile Picture: Not uploaded");
                    }


                    // Отримати банківські дані
                    BankDetailData bankDetails = bankDetailDal.GetBankDetailData(user.UserId);
                    if (bankDetails != null)
                    {
                        Console.WriteLine("\nBank Details:");
                        Console.WriteLine($"Card Number: {bankDetails.CardNumber}");
                        Console.WriteLine($"Expiration Date: {bankDetails.ExpirationDate}");
                        Console.WriteLine($"Card Holder Name: {bankDetails.CardHolderName}");
                        Console.WriteLine($"Billing BillingAddress: {bankDetails.BillingAddress}");
                    }
                    else
                    {
                        Console.WriteLine("\nNo bank details found.");
                    }
                    break;


                case "2":
                    UpdateProfile(user.UserId, userDal, bankDetailDal);
                    break;
                case "3":
                    sessionDal.EndSession(user.UserId);
                    Console.WriteLine("Logged out");
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
    static void UpdateProfile(int userId, IUserDal userDal, IBankDetailDal bankDetailDal)
    {
        while (true)
        {
            Console.WriteLine("Available fields to update:");
            Console.WriteLine("1. Username\n2. Email\n3. FirstName\n4. LastName\n5. Gender\n6. PhoneNumber\n7. Address\n8. Password\n9. Profile Picture\n10. Bank Details");
            Console.Write("Enter the field number to update or 0 to go back: ");
            string fieldChoice = Console.ReadLine();

            if (fieldChoice == "0")
                break;

            string field = FieldMapping(fieldChoice);
            if (field == "ProfilePicture")
            {
                Console.WriteLine("Save your profile picture in the following directory:");
                Console.WriteLine(Environment.CurrentDirectory);

                Console.Write("Enter the file name (e.g., profile.jpg): ");
                string fileName = Console.ReadLine()?.Trim();

                // Формуємо повний шлях
                string filePath = Path.Combine(Environment.CurrentDirectory, fileName);

                // Перевірка існування файлу
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("File not found in the current directory. Please try again.");
                    return;
                }

                try
                {
                    // Прочитати файл у байтовий масив
                    byte[] imageData = File.ReadAllBytes(filePath);

                    // Зберегти дані у базу
                    userDal.UpdateUser("ProfilePicture", imageData, userId);
                    Console.WriteLine("Profile picture uploaded successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while uploading profile picture: {ex.Message}");
                }


            }
            else if (field == "BankDetails")
            {
                Console.Write("Enter card number: ");
                string cardNumber = Console.ReadLine();
                if (!BankDetail.IsValidCardNumber(cardNumber))
                {
                    Console.WriteLine("Invalid card number. Please try again.");
                    return; // Повертаємося до меню, якщо номер картки недійсний
                }

                Console.Write("Enter expiration date (MM/YY): ");
                string expirationDate = Console.ReadLine();
                if (!BankDetail.IsValidExpirationDate(expirationDate))
                {
                    Console.WriteLine("Invalid or expired card. Please try again.");
                    return;
                }

                Console.Write("Enter CVV: ");
                string cvv = Console.ReadLine();
                if (!BankDetail.IsValidCVV(cvv))
                {
                    Console.WriteLine("Invalid CVV. Please try again.");
                    return;
                }

                Console.Write("Enter card holder name: ");
                string cardHolderName = Console.ReadLine();
                Console.Write("Enter billing address: ");
                string billingAddress = Console.ReadLine();

                BankDetailData bankDetail = new BankDetailData
                {
                    UserId = userId,
                    CardNumber = cardNumber,
                    ExpirationDate = expirationDate,
                    CardCVV = cvv,
                    CardHolderName = cardHolderName,
                    BillingAddress = billingAddress
                };

                bankDetailDal.UpdateBankDetail(bankDetail);
                Console.WriteLine("Bank details updated successfully.");
            }

            else if (field != null)
            {
                Console.Write("Enter new value: ");
                string value = Console.ReadLine();
                userDal.UpdateUser(field, value, userId);
                Console.WriteLine("Profile updated successfully.");
            }
            else
            {
                Console.WriteLine("Invalid field selection.");
            }
        }
    }

    static string FieldMapping(string choice)
    {
        return choice switch
        {
            "1" => "Username",
            "2" => "Email",
            "3" => "FirstName",
            "4" => "LastName",
            "5" => "Gender",
            "6" => "PhoneNumber",
            "7" => "Address",
            "8" => "Password",
            "9" => "ProfilePicture",
            "10" => "BankDetails",
            _ => null
        };
    }
}


