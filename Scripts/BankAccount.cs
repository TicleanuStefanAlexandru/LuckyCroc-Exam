using UnityEngine;

public class BankAccount : MonoBehaviour
{
    public int balance = 0;

    public bool Deposit(int amount)
    {
        if (amount <= 0) return false;

        balance += amount;
        Debug.Log($"Deposited {amount}. New balance: {balance}");
        return true;
    }

    public bool Withdraw(int amount)
    {
        if (amount <= 0 || amount > balance)
        {
            Debug.Log("Withdrawal failed: insufficient funds.");
            return false;
        }

        balance -= amount;
        Debug.Log($"Withdrew {amount}. Remaining balance: {balance}");
        return true;
    }

    public int GetBalance()
    {
        return balance;
    }
}
