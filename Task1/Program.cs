
using System;
using System.Collections.Generic;
using DTO;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Extensions.Configuration;
using BusinessLogic;
using System.Data.Entity;

class Program
{
    private static void Main(string[] args)
    {
        Start();

    }

    private static void Start()
    {

        TradingCompany company = new TradingCompany("SqlServer");

        Console.WriteLine("Welcome to the system!");

        while (true)
        {
            Console.WriteLine("\nPlease select an option:");
            Console.WriteLine("1. Log in");
            Console.WriteLine("Q. Quit");

            string? choice = Console.ReadLine();
            if (choice == null)
                continue;

            if (choice.ToLower() == "q")
            {
                Console.WriteLine("Goodbye!");
                break;
            }

            switch (choice)
            {
                case "1":
                    LogIn(company);
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private static void LogIn(TradingCompany company)
    {
        Console.Write("Enter username: ");
        string? username = Console.ReadLine();
        
        Console.Write("Enter password: ");
        string? password = Console.ReadLine();

        if (username == null || password == null)
            return;

        User? user = company.LogIn(username, password);

        if (user == null)
        {
            PasswordRecover(company, username);
            return;
        }

        Console.WriteLine($"Welcome, {user.Data.Username}! Your role is {user.Role}.");

        if (user.Role == UserRole.Admin)
        {
            AdminMenu(company);
        }
        else
        {
            UserMenu(company);
        }
    }

    private static void PasswordRecover(TradingCompany company, string username)
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

        bool correctKey = company.CheckRecoveryKey(username, recoveryKey);
        if (!correctKey)
        {
            Console.WriteLine("Incorrect recovery key");
            return;
        }

        Console.Write("Enter a new password: ");
        string? newPassword = Console.ReadLine();
        if (newPassword == null)
            return;

        company.UpdatePassword(username, recoveryKey, newPassword);

        Console.WriteLine("Your password has been updated. Please log in again.");
    }


    static void AdminMenu(TradingCompany company)
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
                    List<User> users = company.GetAllUsers();
                    Console.WriteLine("\nUser List:");
                    foreach (var user in users)
                    {
                        Console.WriteLine($"ID: {user.Data.UserId}, Username: {user.Data.Username}, Role: {user.Role}");
                    }
                    break;

                case "2":
                    ViewProfile(company, company.LoggedInUser);
                    break;

                case "3":
                    UpdateProfile(company, company.LoggedInUser);
                    break;

                case "4":
                    ViewOtherUserProfile(company);
                    break;

                case "5":
                    EditOtherProfile(company);
                    break;

                case "6":
                    DeleteOtherProfile(company);
                    break; 
                case "7":

                    var userSessions = company.GetAllUserSessions();

                    Console.WriteLine("\nUser Sessions:");
                    foreach (var session in userSessions)
                    {
                        Console.WriteLine($"UserID: {session.UserId}, Status: {session.Status}, Login Time: {session.LoginTime}, Logout Time: {session.LogoutTime}");
                    }
                    break;

                case "8":
                    EndUserSession(company);
                    break;


                case "9":
                    company.EndUserSession(company.LoggedInUser);
                    Console.WriteLine("Logged out");
                    return;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private static void UserMenu(TradingCompany company)
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
                    ViewProfile(company, company.LoggedInUser);
                    break;
                case "2":
                    UpdateProfile(company, company.LoggedInUser);
                    break;
                case "3":
                    company.EndUserSession(company.LoggedInUser);
                    Console.WriteLine("Logged out");
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private static void ViewProfile(TradingCompany company, User? user)
    {
        if (user == null)
            return;

        Console.WriteLine("\nYour Profile:");
        Console.WriteLine($"Username: {user.Data.Username}");
        Console.WriteLine($"Email: {user.Data.Email}");
        Console.WriteLine($"Role: {user.Data.Role}");
        Console.WriteLine($"FirstName: {user.Data.FirstName}");
        Console.WriteLine($"LastName: {user.Data.LastName}");
        Console.WriteLine($"Gender: {user.Data.Gender}");
        Console.WriteLine($"PhoneNumber: {user.Data.PhoneNumber}");
        Console.WriteLine($"Address: {user.Data.Address}");

        if (user.Data.ProfilePicture != null && user.Data.ProfilePicture.Length > 0)
        {
            Console.WriteLine("Profile Picture: Uploaded");

            // Додати UserId до назви файлу
            string outputFileName = $"ProfilePicture_UserId_{user.Data.UserId}.jpg";
            string outputFilePath = Path.Combine(Environment.CurrentDirectory, outputFileName);

            // Зберегти файл
            File.WriteAllBytes(outputFilePath, user.Data.ProfilePicture);
            Console.WriteLine($"Profile picture exported to: {outputFilePath}");
        }
        else
        {
            Console.WriteLine("Profile Picture: Not uploaded");
        }


        // Отримати банківські дані
        BankDetailData? bankDetails = company.GetBankDetail(user);
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

    private static void ViewOtherUserProfile(TradingCompany company)
    {
        Console.Write("Enter the ID of the user to view their profile: ");
        if (!int.TryParse(Console.ReadLine(), out int detailedUserId))
        {
            Console.WriteLine("Invalid ID. Please enter a valid user ID.");
            return;
        }

        User? detailedUser = company.GetUser(detailedUserId);
        if (detailedUser == null)
        {
            Console.WriteLine("User not found.");
            return;
        }
    
        ViewProfile(company, detailedUser);
    }

    private static void EditOtherProfile(TradingCompany company)
    {
        Console.Write("Enter the ID of the user to edit their profile: ");
        if (!int.TryParse(Console.ReadLine(), out int userIdToEdit))
        {
            Console.WriteLine("Invalid ID. Please enter a valid user ID.");
            return;
        }

        User? userToEdit = company.GetUser(userIdToEdit);
        if (userToEdit != null)
        {
            Console.WriteLine($"Editing profile for user: {userToEdit.Data.Username}");
            UpdateProfile(company, userToEdit);
        }
        else
        {
            Console.WriteLine("User not found.");
        }
    }

    private static void DeleteOtherProfile(TradingCompany company)
    {
        Console.Write("Enter the ID of the user to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int userIdToDelete))
        {
            Console.WriteLine("Invalid ID. Please enter a valid user ID.");
            return;
        }

        User? userToDelete = company.GetUser(userIdToDelete);
        if (userToDelete == null)
        {
            Console.WriteLine("User not found.");
            return;
        }

        Console.Write($"Are you sure you want to delete the profile of {userToDelete.Data.Username}? (yes/no): ");
        string? confirmation = Console.ReadLine();

        if (confirmation == "yes")
        {
            company.DeleteUser(userToDelete);
            Console.WriteLine("User profile deleted successfully.");
        }
        else
        {
            Console.WriteLine("User deletion canceled.");
        }
        
    }

    private static void EndUserSession(TradingCompany company)
    {

        Console.Write("Enter the ID of the user whose session you want to end: ");
        if (!int.TryParse(Console.ReadLine(), out int userIdToEndSession))
        {
            Console.WriteLine("Invalid ID. Please enter a valid user ID.");
            return;
        }

        User? userToEndSession = company.GetUser(userIdToEndSession);
        if (userToEndSession == null)
        {
            Console.WriteLine("User not found.");
            return;
        }

        Console.Write($"Are you sure you want to end the session for {userToEndSession.Data.Username}? (yes/no): ");
        string? confirmation = Console.ReadLine();
        if (confirmation == "yes")
        {
            company.EndUserSession(userToEndSession);
            Console.WriteLine($"Session for {userToEndSession.Data.Username} has been ended.");
        }
        else
        {
            Console.WriteLine("Action canceled.");
        }
    }

    private static void UpdateProfile(TradingCompany company, User? user)
    {
        if (user == null)
            return;

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


            object? value;
            if (choice == UpdateProfileChoice.ProfilePicture)
            {
                value = ReadPicture();
                if (value == null)
                    return;

                company.UpdateUser(user, choice.ToString(), value);
                Console.WriteLine("Profile updated successfully.");
            }
            else if (choice == UpdateProfileChoice.BankDetails)
            {
                BankDetailData? data = ReadBankDetails(company, user);
                if (data == null)
                    return;

                company.UpdateBankDetail(data);
                Console.WriteLine("Bank details updated successfully.");
            }
            else
            {
                Console.Write("Enter new value: ");
                value = Console.ReadLine();
                if (value == null)
                    return;

                company.UpdateUser(user, choice.ToString(), value);
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

    private static BankDetailData? ReadBankDetails(TradingCompany company, User user)
    {
        Console.Write("Enter card number: ");
        string? cardNumber = Console.ReadLine();
        if (cardNumber == null)
            return null;

        if (!BankDetailData.IsValidCardNumber(cardNumber))
        {
            Console.WriteLine("Invalid card number. Please try again.");
            return null; // Повертаємося до меню, якщо номер картки недійсний
        }

        Console.Write("Enter expiration date (MM/YY): ");
        string? expirationDate = Console.ReadLine();
        if (expirationDate == null)
            return null;
        
        if (!BankDetailData.IsValidExpirationDate(expirationDate))
        {
            Console.WriteLine("Invalid or expired card. Please try again.");
            return null;
        }

        Console.Write("Enter CVV: ");
        string? cvv = Console.ReadLine();
        if (cvv == null)
            return null;

        if (!BankDetailData.IsValidCVV(cvv))
        {
            Console.WriteLine("Invalid CVV. Please try again.");
            return null;
        }

        Console.Write("Enter card holder name: ");
        string? cardHolderName = Console.ReadLine();
        if (cardHolderName == null)
            return null;

        Console.Write("Enter billing address: ");
        string? billingAddress = Console.ReadLine();
        if (billingAddress == null)
            return null;

        BankDetailData bankDetail = new BankDetailData
        {
            UserId = user.Data.UserId,
            CardNumber = cardNumber,
            ExpirationDate = expirationDate,
            CardCVV = cvv,
            CardHolderName = cardHolderName,
            BillingAddress = billingAddress
        };

        return bankDetail;
    }

    private static byte[]? ReadPicture()
    {
        Console.WriteLine("Save your profile picture in the following directory:");
        Console.WriteLine(Environment.CurrentDirectory);

        Console.Write("Enter the file name (e.g., profile.jpg): ");
        string? fileName = Console.ReadLine();

        if (fileName == null)
            return null;

        // Формуємо повний шлях
        string filePath = Path.Combine(Environment.CurrentDirectory, fileName);

        // Перевірка існування файлу
        if (!File.Exists(filePath))
        {
            Console.WriteLine("File not found in the current directory. Please try again.");
            return null;
        }

        try
        {
            // Прочитати файл у байтовий масив
            return File.ReadAllBytes(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while uploading profile picture: {ex.Message}");
        }

        return null;
    }
}


