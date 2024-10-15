using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiceData", menuName = "ScriptableObjects/DiceData", order = 1)]
public class DiceData : ScriptableObject
{
    public List<(string diceName, int diceValue)> winnersDiceCombination = new List<(string, int)>();

    // This method can be used to add a new winner's dice combination
    public void AddWinnersDice(string diceName, int diceValue)
    {
        winnersDiceCombination.Add((diceName, diceValue));
    }

    // Optionally, you can add a method to clear the saved combinations
    public void ClearWinnersDice()
    {
        winnersDiceCombination.Clear();
    }
}
