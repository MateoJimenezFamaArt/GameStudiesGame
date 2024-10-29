using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    public int dungeonWidth, dungeonLength;
    public int roomWidthMin, roomLengthMin;
    public int maxIterations;
    public int corridorWidth;
    public GameObject playerPrefab; // Reference to the player prefab
    public GameObject enemyPrefab;  // Reference to the enemy prefab
    public float wallHeight = 3.0f; // Height of the walls
    [Range(0.0f, 0.3f)]
    public float roomBottomCornerModifier;
    [Range(0.7f, 1.0f)]
    public float roomTopCornerMidifier;
    [Range(0, 2)]
    public int roomOffset;
    public GameObject wallVertical, wallHorizontal;
    // New Variables
    public DiceData diceData; // Reference to the DiceData ScriptableObject
    public Material[] floorMaterials; // Array of materials for floors (7 elements)
    public Material[] wallMaterials;  // Array of materials for walls (7 elements)
    public Material[] enemyMaterials; // Array of materials for enemies (7 elements)
    public GameObject[] chestPrefabs;
    public GameObject[] trapsPrefabs;

    List<Vector3Int> possibleDoorVerticalPosition;
    List<Vector3Int> possibleDoorHorizontalPosition;
    List<Vector3Int> possibleWallHorizontalPosition;
    List<Vector3Int> possibleWallVerticalPosition;

    void Start()
    {
        CreateLevel();
    }

    public void CreateLevel()
{
    DestroyAllChildren();
    LevelGenerator generator = new LevelGenerator(dungeonWidth, dungeonLength);
    var listOfRooms = generator.CalculateDungeon(maxIterations,
        roomWidthMin,
        roomLengthMin,
        roomBottomCornerModifier,
        roomTopCornerMidifier,
        roomOffset,
        corridorWidth);

    GameObject wallParent = new GameObject("WallParent");
    wallParent.transform.parent = transform;

    possibleDoorVerticalPosition = new List<Vector3Int>();
    possibleDoorHorizontalPosition = new List<Vector3Int>();
    possibleWallHorizontalPosition = new List<Vector3Int>();
    possibleWallVerticalPosition = new List<Vector3Int>();

    // Spawn the player in a valid room
    SpawnPlayerInRoom(listOfRooms);

    // Get the synchronized material index based on Terrain Dice
    int materialIndex = GetMaterialIndexForTerrainDice();

    // Get the materials for floors, walls, and enemies
    Material floorMaterial = floorMaterials[materialIndex];
    Material wallMaterial = wallMaterials[materialIndex];
    Material enemyMaterial = enemyMaterials[materialIndex]; // You can use this material for your enemies later

    // Create the rooms and walls
    for (int i = 0; i < listOfRooms.Count; i++)
    {
        CreateMesh(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner, floorMaterial);
        SpawnEnemiesInRoom(listOfRooms[i], materialIndex);  // Spawn enemies in each room
        SpawnChestsInRoom(listOfRooms[i]);
        SpawnTrapsInRoom(listOfRooms[i]);
    }

    CreateWalls(wallParent, wallMaterial);
}

private void SpawnPlayerInRoom(List<Node> rooms)
{
    // Randomly select a room from the list
    Node selectedRoom = rooms[UnityEngine.Random.Range(0, rooms.Count)];

    // Calculate a spawn position within the selected room
    Vector3 playerSpawnPosition = new Vector3(
        UnityEngine.Random.Range(selectedRoom.BottomLeftAreaCorner.x, selectedRoom.TopRightAreaCorner.x), 
        1.0f,  // Adjusted spawn position to be above the ground
        UnityEngine.Random.Range(selectedRoom.BottomLeftAreaCorner.y, selectedRoom.TopRightAreaCorner.y)
    );

    // Ensure the spawn position is within the room bounds
    if (IsPositionInRoom(selectedRoom, playerSpawnPosition))
    {
        Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity);
    }
    else
    {
        Debug.LogError("Spawn position is not within the room bounds!");
    }
}

    private void CreateWalls(GameObject wallParent, Material wallMaterial)
    {
        foreach (var wallPosition in possibleWallHorizontalPosition)
        {
            CreateWall(wallParent, wallPosition, wallHorizontal, wallMaterial);
        }
        foreach (var wallPosition in possibleWallVerticalPosition)
        {
            CreateWall(wallParent, wallPosition, wallVertical, wallMaterial);
        }
    }

    private void CreateWall(GameObject wallParent, Vector3Int wallPosition, GameObject wallPrefab, Material wallMaterial)
    {
        GameObject wall = Instantiate(wallPrefab, wallPosition + new Vector3(0, wallHeight / 2, 0), Quaternion.identity, wallParent.transform);
        wall.transform.localScale = new Vector3(1, wallHeight, 1); // Scale the wall to the desired height
        wall.GetComponent<MeshRenderer>().material = wallMaterial; // Set the wall material
    }

    private void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner, Material floorMaterial)
    {
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0, bottomLeftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, 0, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, 0, topRightCorner.y);

        Vector3[] vertices = new Vector3[] { topLeftV, topRightV, bottomLeftV, bottomRightV };
        Vector2[] uvs = new Vector2[vertices.Length];

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[] { 0, 1, 2, 2, 1, 3 };
        Mesh mesh = new Mesh { vertices = vertices, uv = uvs, triangles = triangles };

        GameObject dungeonFloor = new GameObject("Mesh" + bottomLeftCorner, typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = floorMaterial; // Set the floor material
        dungeonFloor.transform.parent = transform;

        // Assign the mesh to the MeshCollider for more precision
        MeshCollider meshCollider = dungeonFloor.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;  // Use the generated mesh as the collider
        meshCollider.convex = false;     // Make sure it's not set to convex for better precision in large flat surfaces

        for (int row = (int)bottomLeftV.x; row < (int)bottomRightV.x; row++)
        {
            var wallPosition = new Vector3(row, 0, bottomLeftV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }

        for (int row = (int)topLeftV.x; row < (int)topRightCorner.x; row++)
        {
            var wallPosition = new Vector3(row, 0, topRightV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }

        for (int col = (int)bottomLeftV.z; col < (int)topLeftV.z; col++)
        {
            var wallPosition = new Vector3(bottomLeftV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }

        for (int col = (int)bottomRightV.z; col < (int)topRightV.z; col++)
        {
            var wallPosition = new Vector3(bottomRightV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
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

    private int GetEnemyCountForEnemyDice()
    {
        // Assuming your DiceData holds a list of dice with their names and values
        foreach (var dice in diceData.winnersDiceCombination)
        {
            if (dice.diceName == "Enemy Dice")
            {
                // Return the value from the Enemy Dice (you can clamp it to a max value if needed)
                return Mathf.Max(0, dice.diceValue); 
            }
        }

        // Fallback to 0 enemies if Enemy Dice is not found
        return 0;
    }

    private int GetChestCountForChestDice()
    {
        // Assuming your DiceData holds a list of dice with their names and values
        foreach (var dice in diceData.winnersDiceCombination)
        {
            if (dice.diceName == "Chest Dice")
            {
                // Return the value from the Chest Dice (you can clamp it to a max value if needed)
                return Mathf.Max(0, dice.diceValue); 
            }
        }

        // Fallback to 0 chests if Chest Dice is not found
        return 0;
    }

        private int GetTrapCountForTrapDice()
    {
        // Assuming your DiceData holds a list of dice with their names and values
        foreach (var dice in diceData.winnersDiceCombination)
        {
            if (dice.diceName == "Trap Dice")
            {
                // Return the value from the Chest Dice (you can clamp it to a max value if needed)
                return Mathf.Max(0, dice.diceValue); 
            }
        }

        // Fallback to 0 chests if Chest Dice is not found
        return 0;
    }

    private void SpawnEnemiesInRoom(Node room, int materialIndex)
    {
        int enemyCount = GetEnemyCountForEnemyDice();  // Get number of enemies from Enemy Dice

        // Check if the room is a full-sized room (larger than corridor width)
        if ((room.TopRightAreaCorner.x - room.BottomLeftAreaCorner.x > corridorWidth) && 
            (room.TopRightAreaCorner.y - room.BottomLeftAreaCorner.y > corridorWidth))
        {
            for (int i = 0; i < enemyCount; i++)
            {
                // Spawn enemies in the room above the ground
                Vector3 enemySpawnPosition = new Vector3(
                    UnityEngine.Random.Range(room.BottomLeftAreaCorner.x, room.TopRightAreaCorner.x), 
                    1.0f,  // Adjusted spawn position to be above the ground
                    UnityEngine.Random.Range(room.BottomLeftAreaCorner.y, room.TopRightAreaCorner.y)
                );

                // Ensure the spawn position is within the room bounds
                if (IsPositionInRoom(room, enemySpawnPosition))
                {
                    GameObject enemy = Instantiate(enemyPrefab, enemySpawnPosition, Quaternion.identity);
                    enemy.GetComponent<MeshRenderer>().material = enemyMaterials[materialIndex];  // Set enemy material
                }
            }
            
        }
    }

    private void SpawnChestsInRoom(Node room) 
    {
            // Check if there's a chest to spawn based on the chest count
            int chestCount = GetChestCountForChestDice();
            if ((room.TopRightAreaCorner.x - room.BottomLeftAreaCorner.x > corridorWidth) && 
            (room.TopRightAreaCorner.y - room.BottomLeftAreaCorner.y > corridorWidth))
            {

                if (chestCount <= 0) return; // Exit if no chests to spawn

                // Calculate the center of the room
                Vector2 roomCenter = new Vector2(
                    (room.BottomLeftAreaCorner.x + room.TopRightAreaCorner.x) / 2,
                    (room.BottomLeftAreaCorner.y + room.TopRightAreaCorner.y) / 2
                );

                // Attempt to find a valid spawn position near the center of the room
                Vector3 chestSpawnPosition;
                do
                {
                    // Randomly offset from the center to find a spawn position
                    float offsetX = UnityEngine.Random.Range(-15f, 15f); // Adjust the range as needed
                    float offsetY = UnityEngine.Random.Range(-15f, 15f); // Adjust the range as needed

                    chestSpawnPosition = new Vector3(
                        roomCenter.x + offsetX,
                        0.5f, // Adjusted for height
                        roomCenter.y + offsetY
                    );
                } while (!IsPositionInRoom(room, chestSpawnPosition)); // Ensure it's within the room

                // Instantiate the chest
                int chestIndex = UnityEngine.Random.Range(0, chestPrefabs.Length);
                Instantiate(chestPrefabs[chestIndex], chestSpawnPosition, Quaternion.identity);
            }

    }

    private void SpawnTrapsInRoom(Node room) 
    {
        // Check if there's a chest to spawn based on the chest count
        int trapCount = GetTrapCountForTrapDice();

        if ((room.TopRightAreaCorner.x - room.BottomLeftAreaCorner.x > corridorWidth) && 
        (room.TopRightAreaCorner.y - room.BottomLeftAreaCorner.y > corridorWidth))
        {
            if (trapCount <= 0) return; // Exit if no chests to spawn

        // Calculate the center of the room
        Vector2 roomCenter = new Vector2(
            (room.BottomLeftAreaCorner.x + room.TopRightAreaCorner.x) / 2,
            (room.BottomLeftAreaCorner.y + room.TopRightAreaCorner.y) / 2
        );

        // Attempt to find a valid spawn position near the center of the room
        Vector3 trapSpawnPosition;
        do
        {
            // Randomly offset from the center to find a spawn position
            float offsetX = UnityEngine.Random.Range(-0.5f, 0.5f); // Adjust the range as needed
            float offsetY = UnityEngine.Random.Range(-0.5f, 0.5f); // Adjust the range as needed

            trapSpawnPosition = new Vector3(
                roomCenter.x + offsetX,
                0.5f, // Adjusted for height
                roomCenter.y + offsetY
            );
        } while (!IsPositionInRoom(room, trapSpawnPosition)); // Ensure it's within the room

        // Instantiate the chest
        int trapIndex = UnityEngine.Random.Range(0, trapsPrefabs.Length);
        Instantiate(trapsPrefabs[trapIndex], trapSpawnPosition, Quaternion.identity);
        }


    }

    private bool IsPositionInRoom(Node room, Vector3 position)
    {
        return position.x >= room.BottomLeftAreaCorner.x && position.x <= room.TopRightAreaCorner.x &&
               position.z >= room.BottomLeftAreaCorner.y && position.z <= room.TopRightAreaCorner.y;
    }

    private void AddWallPositionToList(Vector3 wallPosition, List<Vector3Int> wallList, List<Vector3Int> doorList)
    {
        Vector3Int point = Vector3Int.CeilToInt(wallPosition);
        if (wallList.Contains(point))
        {
            doorList.Add(point);
            wallList.Remove(point);
        }
        else
        {
            wallList.Add(point);
        }
    }

    public void DestroyAllChildren()
    {
        while (transform.childCount != 0)
        {
            foreach (Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }
}