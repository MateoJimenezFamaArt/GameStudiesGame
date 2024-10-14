using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceGameUI : MonoBehaviour
{
    public Button rerollButton;
    public TextMeshProUGUI rerollText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI aiDiceText; // New UI Text to display AI's dice rolls

    private DiceManager diceManager;
    private int rerollCount = 0; // Track the number of re-rolls used
    private const int maxRerolls = 2; // Maximum re-rolls allowed
    private bool canReroll = true; // Flag to check if re-roll is possible

    private void Start()
    {
        diceManager = Object.FindObjectOfType<DiceManager>();
        diceManager.RollAIDice(); // Roll AI's dice at the start
        RollPlayerDice();
    }

    private void RollPlayerDice()
    {
        diceManager.RollDice(ref diceManager.playerDiceRoll); // Roll player's dice
        UpdateUI();
    }

    private void OnRerollButtonClicked()
    {
        if (canReroll && rerollCount < maxRerolls)
        {
            rerollCount++;
            diceManager.ReRollPlayerDice(); // Re-roll player's dice

            if (rerollCount >= maxRerolls)
            {
                canReroll = false; // Disable further re-rolls
                StartSecondRoll(); // Proceed to the second roll
            }

            UpdateUI();
        }
    }

    private void StartSecondRoll()
    {
        rerollButton.gameObject.SetActive(false); // Disable the reroll button
        diceManager.RollSecondSetOfDice(); // Roll the second set of dice
        
        // Display results of the second roll
        resultText.text += "\nSecond Roll: " + string.Join(", ", diceManager.secondDiceRoll);
        
        // Check combinations using DiceManager
        string result = diceManager.CheckPokerCombinations();
        resultText.text += "\nResult: " + result;
    }

    private void UpdateUI()
    {
        rerollText.text = "Re-rolls left: " + (maxRerolls - rerollCount);
        resultText.text = "First Roll: " + string.Join(", ", diceManager.playerDiceRoll); // Update with player's rolls
        aiDiceText.text = "AI Roll: " + string.Join(", ", diceManager.aiDiceRoll); // Update AI dice rolls
    }
}
