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
    public Material[] terrainMaterials; // Array of materials based on terrain types

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

        // Ensure there's only one player in the scene
        GameObject existingPlayer = GameObject.FindWithTag("Player");
        if (existingPlayer == null)
        {
            // Instantiate the player in the first room if no player exists
            Vector3 playerSpawnPosition = new Vector3(
                (listOfRooms[0].BottomLeftAreaCorner.x + listOfRooms[0].TopRightAreaCorner.x) / 2, 
                3, 
                (listOfRooms[0].BottomLeftAreaCorner.y + listOfRooms[0].TopRightAreaCorner.y) / 2
            );
            Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity);
        }

        // Determine material based on Terrain Dice
        Material floorMaterial = GetMaterialForTerrainDice();

        // Create the rooms and walls
        for (int i = 0; i < listOfRooms.Count; i++)
        {
            CreateMesh(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner, floorMaterial);
        }

        CreateWalls(wallParent, floorMaterial);
    }

    private void CreateWalls(GameObject wallParent, Material floorMaterial)
    {
        foreach (var wallPosition in possibleWallHorizontalPosition)
        {
            CreateWall(wallParent, wallPosition, wallHorizontal, floorMaterial);
        }
        foreach (var wallPosition in possibleWallVerticalPosition)
        {
            CreateWall(wallParent, wallPosition, wallVertical, floorMaterial);
        }
    }

    private void CreateWall(GameObject wallParent, Vector3Int wallPosition, GameObject wallPrefab, Material floorMaterial)
    {
        GameObject wall = Instantiate(wallPrefab, wallPosition + new Vector3(0, wallHeight / 2, 0), Quaternion.identity, wallParent.transform);
        wall.transform.localScale = new Vector3(1, wallHeight, 1); // Scale the wall to the desired height
        wall.GetComponent<MeshRenderer>().material = floorMaterial; // Set the wall material
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

    private Material GetMaterialForTerrainDice()
    {
        // Assuming your DiceData holds a list of dice with their names and values
        foreach (var dice in diceData.winnersDiceCombination)
        {
            if (dice.diceName == "Terrain Dice")
            {
                // Map the dice value to the corresponding material
                int index = Mathf.Clamp(dice.diceValue, 0, terrainMaterials.Length - 1); // Ensure we stay within bounds
                return terrainMaterials[index];
            }
        }

        // Fallback to default material if Terrain Dice is not found or invalid value
        return terrainMaterials[0];
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
