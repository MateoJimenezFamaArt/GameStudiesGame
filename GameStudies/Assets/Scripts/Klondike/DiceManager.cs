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
    public Button skipButton; // Assign a Button for skipping rerolls
    public TextMeshProUGUI winnerText; // Assign a TextMeshProUGUI to display the current winner

    private List<Dice> playerDice;
    private List<Dice> bankDice;
    private List<Dice> hiddenDice; // List to hold hidden dice for tie resolution
    private string[] diceNames = { "Terrain Dice", "Enemy Dice", "Chest Dice", "Trap Dice", "Bonus Dice" };
    private bool isRerolling = false;
    private bool isGameOver = false;
    private int rerollCount = 0;
    private const int maxRerolls = 2; // Maximum rerolls allowed

    private void Start()
    {
        InitializeGame();
        RollBankDice();
        RollPlayerDice();
        UpdateRerollCountUI(); // Update the UI for rerolls
        continueButton.gameObject.SetActive(false); // Hide continue button at start
        winnerText.text = ""; // Initialize winner text
        skipButton.gameObject.SetActive(true); // Enable skip button at start
    }

    private void Update()
    {
        if (!isGameOver)
        {
            HandleRerollInput();
        }
    }

    // Attach this method to the skip button's OnClick event in the inspector
    public void OnSkipButtonPressed()
    {
        isGameOver = true;
        DetermineWinner();
        rerollButton.interactable = false; // Disable reroll button
        skipButton.gameObject.SetActive(false); // Hide skip button after skipping
        continueButton.gameObject.SetActive(true); // Show continue button
    }

    // Handle player input for reroll
    private void HandleRerollInput()
    {
        if (!isRerolling && rerollCount < maxRerolls)
        {
            isRerolling = true; // Lock this phase until player input is processed
            rerollButton.interactable = true; // Enable the reroll button
        }

        if (Input.GetKeyDown(KeyCode.Y) && rerollCount < maxRerolls)
        {
            RerollPlayerDice();
            isRerolling = false;
            rerollCount++;
            UpdateRerollCountUI(); // Update the UI for rerolls

            if (rerollCount >= maxRerolls)
            {
                Debug.Log("Maximum rerolls reached. No more rerolls allowed.");
                isGameOver = true;
                winnerText.text = ""; // Clear the winner text
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
        Debug.Log("The player rerolls its dice...");
        foreach (var dice in playerDice)
        {
            dice.Roll();
        }
        DisplayDiceResults(playerDice);
        UpdateDiceResultsUI(); // Update the UI with new results
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
        DisplayDiceResults(bankDice);
        UpdateDiceResultsUI(); // Update the UI with bank results
        DetermineWinner(); // Check for a winner after rolling bank dice
    }

    private void RollPlayerDice()
    {
        Debug.Log("The player rolls its dice...");
        foreach (var dice in playerDice)
        {
            dice.Roll();
        }
        DisplayDiceResults(playerDice);
        UpdateDiceResultsUI(); // Update the UI with player results
        DetermineWinner(); // Check for a winner after rolling player dice
    }

    // Display the result of the dice rolls in the console
    private void DisplayDiceResults(List<Dice> diceSet)
    {
        foreach (var dice in diceSet)
        {
            Debug.Log(dice.ToString());
        }
    }

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
    }

    // Update the number of remaining rerolls in the UI
    private void UpdateRerollCountUI()
    {
        rerollCountText.text = (maxRerolls - rerollCount).ToString();
        if (rerollCount >= maxRerolls)
        {
            rerollButton.interactable = false; // Disable the reroll button when out of rerolls
        }
    }

    // Determine the winner and highlight the dice
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
            SaveWinnersDiceCombination(playerDice);
        }
        else if (comparison < 0)
        {
            winnerText.text = "Enemies Wins!";
            HighlightWinningCombination(bankDice, true); // Highlight bank's winning dice
            HighlightWinningCombination(playerDice, false); // Reset player's dice color
            SaveWinnersDiceCombination(bankDice);
        }
        else
        {
            winnerText.text = "The tree shall decide what is to come..."; // Tie message
            HighlightWinningCombination(playerDice, false, true); // Highlight both player's and bank's dice in red
            HighlightWinningCombination(bankDice, false, true); // Highlight both player's and bank's dice in red
            RollHiddenDice(); // Roll hidden dice for tie resolution
        }

        Debug.Log("The 'Klondike' game has finished, now sending the winners data to the next scene");
        continueButton.gameObject.SetActive(true); // Show continue button after the game ends
    }

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
        SaveWinnersDiceCombination(hiddenDice);
    }

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

    // Save the winner's dice combination (name and value)
    private void SaveWinnersDiceCombination(List<Dice> winnersDice)
    {

        skipButton.gameObject.SetActive(false);

        if (diceData == null)
        {
            Debug.LogError("DiceData is not assigned in DiceManager!");
            return; // Exit the method if diceData is null
        }

        diceData.ClearWinnersDice(); // Clear previous data

        foreach (var dice in winnersDice)
        {
            diceData.AddWinnersDice(dice.DiceName, dice.CurrentValue); // Store each die's name and value
        }

        Debug.Log("Winners' dice combination saved in DiceData.");
    }

    // Method to get the best combination
    private string GetBestCombination(List<Dice> diceSet)
    {
        return ""; // Placeholder implementation
    }

    // Method to compare combinations
    private int CompareCombos(string combo1, string combo2)
    {
        return 0; // Placeholder implementation
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
                DetermineWinner();
            }
        }
    }

    // Attach this method to the continue button's OnClick event in the inspector
    public void OnContinueButtonPressed()
    {
        LoadNextScene();
    }
}
