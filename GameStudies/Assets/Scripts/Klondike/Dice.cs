using UnityEngine;

public class Dice
{
    public string DiceName { get; private set; }
    public int CurrentValue { get; private set; }

    public Dice(string diceName)
    {
        DiceName = diceName;
        Roll();
    }

    // Roll the dice (generating a value from 1 to 6)
    public void Roll()
    {
        CurrentValue = Random.Range(1, 7);
    }

    // For debugging in the Console
    public override string ToString()
    {
        return $"{DiceName}: {CurrentValue}";
    }

    
}
