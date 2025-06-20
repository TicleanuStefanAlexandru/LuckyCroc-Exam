using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if TMP_PRESENT
using TMPro;
#endif

public class SlotMachine3X3 : MonoBehaviour
{
    [Header("Slot Grid Settings")]
    public int rows = 3;
    public int cols = 3;
    public GameObject slotCellPrefab;     // Prefab with an Image
    public Transform slotGridParent;      // Parent with GridLayoutGroup
    public Sprite[] symbols;              // Assign symbol sprites here

    [Header("Spin Settings")]
    public int spinCost = 10;
    public Button spinButton;
    public Text resultText;               // Or TextMeshProUGUI if using TMP

    [Header("Money Display")]
    public Text moneyDisplayText;          // Live display of Dave's money

    [Header("Slot UI Controls")]
    public GameObject slotUIPanel;        // Reference to the Slot UI panel

    private Image[,] slotGrid;
    private int[,] currentSymbols;
    private DaveStats dave;

    private List<int[]> paylines = new List<int[]>
    {
        new int[] {0,1,2}, // Top row
        new int[] {3,4,5}, // Middle row
        new int[] {6,7,8}, // Bottom row
        new int[] {0,4,8}, // Diagonal \
        new int[] {2,4,6}  // Diagonal /
    };

    void Start()
    {
        dave = FindObjectOfType<DaveStats>();
        InitializeGrid();
        spinButton.onClick.AddListener(() => StartCoroutine(Spin()));
        UpdateMoneyDisplay();
    }

    void Update()
    {
        // Close the Slot UI if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseSlotUI();
        }
    }

    void InitializeGrid()
    {
        slotGrid = new Image[rows, cols];
        currentSymbols = new int[rows, cols];

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                GameObject cell = Instantiate(slotCellPrefab, slotGridParent);
                slotGrid[r, c] = cell.GetComponent<Image>();
            }
        }
    }

    IEnumerator Spin()
    {
        if (dave.money < spinCost)
        {
            resultText.text = "Not enough money!";
            yield break;
        }

        dave.SpendMoney(spinCost);
        UpdateMoneyDisplay();
        spinButton.interactable = false;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                int symbolIndex = Random.Range(0, symbols.Length);
                currentSymbols[r, c] = symbolIndex;
                slotGrid[r, c].sprite = symbols[symbolIndex];
                yield return new WaitForSeconds(0.05f);
            }
        }

        EvaluateResult();
        spinButton.interactable = true;
    }

    void EvaluateResult()
    {
        int winnings = 0;

        foreach (var line in paylines)
        {
            int a = line[0], b = line[1], c = line[2];
            int r1 = a / cols, c1 = a % cols;
            int r2 = b / cols, c2 = b % cols;
            int r3 = c / cols, c3 = c % cols;

            int sym1 = currentSymbols[r1, c1];
            int sym2 = currentSymbols[r2, c2];
            int sym3 = currentSymbols[r3, c3];

            if (sym1 == sym2 && sym2 == sym3)
            {
                winnings += 100; // Flat payout per line win
            }
        }

        if (winnings > 0)
        {
            dave.AddMoney(winnings);
            resultText.text = $"You won {winnings} money!";
        }
        else
        {
            resultText.text = "No win. Try again!";
        }

        UpdateMoneyDisplay();
    }

    void UpdateMoneyDisplay()
    {
        if (dave != null && moneyDisplayText != null)
        {
            moneyDisplayText.text = $"Money: {dave.money}";
        }
    }

    // Method to hide the Slot UI when called
    private void CloseSlotUI()
    {
        if (slotUIPanel != null)
        {
            slotUIPanel.SetActive(false);  // Hide the slot UI panel
        }
    }
}
