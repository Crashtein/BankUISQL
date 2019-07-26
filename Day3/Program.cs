using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.Common;

namespace Day3
{
    class Program
    {
        static string input;
        static BankAccount acc = null;
        static string userName;
        static string password;
        static int userId;
        static void Main(string[] args)
        {
            while (acc == null)
            {
                input = null;
                while (input != "1" && input != "2")
                {
                    Console.Clear();
                    Console.WriteLine("Witaj w aplikacji bankowej! Wybierz co chcesz zrobić: ");
                    Console.WriteLine("1) Stwórz konto bankowe.");
                    Console.WriteLine("2) Zaloguj się na swoje konto.");
                    input = Console.ReadLine();
                }
                switch (input)
                {
                    case "1":
                        SignIn();
                        break;
                    case "2":
                        LogIn();
                        break;
                    default:
                        //never
                        break;
                }
            }
            AccMenu();
        }
        static void SignIn()
        {
            Console.Clear();
            Console.WriteLine("Stwórz konto bankowe!");
            Console.WriteLine("Podaj nazwe użytkownika: ");
            userName = Console.ReadLine();
            if (!SQLMethods.CheckAcc(userName))
            {
                Console.WriteLine("Podaj hasło: ");
                password = Console.ReadLine();
                SQLMethods.NewAcc(userName, password);
                userId = SQLMethods.GetUserId(userName);
                if (userId != -1)
                {
                    acc = new BankAccount(userName, userId);
                }
                else
                {
                    Console.WriteLine("Wystąpił błąd podczas zakładania konta!");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Takie konto już istnieje!");
                Console.ReadKey();
            }
        }
        static void LogIn()
        {
            Console.WriteLine("Podaj nazwe użytkownika: ");
            userName = Console.ReadLine();
            Console.WriteLine("Podaj hasło: ");
            password = Console.ReadLine();
            if (SQLMethods.LogIn(userName, password))
            {
                Console.WriteLine("Logowanie udane!");
                userId = SQLMethods.GetUserId(userName);
                if (userId != -1)
                {
                    acc = new BankAccount(userName, userId);
                }
                else
                {
                    Console.WriteLine("Nie znaleziono konta!");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Nie znaleziono konta!");
                Console.ReadKey();
            }
        }
        static void AccMenu()
        {
            input = null;
            while (input != "0")
            {
                input = null;
                while (input != "1" && input != "2" && input != "3" && input != "4" && input != "5" && input!="6" && input != "0")
                {
                    Console.Clear();
                    Console.WriteLine("Witaj na swoim koncie! " + acc.Name() + "Numer Twojego konta to: " + acc.Id().ToString());
                    Console.WriteLine("Co chcesz zrobić?");
                    Console.WriteLine("Menu: ");
                    Console.WriteLine("-------------------------------------");
                    Console.WriteLine("1) Wpłać pieniądze");
                    Console.WriteLine("2) Wypłać pieniądze");
                    Console.WriteLine("3) Pokaż stan konta");
                    Console.WriteLine("4) Wyświetl listę dokonanych operacji");
                    Console.WriteLine("5) Przelew na inne konto");
                    Console.WriteLine("6) Wygeneruj plik z potwierdzeniem transakcji");
                    Console.WriteLine("0) Wyjdź");
                    Console.WriteLine("-------------------------------------");
                    input = Console.ReadLine();
                }
                switch (input)
                {
                    case "1":
                        acc.CashIn();
                        break;
                    case "2":
                        acc.CashOut();
                        break;
                    case "3":
                        acc.ShowBalance();
                        break;
                    case "4":
                        acc.ShowTransactions();
                        break;
                    case "5":
                        acc.StartTransaction();
                        break;
                    case "6":
                        acc.PrintTransaction();
                        break;
                    case "0":
                        Console.WriteLine("Dziękujemy za korzystanie z naszych usług!");
                        Console.WriteLine("Wciśnij klawisz aby zamknąć aplikacje:");
                        Console.ReadKey();
                        Environment.Exit(1);
                        break;
                    default:
                        //never happens
                        break;
                }
            }
        }
    }
}
