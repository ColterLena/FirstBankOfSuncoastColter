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
        class Transaction
        {
            public int Amount { get; set; }

            public string Type { get; set; }

            public string Account { get; set; }

        }
        static string PromptForString(string prompt)
        {
            Console.Write(prompt);
            var userInput = Console.ReadLine();

            return userInput;
        }

        public static int PromptForInteger(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var userInput = 0;

                var isThisGoodInput = Int32.TryParse(Console.ReadLine(), out userInput);

                if (isThisGoodInput && userInput >= 0)
                {
                    return userInput;
                }
                else
                {
                    Console.WriteLine("Sorry, that isn't a valid input.");
                }
            }
        }

        static void Main(string[] args)
        {
            var streamReader = new StreamReader("transactions.csv");
            var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            var transactions = csvReader.GetRecords<Transaction>().ToList();

            Console.WriteLine();
            Console.WriteLine($"Welcome John Doe to your bank account management system at the First Bank of Suncoast. Please select one of the following options: ");
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
                    var checkingDepositTransactionAmountsTotal = transactions.Where(transaction => transaction.Account == "Checking" && transaction.Type == "Deposit").Sum(transaction => transaction.Amount);
                    var checkingWithdrawTransactionAmountsTotal = transactions.Where(transaction => transaction.Account == "Checking" && transaction.Type == "Withdraw").Sum(transaction => transaction.Amount);

                    var savingsDepositTransactionAmountsTotal = transactions.Where(transaction => transaction.Account == "Savings" && transaction.Type == "Deposit").Sum(transaction => transaction.Amount);
                    var savingsWithdrawTransactionAmountsTotal = transactions.Where(transaction => transaction.Account == "Savings" && transaction.Type == "Withdraw").Sum(transaction => transaction.Amount);

                    var checkingAccountBalance = checkingDepositTransactionAmountsTotal - checkingWithdrawTransactionAmountsTotal;
                    var savingsAccountBalance = savingsDepositTransactionAmountsTotal - savingsWithdrawTransactionAmountsTotal;

                    Console.WriteLine($"Checking Account Balance: ${checkingAccountBalance}");

                    Console.WriteLine($"Savings Account Balance: ${savingsAccountBalance}");
                }

                if (choice == "D")
                {
                    var amount = PromptForInteger("How much would you like to deposit into Checking? ");

                    if (amount < 0)
                    {
                        Console.WriteLine($"Sorry, that's not a valid entry. ");
                    }
                    else
                    {
                        var newTransaction = new Transaction
                        {
                            Type = "Deposit",
                            Account = "Checking",
                            Amount = amount
                        };

                        transactions.Add(newTransaction);
                    }
                }

                if (choice == "W")
                {
                    var amount = PromptForInteger("How much would you like to withdraw from Checking? ");

                    var checkingDepositTransactionAmountsTotal = transactions.Where(transaction => transaction.Account == "Checking" && transaction.Type == "Deposit").Sum(transaction => transaction.Amount);
                    var checkingWithdrawTransactionAmountsTotal = transactions.Where(transaction => transaction.Account == "Checking" && transaction.Type == "Withdraw").Sum(transaction => transaction.Amount);

                    var checkingAccountBalance = checkingDepositTransactionAmountsTotal - checkingWithdrawTransactionAmountsTotal;

                    if (amount > checkingAccountBalance || amount < 0)
                    {
                        Console.WriteLine("Sorry, you either don't have enough funds to withdraw that amount or the entry is invalid. ");
                    }
                    else
                    {
                        var newTransaction = new Transaction
                        {
                            Type = "Withdraw",
                            Account = "Checking",
                            Amount = amount
                        };

                        transactions.Add(newTransaction);
                    }
                }

                if (choice == "e")
                {
                    var amount = PromptForInteger("How much would you like to deposit into Savings? ");

                    if (amount < 0)
                    {
                        Console.WriteLine($"Sorry, that's not a valid entry. ");
                    }
                    else
                    {
                        var newTransaction = new Transaction
                        {
                            Type = "Deposit",
                            Account = "Savings",
                            Amount = amount
                        };

                        transactions.Add(newTransaction);
                    }
                }

                if (choice == "i")
                {
                    var amount = PromptForInteger("How much would you like to withdraw from Savings? ");

                    var savingsDepositTransactionAmountsTotal = transactions.Where(transaction => transaction.Account == "Savings" && transaction.Type == "Deposit").Sum(transaction => transaction.Amount);
                    var savingsWithdrawTransactionAmountsTotal = transactions.Where(transaction => transaction.Account == "Savings" && transaction.Type == "Withdraw").Sum(transaction => transaction.Amount);

                    var savingsAccountBalance = savingsDepositTransactionAmountsTotal - savingsWithdrawTransactionAmountsTotal;

                    if (amount > savingsAccountBalance || amount < 0)

                    {
                        Console.WriteLine("Sorry, you either don't have enough funds to withdraw that amount or the entry is invalid. ");
                    }
                    else
                    {
                        var newTransaction = new Transaction
                        {
                            Type = "Withdraw",
                            Account = "Savings",
                            Amount = amount
                        };

                        transactions.Add(newTransaction);
                    }
                }

                var streamWriter = new StreamWriter("transactions.csv");
                var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

                csvWriter.WriteRecords(transactions);

                streamWriter.Close();

            }
        }
    }
}