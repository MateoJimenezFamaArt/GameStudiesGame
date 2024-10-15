using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DiceData))]
public class DiceDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draw the default inspector UI

        DiceData diceData = (DiceData)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Winners Dice Combination", EditorStyles.boldLabel);

        // Display the saved dice combinations
        for (int i = 0; i < diceData.winnersDiceCombination.Count; i++)
        {
            var dice = diceData.winnersDiceCombination[i];
            EditorGUILayout.LabelField($"Dice Name: {dice.diceName}, Dice Value: {dice.diceValue}");
        }

        // Allow the user to clear the saved dice combinations
        if (GUILayout.Button("Clear Winners Dice"))
        {
            diceData.ClearWinnersDice();
        }
    }
}
