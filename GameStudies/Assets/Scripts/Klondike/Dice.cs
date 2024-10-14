using UnityEngine;

public class Die
{
    public string dieName;  // Each die has a unique name
    public int value;       // Value rolled

    public Die(string name)
    {
        dieName = name;
        Roll();
    }

    // Roll the die to get a random value between 1 and 6
    public void Roll()
    {
        value = Random.Range(1, 7); // Random number between 1 and 6
    }
}
