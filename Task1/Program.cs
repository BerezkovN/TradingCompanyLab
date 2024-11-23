
using System;
using System.Collections.Generic;
using DTO;
using DAL.Interface;
using DAL.AdoNet;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Extensions.Configuration;
using BusinessLogic;
using System.Data.Entity;

class Program
{
    private static void Main(string[] args)
    {

        try
        {
            Start();
        }
        finally
        {
            Console.WriteLine("Exiting"); 
        }
    }

    private static void Start()
    {

        Console.WriteLine("Welcome to the system!");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json")
            .Build();

        string? connectionString = configuration.GetConnectionString("SqlServer");

        if (connectionString == null)
        {
            Console.WriteLine("Unable to connect to the SQL server");
            return;
        }


        IDatabase database = new DAL.AdoNet.Database(connectionString);

        while (true)
        {
            Console.WriteLine("\nPlease select an option:");
            Console.WriteLine("1. Log in");
            Console.WriteLine("Q. Quit");

            string? choice = Console.ReadLine();

            if (choice == null)
                continue;

            if (choice?.ToLower() == "q")
            {
                Console.WriteLine("Goodbye!");
                break;
            }

            switch (choice)
            {
                case "1":
                    LogIn(database);
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private static void LogIn(IDatabase database)
    {
        Console.Write("Enter username: ");
        string? username = Console.ReadLine();
        
        Console.Write("Enter password: ");
        string? password = Console.ReadLine();

        if (username == null || password == null)
            return;

        UserData? user = database.UserDal.Login(username, password);

        if (user == null)
        {
            PasswordRecover(database, username);
            return;
        }

        // Створення сесії після успішного входу
        database.SessionDal.StartSession(user.UserId);
        Console.WriteLine($"Welcome, {user.Username}! Your role is {user.Role}.");

        if (user.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
        {
            AdminMenu(database, user);
        }
        else
        {
            UserMenu(database, user);
        }
    }

    private static void PasswordRecover(IDatabase database, string username)
    {
        Console.WriteLine("Invalid username or password.");
        Console.Write("Do you want to recover your password? (yes/no): ");
        string? recoverChoice = Console.ReadLine();

        if (recoverChoice == null)
            return;

        if (recoverChoice.ToLower() != "yes")
            return;

        Console.Write("Enter your recovery key: ");
        string? recoveryKey = Console.ReadLine();

        if (recoveryKey == null)
            return;

        UserData recoveryUser = database.UserDal.GetUserByUsername(username);

        if (recoveryUser == null || recoveryUser.RecoveryKey != recoveryKey)
        {
            Console.WriteLine("Invalid recovery key. Cannot recover password.");
            return;
        }

        Console.Write("Enter a new password: ");
        string? newPassword = Console.ReadLine();

        if (newPassword == null)
        {
            return;
        }

        database.UserDal.UpdateUser("Password", newPassword, recoveryUser.UserId);
        Console.WriteLine("Your password has been updated. Please log in again.");
        return;
    }


    static void AdminMenu(IDatabase database, UserData admin)
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


            string? choice = Console.ReadLine();
            if (choice == null)
                return;


            switch (choice)
            {
                case "1":
                    List<UserData> users = database.UserDal.GetAllUsers();
                    Console.WriteLine("\nUser List:");
                    foreach (var user in users)
                    {
                        Console.WriteLine($"ID: {user.UserId}, Username: {user.Username}, Role: {user.Role}");
                    }
                    break;

                case "2":
                    ViewProfile(database, admin);
                    break;

                case "3":
                    UpdateProfile(database, admin.UserId);
                    break;

                case "4":
                    // View detailed profile of another user
                    Console.Write("Enter the ID of the user to view their profile: ");
                    if (int.TryParse(Console.ReadLine(), out int detailedUserId))
                    {
                        UserData detailedUser = database.UserDal.GetUser(detailedUserId);
                        if (detailedUser != null)
                        {
                            ViewProfile(database, detailedUser);
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

                // Edit a user profile
                case "5":
                    Console.Write("Enter the ID of the user to edit their profile: ");
                    if (!int.TryParse(Console.ReadLine(), out int userIdToEdit))
                    {
                        Console.WriteLine("Invalid ID. Please enter a valid user ID.");
                        continue;
                    }

                    UserData userToEdit = database.UserDal.GetUser(userIdToEdit);
                    if (userToEdit != null)
                    {
                        Console.WriteLine($"Editing profile for user: {userToEdit.Username}");
                        UpdateProfile(database, userIdToEdit);
                    }
                    else
                    {
                        Console.WriteLine("User not found.");
                    }
                    break;

                // Delete a user profile
                case "6":
                    Console.Write("Enter the ID of the user to delete: ");
                    if (!int.TryParse(Console.ReadLine(), out int userIdToDelete))
                    {
                        Console.WriteLine("Invalid ID. Please enter a valid user ID.");
                        continue;
                    }

                    UserData userToDelete = database.UserDal.GetUser(userIdToDelete);
                    if (userToDelete != null)
                    {
                        Console.Write($"Are you sure you want to delete the profile of {userToDelete.Username}? (yes/no): ");
                        string? confirmation = Console.ReadLine();

                        if (confirmation == "yes")
                        {
                            database.UserDal.DeleteUser(userIdToDelete);
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
                    
                    break; 
                case "7":

                    var userSessions = database.SessionDal.GetUserSessions();

                    Console.WriteLine("\nUser Sessions:");
                    foreach (var session in userSessions)
                    {
                        Console.WriteLine($"UserID: {session.UserId}, Status: {session.Status}, Login Time: {session.LoginTime}, Logout Time: {session.LogoutTime}");
                    }
                    break;

                case "8":
                    
                    Console.Write("Enter the ID of the user whose session you want to end: ");
                    if (!int.TryParse(Console.ReadLine(), out int userIdToEndSession))
                    {
                        Console.WriteLine("Invalid ID. Please enter a valid user ID.");
                        continue;
                    }

                    UserData userToEndSession = database.UserDal.GetUser(userIdToEndSession);
                    if (userToEndSession != null)
                    {
                        Console.Write($"Are you sure you want to end the session for {userToEndSession.Username}? (yes/no): ");
                        string? confirmation = Console.ReadLine();
                        if (confirmation == "yes")
                        {
                            database.SessionDal.EndSession(userIdToEndSession);
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
                    break;


                case "9":
                    database.SessionDal.EndSession(admin.UserId);
                    Console.WriteLine("Logged out");
                    return;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private static void UserMenu(IDatabase database, UserData user)
    {
        while (true)
        {
            Console.WriteLine("\nUser Menu:");
            Console.WriteLine("1. View profile");
            Console.WriteLine("2. Update profile");
            Console.WriteLine("3. Log out");

            string? choice = Console.ReadLine();
            if (choice == null)
                continue;

            switch (choice)
            {
                case "1":
                    ViewProfile(database, user);
                    break;
                case "2":
                    UpdateProfile(database, user.UserId);
                    break;
                case "3":
                    database.SessionDal.EndSession(user.UserId);
                    Console.WriteLine("Logged out");
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private static void ViewProfile(IDatabase database, UserData user)
    {
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
        BankDetailData? bankDetails = database.BankDetailDal.GetBankDetailData(user.UserId);
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
    }

    private static void UpdateProfile(IDatabase database, int userId)
    {
        while (true)
        {
            Console.WriteLine("Available fields to update:");
            Console.WriteLine("1. Username"); 
            Console.WriteLine("2. Email");
            Console.WriteLine("3. FirstName");
            Console.WriteLine("4. LastName");
            Console.WriteLine("5. Gender"); 
            Console.WriteLine("6. PhoneNumber"); 
            Console.WriteLine("7. Address"); 
            Console.WriteLine("8. Password"); 
            Console.WriteLine("9. Profile Picture"); 
            Console.WriteLine("10. Bank Details");
            
            Console.Write("Enter the field number to update or 0 to go back: ");

            if (!int.TryParse(Console.ReadLine(), out int fieldChoice))
                continue;

            if (fieldChoice == 0)
                break;

            UpdateProfileChoice choice = (UpdateProfileChoice)fieldChoice;
            if (!Enum.IsDefined(choice))
            {
                Console.WriteLine("Invalid field selection.");
                continue;
            }

            if (choice == UpdateProfileChoice.ProfilePicture)
            {
                UpdatePicture(database, userId);
            }
            else if (choice == UpdateProfileChoice.BankDetails)
            {
                UpdateBankDetails(database, userId);
            }
            else
            {
                Console.Write("Enter new value: ");
                string? value = Console.ReadLine();
                if (value == null)
                    continue;

                database.UserDal.UpdateUser(choice.ToString(), value, userId);
                Console.WriteLine("Profile updated successfully.");
            }
        }
    }

    private enum UpdateProfileChoice
    {
        Username = 1,
        Email,
        FirstName,
        LastName,
        Gender,
        PhoneNumber,
        Address,
        Password,
        ProfilePicture,
        BankDetails
    }

    private static void UpdateBankDetails(IDatabase database, int userId)
    {
        Console.Write("Enter card number: ");
        string? cardNumber = Console.ReadLine();
        if (cardNumber == null)
            return;

        if (!BankDetailData.IsValidCardNumber(cardNumber))
        {
            Console.WriteLine("Invalid card number. Please try again.");
            return; // Повертаємося до меню, якщо номер картки недійсний
        }

        Console.Write("Enter expiration date (MM/YY): ");
        string? expirationDate = Console.ReadLine();
        if (expirationDate == null)
            return;
        
        if (!BankDetailData.IsValidExpirationDate(expirationDate))
        {
            Console.WriteLine("Invalid or expired card. Please try again.");
            return;
        }

        Console.Write("Enter CVV: ");
        string? cvv = Console.ReadLine();
        if (cvv == null)
            return;

        if (!BankDetailData.IsValidCVV(cvv))
        {
            Console.WriteLine("Invalid CVV. Please try again.");
            return;
        }

        Console.Write("Enter card holder name: ");
        string? cardHolderName = Console.ReadLine();
        if (cardHolderName == null)
            return;

        Console.Write("Enter billing address: ");
        string? billingAddress = Console.ReadLine();
        if (billingAddress == null)
            return;

        BankDetailData bankDetail = new BankDetailData
        {
            UserId = userId,
            CardNumber = cardNumber,
            ExpirationDate = expirationDate,
            CardCVV = cvv,
            CardHolderName = cardHolderName,
            BillingAddress = billingAddress
        };

        database.BankDetailDal.UpdateBankDetail(bankDetail);
        Console.WriteLine("Bank details updated successfully.");
    }

    private static void UpdatePicture(IDatabase database, int userId)
    {
        Console.WriteLine("Save your profile picture in the following directory:");
        Console.WriteLine(Environment.CurrentDirectory);

        Console.Write("Enter the file name (e.g., profile.jpg): ");
        string? fileName = Console.ReadLine();

        if (fileName == null)
            return;

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
            database.UserDal.UpdateUser("ProfilePicture", imageData, userId);
            Console.WriteLine("Profile picture uploaded successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while uploading profile picture: {ex.Message}");
        }

    }
}


