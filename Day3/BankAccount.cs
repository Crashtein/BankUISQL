using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day3
{
    class BankAccount
    {
        readonly int UserId;
        readonly string Owner;
        List<Transaction> allTransactions;
        public BankAccount(string name, int userId)
        {
            Owner = name;
            UserId = userId;
            allTransactions = SQLMethods.GetTransactions(UserId);
        }
        public string Name()
        {
            return Owner;
        }
        public int Id()
        {
            return UserId;
        }
        public void CashIn()
        {
            Console.WriteLine("Podaj kwote jaką chcesz wpłacić[zł](np. 250,53): ");
            string sAmount = Console.ReadLine();
            if (Double.TryParse(sAmount, out double Amount))
            {
                if (Amount > 0)
                {
                    Console.WriteLine("Dodaj opis do transakcji: ");
                    string description = "Wpłata. " + Console.ReadLine();
                    SQLMethods.SqlTransaction(Amount, description, UserId);
                    Console.WriteLine("Wpłacono: " + String.Format("{0:N2}", Amount) + " zł");
                    Console.WriteLine("Opis: " + description);
                }
                else
                {
                    Console.WriteLine("Kwota musi być większa od 0!");
                }
            }
            else
            {
                Console.WriteLine("Błędnie podana kwota!");
            }
            Console.ReadKey();
        }
        public void CashOut()
        {
            Console.WriteLine("Podaj kwote jaką chcesz wypłacić[zł](np. 250,53): ");
            string sAmount = Console.ReadLine();
            if (Double.TryParse(sAmount, out double Amount))
            {
                double Balance = SQLMethods.GetBalance(UserId);
                if (Amount <= Balance)
                {
                    if (Amount > 0)
                    {
                        Console.WriteLine("Dodaj opis do transakcji: ");
                        string description = "Wypłata. " + Console.ReadLine();
                        SQLMethods.SqlTransaction(-Amount, description, UserId);
                        Console.WriteLine("Wypłacono: " + String.Format("{0:N2}", Amount) + " zł");
                        Console.WriteLine("Opis: " + description);
                    }
                    else
                    {
                        Console.WriteLine("Kwota musi być większa od 0!");
                    }
                }
                else
                {
                    Console.WriteLine("Niewystarczająco funduszy!");
                }
            }
            else
            {
                Console.WriteLine("Błędnie podana kwota!");
            }
            Console.ReadKey();
        }
        public void ShowBalance()
        {
            double Balance = SQLMethods.GetBalance(UserId);
            Console.WriteLine("Bilans Twojego konta wynosi: " + String.Format("{0:N2}", Balance) + " zł");
            if (Balance > 0)
                Console.WriteLine("Średniawka, ale może na bułki wystarczy ;)");
            Console.WriteLine("Wciśnij przycisk aby powrócić do menu");
            Console.ReadKey();
        }
        public void ShowTransactions()
        {
            allTransactions = SQLMethods.GetTransactions(UserId);
            foreach (Transaction tr in allTransactions)
            {
                Console.WriteLine("Transakcja numer: " + tr.number().ToString());
                Console.WriteLine("Kwota transakcji: " + String.Format("{0:N2}", tr.amount()) + " zł");
                Console.WriteLine("Data transakcji: " + tr.date());
                Console.WriteLine("Opis transakcji: " + tr.notes());
            }
            Console.ReadKey();
        }
        public void StartTransaction()
        {
            Console.WriteLine("Podaj Numer konta do przelewu: ");
            string stoUserId = Console.ReadLine();
            int toUserId;
            if (Int32.TryParse(stoUserId, out toUserId))
            {
                if (SQLMethods.CheckAccById(toUserId))
                {
                    Console.WriteLine("Podaj kwote jaką chcesz przelać[zł](np. 250,53): ");
                    string sAmount = Console.ReadLine();
                    if (Double.TryParse(sAmount, out double Amount))
                    {
                        double Balance = SQLMethods.GetBalance(UserId);
                        if (Amount <= Balance)
                        {
                            if (Amount > 0)
                            {
                                Console.WriteLine("Dodaj opis do transakcji: ");
                                string description = Console.ReadLine();
                                string fdescription = "Przelew wychodzący na konto numer: " + toUserId.ToString() + " Opis: " + description;
                                string tdescription = "Przelew przychodzący z konta numer: " + UserId.ToString() + " Opis: " + description;
                                SQLMethods.TransferCash(UserId, toUserId, Amount, fdescription, tdescription);
                                Console.WriteLine("Przelano: " + String.Format("{0:N2}", Amount) + " zł Na konto numer: " + toUserId.ToString());
                                Console.WriteLine("Opis: " + description);
                            }
                            else
                            {
                                Console.WriteLine("Kwota musi być większa od 0!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Niewystarczająco funduszy!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Błędnie podana kwota!");
                    }
                }
                else
                {
                    Console.WriteLine("Takie konto nie istnieje!");
                }
            }
            else
            {
                Console.WriteLine("Błędnie wpisany numer konta");
            }
            Console.ReadKey();
        }
        public void PrintTransaction()
        {
            Console.WriteLine("Podaj numer transakcji: ");
            string sTransactionNumber = Console.ReadLine();
            if (Int32.TryParse(sTransactionNumber, out int TransactionNumber) && allTransactions != null)
            {
                bool printed = false;
                foreach (Transaction tr in allTransactions)
                {
                    if (tr.number() == TransactionNumber)
                    {
                        Console.WriteLine("Drukuję potwierdzenie transakcji nr: " + TransactionNumber.ToString());
                        //Console.WriteLine("Transakcja numer: " + tr.number().ToString());
                        Console.WriteLine("Kwota transakcji: " + String.Format("{0:N2}", tr.amount()) + " zł");
                        Console.WriteLine("Data transakcji: " + tr.date());
                        Console.WriteLine("Opis transakcji: " + tr.notes());
                        string fileName = "PotwierdzenieNr" + TransactionNumber.ToString() + ".txt";
                        string currentPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\Potwierdzenia\\";
                        System.IO.Directory.CreateDirectory(currentPath);
                        System.IO.File.WriteAllText(currentPath + fileName, "Potwierdzenie transakcji z konta numer: " + UserId.ToString());
                        System.IO.File.AppendAllText(currentPath + fileName, "\r\n" + "Transakcja numer: " + tr.number().ToString() + "\r\n" + "Kwota: " + String.Format("{0:N2}", tr.amount()) + " zł" + "\r\n"
                            + "Data Transakcji: " + tr.date().ToString() + "\r\n" + tr.notes());
                        printed = true;
                    }
                }
                if (!printed)
                {
                    Console.WriteLine("Błędny numer transackji");
                }
            }
            else
            {
                Console.WriteLine("Błędnie wpisany numer transakcji");
            }
            Console.ReadKey();
        }
    }
}