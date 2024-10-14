using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Método para generar un nivel procedural
    public ProceduralLevel GenerateLevel()
    {
        // Tiramos dados para cada parámetro
        EnvironmentType areaType = (EnvironmentType)Random.Range(0, 6);
        EnemyDifficulty enemyDifficulty = (EnemyDifficulty)Random.Range(0, 6);
        TrapCount traps = (TrapCount)Random.Range(0, 6);
        ChestCount chests = (ChestCount)Random.Range(0, 6);

        // Creamos un nuevo nivel basado en estos valores
        ProceduralLevel newLevel = new ProceduralLevel(areaType, enemyDifficulty, traps, chests);
        return newLevel;
    }

    // Llamamos este método cuando inicie el juego
    void Start()
    {
        ProceduralLevel level = GenerateLevel();
        Debug.Log("Nuevo nivel generado:");
        Debug.Log("Tipo de Entorno: " + level.AreaType);
        Debug.Log("Dificultad de Enemigos: " + level.Difficulty);
        Debug.Log("Número de Trampas: " + level.Traps);
        Debug.Log("Número de Cofres: " + level.Chests);
    }
}
