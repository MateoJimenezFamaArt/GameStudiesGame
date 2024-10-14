using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralLevel
{
    public EnvironmentType AreaType;  // Tipo de entorno
    public EnemyDifficulty Difficulty; // Dificultad de enemigos
    public TrapCount Traps; // Número de trampas
    public ChestCount Chests; // Número de cofres

    // Constructor para inicializar un nivel procedural
    public ProceduralLevel(EnvironmentType areaType, EnemyDifficulty difficulty, TrapCount traps, ChestCount chests)
    {
        AreaType = areaType;
        Difficulty = difficulty;
        Traps = traps;
        Chests = chests;
    }

    
}
