using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    private const int DiceCount = 5; // Number of dice to roll
    private const int MaxReRolls = 2; // Maximum re-rolls allowed

    public List<int> playerDiceRoll = new List<int>();
    public List<int> secondDiceRoll = new List<int>();
    public List<int> aiDiceRoll = new List<int>();
    
    private int rerollCount = 0; // Track the number of re-rolls used

    private void Start()
    {
        RollAIDice(); // Roll AI's dice at the start
        RollDice(ref playerDiceRoll); // Initial player dice roll
    }

    // Roll the AI's dice once and keep it static
    public void RollAIDice()
    {
        aiDiceRoll.Clear();
        for (int i = 0; i < DiceCount; i++)
        {
            aiDiceRoll.Add(Random.Range(1, 7)); // Simulating a dice roll (1-6)
        }
    }

    // Roll the player's dice, either initially or as a re-roll
    public void RollDice(ref List<int> diceRoll)
    {
        diceRoll.Clear();
        for (int i = 0; i < DiceCount; i++)
        {
            diceRoll.Add(Random.Range(1, 7)); // Simulating a dice roll (1-6)
        }
    }

    public void ReRollPlayerDice()
    {
        if (rerollCount < MaxReRolls)
        {
            rerollCount++;
            RollDice(ref playerDiceRoll);
        }
        else
        {
            Debug.Log("No more re-rolls left.");
        }
    }

    public void RollSecondSetOfDice()
    {
        RollDice(ref secondDiceRoll); // Roll a second set of dice
    }

    public string CheckPokerCombinations()
    {
        List<int> allDice = new List<int>();
        allDice.AddRange(playerDiceRoll);
        allDice.AddRange(secondDiceRoll);
        return EvaluateCombinations(allDice);
    }

    private string EvaluateCombinations(List<int> diceRolls)
    {
        // Check for combinations and return result as a string
        if (HasFiveOfAKind(diceRolls))
        {
            return "Five of a kind!";
        }
        else if (HasFullHouse(diceRolls))
        {
            return "Full house!";
        }
        else if (HasTwoPairs(diceRolls))
        {
            return "Two pairs!";
        }
        else if (HasOnePair(diceRolls))
        {
            return "One pair!";
        }

        return "No special combination.";
    }

    private bool HasFiveOfAKind(List<int> dice)
    {
        return CheckForCount(dice, 5);
    }

    private bool HasFullHouse(List<int> dice)
    {
        int[] counts = CountDice(dice);
        return HasThreeOfAKind(counts) && HasPair(counts);
    }

    private bool HasTwoPairs(List<int> dice)
    {
        int[] counts = CountDice(dice);
        int pairCount = 0;
        foreach (var count in counts)
        {
            if (count == 2) pairCount++;
        }
        return pairCount == 2;
    }

    private bool HasOnePair(List<int> dice)
    {
        int[] counts = CountDice(dice);
        foreach (var count in counts)
        {
            if (count == 2) return true;
        }
        return false;
    }

    private int[] CountDice(List<int> dice)
    {
        int[] counts = new int[7]; // Counts for dice faces 1 to 6
        foreach (var die in dice)
        {
            counts[die]++;
        }
        return counts;
    }

    private bool HasThreeOfAKind(int[] counts)
    {
        foreach (var count in counts)
        {
            if (count == 3) return true;
        }
        return false;
    }

    private bool HasPair(int[] counts)
    {
        foreach (var count in counts)
        {
            if (count == 2) return true;
        }
        return false;
    }
}
