using System;
using System.IO;
using CsvHelper;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;

namespace FirstBankOfSuncoastColter
{
    class Program
    {
        class BankAccounts
        {
            public double CheckingAccountBalance { get; set; }

            public double SavingsAccountBalance { get; set; }

        }
        static string PromptForString(string prompt)
        {
            Console.Write(prompt);
            var userInput = Console.ReadLine();

            return userInput;
        }

        static double PromptForDouble(string prompt)
        {
            Console.Write(prompt);
            double userInput;
            var isThisGoodInput = double.TryParse(Console.ReadLine(), out userInput);

            if (isThisGoodInput)
            {
                return userInput;
            }
            else
            {
                Console.WriteLine("Sorry, that isn't a valid input. I'm using 0 as your answer. ");
                return 0;

            }
        }

        static void Main(string[] args)
        {
            var johnDoe = new BankAccounts
            {
                CheckingAccountBalance = 500,
                SavingsAccountBalance = 2000,
            };

            var recallBankAccountsRecord = new List<double>();

            if (File.Exists("bankAccountsRecordFile.csv"))
            {
                var reader = new StreamReader("bankAccountsRecordFile.csv");
                var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                csvReader.Configuration.HasHeaderRecord = false;
                recallBankAccountsRecord = csvReader.GetRecords<double>().ToList();
                johnDoe.CheckingAccountBalance = recallBankAccountsRecord[0];
                johnDoe.SavingsAccountBalance = recallBankAccountsRecord[1];
            }

            Console.WriteLine();
            Console.WriteLine("Welcome John Doe to your bank account management system at the First Bank of Suncoast. Please select one of the following options. ");
            Console.WriteLine();

            var userHasChosenToQuit = false;

            while (userHasChosenToQuit == false)
            {
                Console.WriteLine("Choose:");
                Console.WriteLine("(S)ummary of account balances");
                Console.WriteLine("(D)eposit money into your Checking Account");
                Console.WriteLine("(W)ithdraw money from your Checking Account");
                Console.WriteLine("D(e)posit money into your Savings Account");
                Console.WriteLine("W(i)thdraw money from your Saving Account");
                Console.WriteLine("(Q)uit the application");
                Console.WriteLine();

                var choice = PromptForString("Choice: ");
                if (choice == "Q")
                {
                    userHasChosenToQuit = true;
                }

                if (choice == "S")
                {
                    var findCheckingAccountBalance = johnDoe.CheckingAccountBalance;

                    var findSavingsAccountBalance = johnDoe.SavingsAccountBalance;

                    Console.WriteLine($"Checking Account Balance: ${findCheckingAccountBalance}");

                    Console.WriteLine($"Savings Account Balance: ${findSavingsAccountBalance}");

                }

                if (choice == "D")
                {
                    var findAccountBalance = johnDoe.CheckingAccountBalance;
                    var depositAmount = PromptForDouble($"Your current Checking Account balance is ${findAccountBalance}. How much money would you like to deposit? ");

                    if (depositAmount < 0)
                    {
                        Console.WriteLine($"Sorry, that's not a valid entry. ");
                    }
                    else
                    {
                        var newAccountBalance = findAccountBalance + depositAmount;

                        johnDoe.CheckingAccountBalance = newAccountBalance;

                        Console.WriteLine($"Here's your new Checking Account balance: ${newAccountBalance}. ");
                    }

                }

                if (choice == "W")
                {
                    var findAccountBalance = johnDoe.CheckingAccountBalance;
                    var withdrawlAmount = PromptForDouble($"Your current Checking Account balance is ${findAccountBalance}. How much money would you like to withdraw? ");

                    if (withdrawlAmount > findAccountBalance || withdrawlAmount < 0)

                    {
                        Console.WriteLine("Sorry, you either don't have enough funds to withdraw that amount or the entry is invalid. ");
                    }
                    else
                    {
                        var newAccountBalance = findAccountBalance - withdrawlAmount;
                        johnDoe.CheckingAccountBalance = newAccountBalance;
                        Console.WriteLine($"Here's you new Checking Account balance: ${newAccountBalance}. ");
                    }
                }

                if (choice == "e")
                {
                    var findAccountBalance = johnDoe.SavingsAccountBalance;
                    var depositAmount = PromptForDouble($"Your current Savings Account balance is ${findAccountBalance}. How much money would you like to deposit? ");

                    if (depositAmount < 0)
                    {
                        Console.WriteLine($"Sorry, that's not a valid entry. ");
                    }
                    else
                    {
                        var newAccountBalance = findAccountBalance + depositAmount;
                        johnDoe.SavingsAccountBalance = newAccountBalance;
                        Console.WriteLine($"Here's your new Savings Account balance: ${newAccountBalance}. ");
                    }
                }

                if (choice == "i")
                {
                    var findAccountBalance = johnDoe.SavingsAccountBalance;
                    var withdrawlAmount = PromptForDouble($"Your current Savings Account balance is ${findAccountBalance}. How much money would you like to withdraw? ");

                    if (withdrawlAmount > findAccountBalance || withdrawlAmount < 0)

                    {
                        Console.WriteLine("Sorry, you either don't have enough funds to withdraw that amount or the entry is invalid. ");
                    }

                    else
                    {
                        var newAccountBalance = findAccountBalance - withdrawlAmount;
                        johnDoe.SavingsAccountBalance = newAccountBalance;
                        Console.WriteLine($"Here's you new Savings Account balance: ${newAccountBalance}. ");
                    }

                }

                var saveBankAccountsRecord = new List<double>()
                {johnDoe.CheckingAccountBalance, johnDoe.SavingsAccountBalance};
                var fileWriter = new StreamWriter("bankAccountsRecordFile.csv");
                var csvWriter = new CsvWriter(fileWriter, CultureInfo.InvariantCulture);
                csvWriter.WriteRecords(saveBankAccountsRecord);
                fileWriter.Close();

            }
        }
    }
}
