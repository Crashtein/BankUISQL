using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day3
{
    class Transaction
    {
        private int TransactionNumber;
        private double Amount;
        private DateTime Date;
        private string Notes;
        public Transaction(int t,double a,DateTime d, string n)
        {
            TransactionNumber = t;
            Amount = a;
            Date = d;
            Notes = n;
        }
        public int number()
        {
            return TransactionNumber;
        }
        public double amount()
        {
            return Amount;
        }
        public DateTime date()
        {
            return Date;
        }
        public string notes()
        {
            return Notes;
        }
    }
}
