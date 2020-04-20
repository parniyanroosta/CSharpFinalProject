using FinalProjectCSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



// Parniyan Roosta, Student ID: 414950


namespace FinalProjectCSharp
{
    public abstract class BankSchema
    {
        public string Name;
        public string Address;
        public List<IBankAccount> BankAccounts;

        //public abstract string BankAccount(string name, long accountNumber, double accountBalance);
        //Abstract methods here
        //public abstract void ShowAccountInfo(long AccountNumber);
        //public abstract double GetAccount(double AccountNumber);
    }


    public class Bank : BankSchema
    {

        public Bank(string name, string address)
        {
            Name = name;
            Address = address;

            //initialize the list of all accounts available in the bank
            BankAccounts = new List<IBankAccount>();
            
        }

               
        // The deposit function, first finds the account by account number, then deposits the money into the account
        public void Deposit(long AccNumber, double amount)
        {
            IBankAccount bankAcc = null;
            for (int i = 0; i < this.BankAccounts.Count && bankAcc == null; i++)
            {
                if (this.BankAccounts[i].AccountNumber == AccNumber)
                {
                    bankAcc = this.BankAccounts[i];
                    bankAcc.AccountBalance = +amount;
                }
            }
        }


        // The withdraw function, first finds the account by account number, then withdraws the amount from the balance, if the balance is more than the amount, if not, it will show a message
        public double Withdraw(long AccNumber, double amount)
        {
            IBankAccount bankAcc = null;
            for (int i = 0; i < this.BankAccounts.Count && bankAcc == null; i++)
            {
                if (this.BankAccounts[i].AccountNumber == AccNumber)
                {
                    bankAcc = this.BankAccounts[i];
                    return bankAcc.Withdraw(amount);
                }
            }
            return 0;
        }


        
        // The function payBills, takes the account number, and if the bill amount is less than the balance, it will subtract the bill from the balance
        public void PayBill(long AccNumber, double amount)
        {
            IBankAccount bankAcc = null;
            for (int i = 0; i < this.BankAccounts.Count && bankAcc == null; i++)
            {
                if (this.BankAccounts[i].AccountNumber == AccNumber)
                {
                    bankAcc = this.BankAccounts[i];
                     bankAcc.PayBill(amount);
                }
            }           
        }


       
        //AddAccount ( both Saving and Chequing)      
        public void AddAccount(IBankAccount temp)
        {
            BankAccounts.Add(temp);
        }


        // Close account
        public void CloseAccount(long accountNumber)
        {
            IBankAccount bankAcc = null;
            for (int i = 0; i < BankAccounts.Count && bankAcc == null; i++)

            {
                if (BankAccounts[i].AccountNumber == accountNumber)
                    bankAcc = BankAccounts[i];
            }

            if (bankAcc != null)
            {
                Console.WriteLine(" The closing balance is : " + bankAcc.Withdraw(bankAcc.AccountBalance));
                BankAccounts.Remove(bankAcc);
            }

            else
                Console.WriteLine(" The account does not exist...");
        }

       
        // The transferFunds function first finds the account from which the user wants the money from,
        // then finds the account into which the amount should be transfered, and then asks for the amount
        // to be transfered
        public void TransferFunds(long accountFrom, long accountTo, double amount)
        {
            
            IBankAccount from = null, to = null;
            for (int i = 0; i < BankAccounts.Count && from == null; i++)
            {
                if (BankAccounts[i].AccountNumber == accountFrom)
                    from = BankAccounts[i];
            }
            for (int i = 0; i < BankAccounts.Count && to == null; i++)
            {
                if (BankAccounts[i].AccountNumber == accountTo)
                    to = BankAccounts[i];
            }
            if (from != null && to != null)
                to.Deposit(from.Withdraw(amount));
            else
                Console.WriteLine("one of the account does not exist.");
        }
    }
}

// This part is for sorting all the accounts ( both saving and chequing)  from higher balance to lower.
// for this part I used the code provided by the instructor in one of the class examples
// an interface to be inherited by both chaquing account class and saving account class
public interface IBankAccount : IComparable<IBankAccount>
{
    string Name { get; set; }
    long AccountNumber { get; set; }
    double AccountBalance { get; set; }
    void Deposit(double amount);
    double Withdraw(double amount);
    void PayBill(double amount);
    double ShowAccountBalance();

}
public class ChequingAccount : IBankAccount
{
    public int CompareTo(IBankAccount other)
    {
        // sorting the account holders based on the amount they have in their chequing account
        if (this.ShowAccountBalance() < other.ShowAccountBalance())
        {
            return -1;
        }
        else if (this.ShowAccountBalance() > other.ShowAccountBalance())
        {
            return 1;
        }
        else return 0;
    }

    public string Name { get; set; }
    public long AccountNumber { get; set; }
    public double AccountBalance { get; set; }       

    public byte AccountType;

    public ChequingAccount(string name, long accountNumber, double accountBalance)
    {
        Name = name;
        AccountNumber = accountNumber;
        AccountBalance = accountBalance;
    }

    // Deposit money into chequing account
    public void Deposit(double amount)
    {
        if (amount > 0)
            this.AccountBalance += amount;
    }

    // withdraw amount from chequing account
    public double Withdraw(double amount)
    {
        if (amount <= this.AccountBalance)
            return AccountBalance -= amount;
        else
            Console.WriteLine(" the balance on the account is not sufficient to do the transaction.");
        return 0;
    }

    //payBills  
    public void PayBill(double billamount)
    {
        // in the main, it should read the bill amount from the client
        if (billamount <= this.AccountBalance)
            this.AccountBalance -= billamount;
        else
            throw new Exception(" The balance on this account is not sufficient for the transaction to be completed...");
    }

    public override string ToString()
     {
           // String representation.
           return "Client : " + this.Name + " has : $" + this.AccountBalance + " in the chequing account, number :" + this.AccountNumber;
     }

    public double ShowAccountBalance()
    {
        return AccountBalance;
    }
 
}


public class SavingAccount :  IBankAccount
{
    public string Name { get; set; }
    public long AccountNumber { get; set; }
    public double AccountBalance { get; set; }
    public byte AccountType;

    public int CompareTo(IBankAccount other)
    {
        // sorting the account holders based on the amount they have in their chequing account
        if (this.ShowAccountBalance() < other.ShowAccountBalance())
        {
            return -1;
        }
        else if (this.ShowAccountBalance() > other.ShowAccountBalance())
        {
            return 1;
        }
        else return 0;
    }

    public SavingAccount(string name, long accountNumber, double accountBalance)
    {
        Name = name;
        AccountNumber = accountNumber;
        AccountBalance = accountBalance;
        AccountType = 1;            // Account type = 1 means saving account
    }


    // to deposit money into saving account
    public void Deposit(double amount)
    {
        if (amount > 0)
            this.AccountBalance += (amount + (0.2 * amount));    // any deposit to saving account comes with a 2 percent bonus.
    }

    
    // to withdraw money from saving account
    public double Withdraw(double amount)
    {
        if (amount <= this.AccountBalance)
            return AccountBalance -= amount;
        else
            Console.WriteLine(" the balance on the account is not sufficient to do the transaction.");
        return 0;
    }

    //payBills  
    public void PayBill(double billamount)
    {
        // in the main, it should read the bill amount from the client
        if (billamount <= this.AccountBalance)
            this.AccountBalance -= billamount;
        else
            throw new Exception(" the balance on this account is not sufficient for the transaction to be completed...");
    }


    public override string ToString()
    {
        // String representation.
        return "Client : " + this.Name + " has : $" + this.AccountBalance + " in the saving account, number :" + this.AccountNumber;
    }

    public double ShowAccountBalance()
    {
        return AccountBalance;
    }
}


class program
{
    static void Main(string[] args)
    {
        //CreateBank CalgaryBank 14 south
        //AddAccount pedram 152565 50
        //AddAccount parniyan 14 500
        //AddAccount pegah 1566 6590
        //transferFund 152565 14 25
        //closeAccount 152565
        //withdraw 1566 600
        //deposit 14 600
        //paybills 152565 
        Bank bank=new Bank("CalgaryBank","14 south");
        Console.WriteLine(" Choose from the menu options:");
        Console.WriteLine(" \nAddAccount \nTransferFunds \nCloseAccount \nWithdraw  \nDeposit  \nPayBills \nSort");
        Console.WriteLine("Enter your command :");
        string input;
        while ((input = Console.ReadLine()) != "exit")
        {
            var cmd = input.Split();
            if (cmd[0] == "AddAccount")
            {
                Console.Write(" How many accounts you want to open?");
                int count = int.Parse(Console.ReadLine());
                // 1th account, Saving or Checking ? saving pari 1566 60054
                // 2th account, Saving or Checking ? saving Pedram 152566 600
                for (int i = 0; i < count; i++)
                {
                    Console.Write((i + 1) + "th account, Saving or chequing? please enter the account holder name, account number and the account balance ");
                    string vv = Console.ReadLine();
                    var option = vv.Split();
                    if (option[0] == "Saving")
                        bank.AddAccount(new SavingAccount(option[1], long.Parse(option[2]), double.Parse(option[3])));
                    else if (option[0] == "Chequing")
                        bank.AddAccount(new ChequingAccount(option[1], long.Parse(option[2]), double.Parse(option[3])));
                }
            }
            else if (cmd[0] == "TransferFunds")
            {
                //long accountFrom, long accountTo, double amount
                bank.TransferFunds(long.Parse(cmd[1]), long.Parse(cmd[2]), double.Parse(cmd[3]));
                Console.WriteLine(" The transaction is done!");
            }

            else if (cmd[0] == "CloseAccount")
            {
                bank.CloseAccount(long.Parse(cmd[1]));
                Console.WriteLine(" The transaction is done!");
            }

            else if (cmd[0] == "Withdraw")
            {
                bank.Withdraw(long.Parse(cmd[1]), double.Parse(cmd[2]));
                Console.WriteLine(" The transaction is done!");
            }

            else if (cmd[0] == "Deposit")
            {
                bank.Deposit(long.Parse(cmd[1]), double.Parse(cmd[2]));
                Console.WriteLine(" The transaction is done!");
            }

            else if (cmd[0] == "PayBills")
            {
                bank.PayBill(long.Parse(cmd[1]), double.Parse(cmd[2]));
                Console.WriteLine(" The transaction is done!");
            }

            else if (cmd[0] == "Sort")
            {
                List<IBankAccount> BankAccounts = new List<IBankAccount>();

                BankAccounts.Add(new ChequingAccount( "Pari", 10205, 663330 ));
                BankAccounts.Add(new ChequingAccount( "Payam",  15205,  996640 ));
                BankAccounts.Add(new SavingAccount( "Pedram",  15005,  988830 ));
                BankAccounts.Add(new SavingAccount( "Pegah",  4305,  45330 ));
                BankAccounts.Add(new ChequingAccount( "Iman",  7005,  312330 ));
                BankAccounts.Add(new SavingAccount( "Hoda",  8705,  90900 ));
                BankAccounts.Add(new SavingAccount( "Shabnam", 185, 545430 ));
                BankAccounts.Add(new ChequingAccount( "Mori",  7805,  565330 ));

                BankAccounts.Sort();

                // Overrides ToString method of the object class.
                foreach (var element in BankAccounts)
                {
                    Console.WriteLine(element);
                }                                                                                                                
            }
        }
    }
}

