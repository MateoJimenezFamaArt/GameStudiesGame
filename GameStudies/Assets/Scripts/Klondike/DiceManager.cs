using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Add this for using Button

public class DiceManager : MonoBehaviour
{
    public DiceData diceData; // Assign in the inspector
    public TextMeshProUGUI[] diceResultTexts; // Assign 10 TextMeshProUGUI for dice results (5 bank + 5 player)
    public TextMeshProUGUI rerollCountText; // Assign a TextMeshProUGUI to show remaining reroll count
    public Button rerollButton; // Assign a Button for rerolling
    public Button continueButton; // Assign a Button for continuing to the next level
    public Button SkipButton; // Assign a Button for continuing to the next level
    public TextMeshProUGUI winnerText; // Assign a TextMeshProUGUI to display the current winner

    private List<Dice> playerDice;
    private List<Dice> bankDice;
    private List<Dice> hiddenDice; // List to hold hidden dice for tie resolution
    private string[] diceNames = { "Terrain Dice", "Enemy Dice", "Chest Dice", "Trap Dice", "Bonus Dice" };
    private bool isGameOver = false;
    private int rerollCount = 0;
    private const int maxRerolls = 2; // Maximum rerolls allowed

    private void Start()
    {
        ReleaseCursor();
        InitializeGame();
        RollBankDice();
        RollPlayerDice();
        UpdateRerollCountUI(); // Update the UI for rerolls
        continueButton.gameObject.SetActive(false); // Hide continue button at start
        winnerText.text = ""; // Initialize winner text
    }

    private void Update()
    {
        if (!isGameOver)
        {
            return;
        }
    }

    // Initialize the player's and bank's dice
    private void InitializeGame()
    {
        playerDice = new List<Dice>();
        bankDice = new List<Dice>();
        hiddenDice = new List<Dice>(); // Initialize the hidden dice list

        // Create 5 dice for both player and bank
        for (int i = 0; i < 5; i++)
        {
            playerDice.Add(new Dice(diceNames[i]));
            bankDice.Add(new Dice(diceNames[i]));
        }
    }

    private void RollBankDice()
    {
        Debug.Log("The bank rolls its dice...");
        foreach (var dice in bankDice)
        {
            dice.Roll();
        }
        //DisplayDiceResults(bankDice);
        UpdateDiceResultsUI(); // Update the UI with bank results
        DetermineWinner(); // Check for a winner after rolling bank dice NOT CHECL IT YET
    }

    private void RollPlayerDice()
    {
        Debug.Log("The player rolls its dice...");
        foreach (var dice in playerDice)
        {
            dice.Roll();
        }
        //DisplayDiceResults(playerDice);
        UpdateDiceResultsUI(); // Update the UI with player results
        DetermineWinner(); // Check for a winner after rolling player dice
    }

    /*
    // Display teh result in console
    private void DisplayDiceResults(List<Dice> diceSet)
    {
        foreach (var dice in diceSet)
        {
            Debug.Log(dice.ToString());
        }
    } */

    // Update the results of the dice in the assigned TextMeshPros
    private void UpdateDiceResultsUI()
    {
        // Show bank results first
        for (int i = 0; i < bankDice.Count; i++)
        {
            diceResultTexts[i].text = bankDice[i].CurrentValue.ToString();
        }

        // Show player results after
        for (int i = 0; i < playerDice.Count; i++)
        {
            diceResultTexts[i + bankDice.Count].text = playerDice[i].CurrentValue.ToString();
        }

        //HIGHILIGHT HERE?
    }

    // Reroll all of the player's dice
    private void RerollPlayerDice()
    {
        Debug.Log("The player rerolls its dice...");
        foreach (var dice in playerDice)
        {
            dice.Roll();
        }
        //DisplayDiceResults(playerDice);
        UpdateDiceResultsUI(); // Update the UI with new results
    }

    // Update the number of remaining rerolls in the UIrerollButton.interactable = false;
    private void UpdateRerollCountUI()
    {
        rerollCountText.text = (maxRerolls - rerollCount).ToString();
        if (rerollCount >= maxRerolls)
        {
             // Disable the reroll button when out of rerolls
        }
    }

    // Update the HighlightWinningCombination method
private void HighlightWinningCombination(List<Dice> diceSet, bool isWinner, bool isTie = false)
{
    Color highlightColor = isTie ? Color.red : (isWinner ? Color.yellow : Color.black); // Set color to red for ties

    for (int i = 0; i < diceSet.Count; i++)
    {
        // Determine the index in the UI TextMeshPro array
        int uiIndex = diceSet == playerDice ? i + bankDice.Count : i; // Adjust index based on player or bank dice
        diceResultTexts[uiIndex].color = highlightColor; // Change to highlight color based on winner or tie
    }
}

    // Modify the DetermineWinner method
private void DetermineWinner()
{
    string playerCombo = GetBestCombination(playerDice);
    string bankCombo = GetBestCombination(bankDice);

    Debug.Log($"Player Combo: {playerCombo}");
    Debug.Log($"Bank Combo: {bankCombo}");

    int comparison = CompareCombos(playerCombo, bankCombo);

    // Update the winner text based on who wins
    if (comparison > 0)
    {
        winnerText.text = "Player Wins!";
        HighlightWinningCombination(playerDice, true); // Highlight player's winning dice
        HighlightWinningCombination(bankDice, false); // Reset bank's dice color
        if (rerollCount <= 0)
        {
            Debug.Log("Se va a guardar la compo de los dados del jugador");
            SaveWinnersDiceCombination(playerDice);}
        
    }
    else if (comparison < 0)
    {
        winnerText.text = "Enemies Wins!";
        HighlightWinningCombination(bankDice, true); // Highlight bank's winning dice
        HighlightWinningCombination(playerDice, false); // Reset player's dice color
        if (rerollCount <= 0)
        {
            Debug.Log("Se va a guardar la compo de los dados del jugador");
            SaveWinnersDiceCombination(bankDice);}
    }
    else
    {
        winnerText.text = "The tree shall decide what is to come..."; // Tie message
        HighlightWinningCombination(playerDice, false, true); // Highlight both player's and bank's dice in red
        HighlightWinningCombination(bankDice, false, true); // Highlight both player's and bank's dice in red
        RollHiddenDice(); // Roll hidden dice for tie resolution
    }
    if (rerollCount <= 0)
    {
        Debug.Log("The 'Klondike' game has finished, now sending the winners data to the next scene");
        continueButton.gameObject.SetActive(true); // Show continue button after the game ends
    }


}

    // Roll the hidden dice in case of a tie
    private void RollHiddenDice()
    {
        Debug.Log("Rolling hidden dice to determine the winner...");
        hiddenDice.Clear(); // Clear previous hidden dice
        for (int i = 0; i < 5; i++)
        {
            hiddenDice.Add(new Dice(diceNames[i]));
            hiddenDice[i].Roll(); // Roll each hidden die
            Debug.Log($"Hidden Die {i + 1}: {hiddenDice[i].CurrentValue}");
        }
        // Implement logic for next level based on hiddenTotal if needed
        // You can also highlight the hidden dice results in the UI if desired
        SaveWinnersDiceCombination(hiddenDice);
    }


    // Save the winner's dice combination (name and value)
    private void SaveWinnersDiceCombination(List<Dice> winnersDice)
    {
        if (diceData == null)
        {
            Debug.LogError("DiceData is not assigned in DiceManager!");
            return; // Exit the method if diceData is null
        }

        diceData.ClearWinnersDice(); // Clear previous data

        foreach (var dice in winnersDice)
        {
            diceData.AddWinnersDice(dice.DiceName, dice.CurrentValue);
        }

        // Log the saved dice combination for debugging
        Debug.Log("Winner's Dice Combination Saved:");
        foreach (var dice in diceData.winnersDiceCombination)
        {
            Debug.Log($"Dice Name: {dice.diceName}, Dice Value: {dice.diceValue}");
        }
    }

    // Get the best combination of the dice
    private string GetBestCombination(List<Dice> diceSet)
    {
        var groups = diceSet.GroupBy(d => d.CurrentValue)
                            .Select(g => new { Value = g.Key, Count = g.Count() })
                            .OrderByDescending(g => g.Count)
                            .ThenByDescending(g => g.Value) // Sort by count and then by value
                            .ToList();

        if (groups[0].Count == 5) return "Five of a Kind";
        if (groups[0].Count == 4) return "Four of a Kind";
        if (groups[0].Count == 3 && groups[1].Count == 2) return "Full House";  // Three of a Kind + Pair
        if (groups[0].Count == 3) return "Three of a Kind";
        if (groups[0].Count == 2 && groups[1].Count == 2) return "Two Pair";
        if (groups[0].Count == 2) return "One Pair";
        return "High Card";
    }

    // Compare two combinations to determine which is better
    private int CompareCombos(string playerCombo, string bankCombo)
    {
        List<string> comboRank = new List<string>
        {
            "High Card", "One Pair", "Two Pair", "Three of a Kind", "Full House", "Four of a Kind", "Five of a Kind"
        };

        int playerRank = comboRank.IndexOf(playerCombo);
        int bankRank = comboRank.IndexOf(bankCombo);

        return playerRank.CompareTo(bankRank);
    }

    private void LoadNextScene()
    {
        // Get the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Calculate the next scene index
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next scene index is within valid range
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            // Optionally: Go back to the first scene
            nextSceneIndex = 0; // or simply do not load any scene
        }

        // Load the next scene
        SceneManager.LoadScene(nextSceneIndex);
    }

    // Attach this method to the reroll button's OnClick event in the inspector
    public void OnRerollButtonPressed()
    {
        if (rerollCount < maxRerolls)
        {
            RerollPlayerDice();
            rerollCount++;
            UpdateRerollCountUI(); // Update the UI for rerolls

            if (rerollCount >= maxRerolls)
            {
                Debug.Log("Maximum rerolls reached. No more rerolls allowed.");
                isGameOver = true;
                winnerText.text = ""; // Clear the winner text
                SkipButton.gameObject.SetActive(false);
                continueButton.gameObject.SetActive(true);
                DetermineWinner();
            }
        }
    }

    public void OnSkipRerollButtonPressed()
    {
        SkipButton.gameObject.SetActive(false); //Hide once clicked
        continueButton.gameObject.SetActive(true); // Hide continue button at start
        rerollCount = 0;
        rerollCountText.text = rerollCount.ToString();
        rerollButton.interactable = false;
        DetermineWinner();
    }

    // Attach this method to the continue button's OnClick event in the inspector
    public void OnContinueButtonPressed()
    {
        LoadNextScene();
    }

    private void ReleaseCursor()
{
    Cursor.lockState = CursorLockMode.None; // Unlocks the cursor
    Cursor.visible = true;                  // Makes the cursor visible
}

}
