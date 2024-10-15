using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class DiceManager : MonoBehaviour
{
    public DiceData diceData; // Assign this in the inspector

    private List<Dice> playerDice;
    private List<Dice> bankDice;
    private string[] diceNames = { "Terrain Dice", "Enemy Dice", "Chest Dice", "Trap Dice", "Bonus Dice" };
    private bool isRerolling = false;
    private bool isGameOver = false;
    private int rerollCount = 0;
    private const int maxRerolls = 2; // Maximum rerolls allowed

    // List to store winner's dice combination (name and value)
    private List<(string diceName, int diceValue)> winnersDiceCombination;

    private void Start()
    {
        InitializeGame();
        RollBankDice();
        RollPlayerDice();
    }

    private void Update()
    {
        if (!isGameOver)
        {
            HandleRerollInput();
        }
    }

    // Initialize both player and bank's dice
    private void InitializeGame()
    {
        playerDice = new List<Dice>();
        bankDice = new List<Dice>();
        winnersDiceCombination = new List<(string, int)>(); // Initialize the list

        // Create 5 dice for both player and bank
        for (int i = 0; i < 5; i++)
        {
            playerDice.Add(new Dice(diceNames[i]));
            bankDice.Add(new Dice(diceNames[i]));
        }

        //diceData.ClearWinnersDice();

    }

    private void RollBankDice()
    {
        Debug.Log("Bank rolls its dice...");
        foreach (var dice in bankDice)
        {
            dice.Roll();
        }
        DisplayDiceResults(bankDice);
    }

    private void RollPlayerDice()
    {
        Debug.Log("Player rolls its dice...");
        foreach (var dice in playerDice)
        {
            dice.Roll();
        }
        DisplayDiceResults(playerDice);
    }

    // Display the result of the dice rolls in the console
    private void DisplayDiceResults(List<Dice> diceSet)
    {
        foreach (var dice in diceSet)
        {
            Debug.Log(dice.ToString());
        }
    }

    // Handle player input for reroll
    private void HandleRerollInput()
    {
        if (!isRerolling && rerollCount < maxRerolls)
        {
            Debug.Log("Do you want to reroll? Press 'Y' to reroll or 'N' to stop.");
            isRerolling = true; // Lock this phase until player input is processed
        }

        if (Input.GetKeyDown(KeyCode.Y) && rerollCount < maxRerolls)
        {
            RerollPlayerDice();
            isRerolling = false;
            rerollCount++;

            if (rerollCount >= maxRerolls)
            {
                Debug.Log("Maximum rerolls reached. No more rerolls allowed.");
                isGameOver = true;
                DetermineWinner();
            }
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            isRerolling = false;
            isGameOver = true;
            DetermineWinner();
        }
    }

    // Reroll all of the player's dice
    private void RerollPlayerDice()
    {
        Debug.Log("Player rerolls its dice...");
        foreach (var dice in playerDice)
        {
            dice.Roll();
        }
        DisplayDiceResults(playerDice);
    }

    // Determine the winner based on dice combinations
    private void DetermineWinner()
    {
        string playerCombo = GetBestCombination(playerDice);
        string bankCombo = GetBestCombination(bankDice);

        Debug.Log($"Player Combo: {playerCombo}");
        Debug.Log($"Bank Combo: {bankCombo}");

        int comparison = CompareCombos(playerCombo, bankCombo);

        if (comparison > 0)
        {
            Debug.Log("Player wins!");
            SaveWinnersDiceCombination(playerDice); // Save player's dice
        }
        else if (comparison < 0)
        {
            Debug.Log("Bank wins!");
            SaveWinnersDiceCombination(bankDice); // Save bank's dice
        }
        else
        {
            Debug.Log("It's a tie! No dice combination will be saved.");
        }

        Debug.Log("The 'Klondike' game has finished, now sending the winners Data to the next Scene");
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
    Debug.Log("Changing Scene to world Building...");
    LoadNextScene();


}


    // Calculate best combination from dice
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
        if (groups[0].Count == 2 && groups[1].Count == 2) return "Two Pairs";
        if (groups[0].Count == 2) return "Pair";
        return "High Card";
    }

    // Compare two combinations to determine which is better
    private int CompareCombos(string playerCombo, string bankCombo)
    {
        List<string> comboRank = new List<string>
        {
            "High Card", "Pair", "Two Pairs", "Three of a Kind", "Full House", "Four of a Kind", "Five of a Kind"
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

    // Check if the next scene index is within the valid range
    if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
    {
        // Optional: Loop back to the first scene
        nextSceneIndex = 0; // or you can just return to not load any scene
    }

    // Load the next scene
    SceneManager.LoadScene(nextSceneIndex);
}



}
