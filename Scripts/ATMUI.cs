using UnityEngine;
using UnityEngine.UI;

public class ATMUI : MonoBehaviour
{
    public GameObject atmPanel;
    public Text balanceText;
    public InputField amountInput;
    public Button depositButton;
    public Button withdrawButton;

    private DaveStats dave;

    void Start()
    {
        dave = FindObjectOfType<DaveStats>();

        depositButton.onClick.AddListener(Deposit);
        withdrawButton.onClick.AddListener(Withdraw);
    }

    void Update()
    {
        if (atmPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseATM();
        }
    }

    void OnEnable()
    {
        UpdateBalanceDisplay();
    }

    public void OpenATM()
    {
        atmPanel.SetActive(true);
        UpdateBalanceDisplay();
    }

    public void CloseATM()
    {
        atmPanel.SetActive(false);
    }

    public void Deposit()
    {
        if (int.TryParse(amountInput.text, out int amount) && amount > 0)
        {
            dave.Deposit(amount);
            UpdateBalanceDisplay();
        }
    }

    public void Withdraw()
    {
        if (int.TryParse(amountInput.text, out int amount) && amount > 0)
        {
            dave.Withdraw(amount);
            UpdateBalanceDisplay();
        }
    }

    void Awake()
    {
        dave = FindObjectOfType<DaveStats>();
    }

    void UpdateBalanceDisplay()
    {
        if (dave == null)
        {
            Debug.LogWarning("DaveStats not found!");
            return;
        }

        balanceText.text = $"Bank Balance: ${dave.bankBalance}";
        amountInput.text = "";
    }
}
