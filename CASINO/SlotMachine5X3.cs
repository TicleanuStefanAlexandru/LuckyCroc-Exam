using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if TMP_PRESENT
using TMPro;
#endif

public class SlotMachine5X3 : MonoBehaviour
{
    [Header("Slot Grid Settings")]
    public int rows = 3;
    public int cols = 5;
    public GameObject slotCellPrefab;     // Prefab with an Image
    public Transform slotGridParent;      // Parent with GridLayoutGroup
    public Sprite[] symbols;              // Assign symbol sprites here

    [Header("Spin Settings")]
    public int spinCost = 100;
    public Button spinButton;
    public Text resultText;               // Or TextMeshProUGUI if using TMP

    [Header("Money Display")]
    public Text moneyDisplayText;          // Live display of Dave's money

    [Header("Slot UI Controls")]
    public GameObject slotUIPanel;        // Reference to the Slot UI panel

    private Image[,] slotGrid;
    private int[,] currentSymbols;
    private DaveStats dave;

    private int losingStreak = 0;  // Counts consecutive losses

    // 5 paylines (flattened 3x5 grid)
    private List<int[]> paylines = new List<int[]>
    {
        new int[] {0,1,2,3,4},       // Top row
        new int[] {5,6,7,8,9},       // Middle row
        new int[] {10,11,12,13,14},  // Bottom row
        new int[] {0,6,7,8,14},      // V shape
        new int[] {10,8,7,6,4}       // Inverted V shape
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

        bool forcedWin = (losingStreak >= 7);
        if (forcedWin)
            losingStreak = 0;  // Reset losing streak as we force a win now

        if (forcedWin)
        {
            // Generate a forced winning spin
            GenerateForcedWinningSpin();
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    int symbolIndex = Random.Range(0, symbols.Length);
                    currentSymbols[r, c] = symbolIndex;
                    slotGrid[r, c].sprite = symbols[symbolIndex];
                    yield return new WaitForSeconds(0.02f);
                }
            }
        }

        bool win = CheckIfAnyWin();

        if (win)
        {
            losingStreak = 0;  // Reset losing streak on win
        }
        else
        {
            losingStreak++;
        }

        EvaluateResult();
        spinButton.interactable = true;
    }

    void GenerateForcedWinningSpin()
    {
        int[] line = paylines[Random.Range(0, paylines.Count)];
        int winningSymbol = Random.Range(0, symbols.Length);

        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
            {
                int randomSymbol = Random.Range(0, symbols.Length);
                currentSymbols[r, c] = randomSymbol;
                slotGrid[r, c].sprite = symbols[randomSymbol];
            }

        foreach (int index in line)
        {
            int r = index / cols;
            int c = index % cols;
            currentSymbols[r, c] = winningSymbol;
            slotGrid[r, c].sprite = symbols[winningSymbol];
        }
    }

    bool CheckIfAnyWin()
    {
        foreach (var line in paylines)
        {
            int firstIndex = line[0];
            int r0 = firstIndex / cols;
            int c0 = firstIndex % cols;
            int firstSymbol = currentSymbols[r0, c0];

            int matchCount = 1;

            for (int i = 1; i < line.Length; i++)
            {
                int r = line[i] / cols;
                int c = line[i] % cols;

                if (currentSymbols[r, c] == firstSymbol)
                    matchCount++;
                else
                    break;
            }

            if (matchCount >= 3)
                return true;
        }
        return false;
    }

    void EvaluateResult()
    {
        int winnings = 0;

        foreach (var line in paylines)
        {
            int firstIndex = line[0];
            int r0 = firstIndex / cols;
            int c0 = firstIndex % cols;
            int firstSymbol = currentSymbols[r0, c0];

            int matchCount = 1;

            for (int i = 1; i < line.Length; i++)
            {
                int r = line[i] / cols;
                int c = line[i] % cols;

                if (currentSymbols[r, c] == firstSymbol)
                    matchCount++;
                else
                    break;
            }

            if (matchCount >= 3)
            {
                int lineWin = 0;
                switch (matchCount)
                {
                    case 3:
                        lineWin = 300;   // Changed payout for 3 matches
                        break;
                    case 4:
                        lineWin = 700;   // Changed payout for 4 matches
                        break;
                    case 5:
                        lineWin = 1000;  // Changed payout for 5 matches
                        break;
                }
                winnings += lineWin;
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

    private void CloseSlotUI()
    {
        if (slotUIPanel != null)
        {
            slotUIPanel.SetActive(false);
        }
    }
}
