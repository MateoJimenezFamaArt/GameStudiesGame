using UnityEngine;

public class EnemyElement : MonoBehaviour
{
    public ElementType enemyElement; // Element assigned to the enemy
    private Renderer enemyRenderer;
    public DiceData diceData; // Reference to the DiceData ScriptableObject

    private void Start()
    {
        enemyRenderer = GetComponent<Renderer>();
        AssignElementBasedOnMaterial();
        Debug.Log("Enemy Element: " + enemyElement);
    }

    private void AssignElementBasedOnMaterial()
    {
        // Get the material index based on Terrain Dice
        int materialIndex = GetMaterialIndexForTerrainDice();

        // Assign the element based on the material index
        enemyElement = (ElementType)materialIndex;

        // You can also set the enemy's material if necessary
        // enemyRenderer.material = enemyMaterials[materialIndex]; // Assuming you have a reference to enemy materials
    }

    private int GetMaterialIndexForTerrainDice()
    {
        // Assuming your DiceData holds a list of dice with their names and values
        foreach (var dice in diceData.winnersDiceCombination)
        {
            if (dice.diceName == "Terrain Dice")
            {
                // Map the dice value to the corresponding index (ensure it's between 0 and 6 for 7 elements)
                return Mathf.Clamp(dice.diceValue, 0, 6); 
            }
        }

        // Fallback to index 0 if Terrain Dice is not found or invalid value
        return 0;
    }
}
