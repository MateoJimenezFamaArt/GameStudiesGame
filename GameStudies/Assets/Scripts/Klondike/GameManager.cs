using UnityEngine;

public class GameManager : MonoBehaviour
{
    private DiceManager diceManager;

    private void Start()
    {
        diceManager = gameObject.AddComponent<DiceManager>();
    }
}
