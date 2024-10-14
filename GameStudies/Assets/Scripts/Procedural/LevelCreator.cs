using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelCreator : MonoBehaviour
{
    public int levelWidth, levelLength;
    public int roomWidthmin, roomLengthmin;
    public int MaxIterations;
    public int CorridorWidth;
    public Material Floor_Material;
    public Material Wall_Material; // Material for walls
    [Range(0.0f, 0.3f)] public float roomBottomCornerModifier;
    [Range(0.7f, 1f)] public float roomTopCornerModifier;
    [Range(0.0f, 2f)] public int roomOffset;

    private List<Vector2> corridorPositions; // Declare corridorPositions
    private List<Vector3> wallPositions;

    void Start()
    {
        CreateLevel();
    }

    public void CreateLevel()
    {
        DestroyAllChildren();

        // Instantiate level generator and get rooms
        LevelGenerator generator = new LevelGenerator(levelWidth, levelLength);
        var listOfRooms = generator.CalculateLevel(
            MaxIterations,
            roomWidthmin,
            roomLengthmin,
            roomBottomCornerModifier,
            roomTopCornerModifier,
            roomOffset,
            CorridorWidth
        );

        // Initialize lists to store wall positions and corridor positions
        wallPositions = new List<Vector3>();
        corridorPositions = generator.GetCorridorPositions(); // Get corridor positions from generator

        // Create floors and walls for each room
        foreach (var room in listOfRooms.OfType<Node>())
        {
            CreateFloorMesh(room.BottomLeftAreaCorner, room.TopRightAreaCorner);
            CreateWallMesh(room.BottomLeftAreaCorner, room.TopRightAreaCorner);
        }
    }

    private void CreateFloorMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner)
    {
        // Convert 2D corners to 3D vectors
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0, bottomLeftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, 0, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, 0, topRightCorner.y);

        // Define vertices for the floor mesh
        Vector3[] vertices = new Vector3[] { topLeftV, topRightV, bottomLeftV, bottomRightV };
        Vector2[] uvs = vertices.Select(v => new Vector2(v.x, v.z)).ToArray();
        int[] triangles = new int[] { 0, 1, 2, 2, 1, 3 };

        // Create and assign mesh
        Mesh mesh = new Mesh { vertices = vertices, uv = uvs, triangles = triangles };
        GameObject levelFloor = new GameObject("Floor " + bottomLeftCorner, typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
        levelFloor.transform.position = Vector3.zero;
        levelFloor.transform.localScale = Vector3.one;

        // Set mesh and collider
        levelFloor.GetComponent<MeshFilter>().mesh = mesh;
        levelFloor.GetComponent<MeshCollider>().sharedMesh = mesh;
        levelFloor.GetComponent<MeshRenderer>().material = Floor_Material;

        // Set the parent to keep hierarchy organized
        levelFloor.transform.parent = transform;
    }

    private void CreateWallMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner)
    {
        // Convert 2D corners to 3D vectors for walls
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0, bottomLeftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, 0, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, 0, topRightCorner.y);

        // Create horizontal walls (bottom and top)
        CreateWallWithCorridor(bottomLeftV, bottomRightV, true);  // Bottom wall
        CreateWallWithCorridor(topLeftV, topRightV, true);        // Top wall

        // Create vertical walls (left and right)
        CreateWallWithCorridor(bottomLeftV, topLeftV, false);      // Left wall
        CreateWallWithCorridor(bottomRightV, topRightV, false);    // Right wall
    }

    private void CreateWallWithCorridor(Vector3 start, Vector3 end, bool isHorizontal)
    {
        // Debug log for wall creation attempt
        Debug.Log($"Attempting to create wall from {start} to {end}");

        // Calculate wall height
        float wallHeight = 3f; // Default height of the wall

        // Initialize list to hold positions for walls that will be created
        List<Vector3> wallSegments = new List<Vector3>();

        // Check for corridor positions and create walls accordingly
        foreach (Vector2 corridorPos in corridorPositions)
        {
            // Check if we're creating a horizontal wall
            if (isHorizontal)
            {
                // Horizontal Wall
                if (Mathf.Approximately(start.z, corridorPos.y) && corridorPos.x >= start.x && corridorPos.x <= end.x)
                {
                    // Add wall segments before and after the corridor
                    wallSegments.Add(start);
                    wallSegments.Add(new Vector3(corridorPos.x, 0, corridorPos.y)); // End before the corridor
                    wallSegments.Add(end); // End of wall

                    CreateSegmentedWall(wallSegments, wallHeight);
                    return; // Exit once the wall has been created
                }
            }
            else
            {
                // Vertical Wall
                if (Mathf.Approximately(start.x, corridorPos.x) && corridorPos.y >= start.z && corridorPos.y <= end.z)
                {
                    // Add wall segments before and after the corridor
                    wallSegments.Add(start);
                    wallSegments.Add(new Vector3(corridorPos.x, 0, corridorPos.y)); // End before the corridor
                    wallSegments.Add(end); // End of wall

                    CreateSegmentedWall(wallSegments, wallHeight);
                    return; // Exit once the wall has been created
                }
            }
        }

        // If no corridors detected, create the full wall normally
        CreateSingleWallMesh(start, end, wallHeight);
    }

    private void CreateSegmentedWall(List<Vector3> wallSegments, float wallHeight)
    {
        for (int i = 0; i < wallSegments.Count - 1; i++)
        {
            // Create wall segment between two points
            CreateSingleWallMesh(wallSegments[i], wallSegments[i + 1], wallHeight);
        }
    }

    private void CreateSingleWallMesh(Vector3 start, Vector3 end, float wallHeight)
    {
        // Ensure that start and end are distinct
        if (start == end)
        {
            Debug.LogWarning($"Start and end points for the wall are the same: {start}. Wall will not be created.");
            return;
        }

        // Calculate direction and length of the wall
        Vector3 direction = (end - start).normalized;
        float wallThickness = 0.1f; // Thickness of the wall

        // Define vertices for the wall mesh (two faces)
        Vector3[] vertices = new Vector3[]
        {
            start, // Bottom left front
            end,   // Bottom right front
            start + Vector3.up * wallHeight, // Top left front
            end + Vector3.up * wallHeight,   // Top right front

            // Back face vertices
            start - Vector3.Cross(direction, Vector3.up).normalized * wallThickness, // Bottom left back
            end - Vector3.Cross(direction, Vector3.up).normalized * wallThickness,   // Bottom right back
            start - Vector3.Cross(direction, Vector3.up).normalized * wallThickness + Vector3.up * wallHeight, // Top left back
            end - Vector3.Cross(direction, Vector3.up).normalized * wallThickness + Vector3.up * wallHeight // Top right back
        };

        // Define triangles for the wall mesh (two triangles for each face)
        int[] triangles = new int[]
        {
            // Front face
            0, 2, 1,
            1, 2, 3,
            // Back face
            4, 5, 6,
            6, 5, 7,
            // Connect front to back
            0, 1, 4,
            4, 1, 5,
            1, 3, 5,
            5, 3, 7,
            2, 6, 3,
            3, 6, 7,
            0, 4, 2,
            2, 4, 6
        };

        // Create and assign mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Create a new GameObject for the wall
        GameObject wall = new GameObject("Wall", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
        wall.GetComponent<MeshFilter>().mesh = mesh;
        wall.GetComponent<MeshCollider>().sharedMesh = mesh;
        wall.GetComponent<MeshRenderer>().material = Wall_Material;

        // Set the parent to keep hierarchy organized
        wall.transform.parent = transform;
    }

    public void DestroyAllChildren()
    {
        foreach (Transform child in transform)
        {
            //DestroyAllChildren();
        }
    }
}
