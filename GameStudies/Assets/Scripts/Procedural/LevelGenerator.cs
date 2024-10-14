using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LevelGenerator
{
    private List<RoomNode> allNodesCollection = new List<RoomNode>();
    private int levelWidth;
    private int levelLength;
    private List<CorridorNode> generatedCorridors; // Store generated corridors

    public LevelGenerator(int levelWidth, int levelLength)
    {
        this.levelWidth = levelWidth;
        this.levelLength = levelLength;
    }

    public List<Vector2> GetCorridorPositions()
    {
        List<Vector2> corridorPositions = new List<Vector2>();

        // Check if corridors were generated before trying to access them
        if (generatedCorridors != null)
        {
            // Loop through each generated corridor and extract its positions
            foreach (var corridor in generatedCorridors)
            {
                corridorPositions.Add(corridor.BottomLeftAreaCorner); // Example: add bottom-left corner
                corridorPositions.Add(corridor.TopRightAreaCorner);   // Example: add top-right corner
            }
        }

        return corridorPositions;
    }

    public List<Node> CalculateLevel(int maxIterations, int roomWidthMin, int roomLengthMin, float roomBottomCornerModifier, float roomTopCornerModifier, int roomOffset, int corridorWidth)
    {
        BinarySpacePartitioner bsp = new BinarySpacePartitioner(levelWidth, levelLength);
        allNodesCollection = bsp.PrepareNodesCollection(maxIterations, roomWidthMin, roomLengthMin);
        List<Node> roomSpaces = StructureHelper.TraverseGraphToExtractLowestLeafes(bsp.RootNode);
        RoomGenerator roomGenerator = new RoomGenerator(maxIterations, roomLengthMin, roomWidthMin);
        List<RoomNode> roomList = roomGenerator.GenerateRoomsInGivenSpaces(roomSpaces, roomBottomCornerModifier, roomTopCornerModifier, roomOffset);

        CorridorsGenerator corridorGenerator = new CorridorsGenerator();
        generatedCorridors = corridorGenerator.CreateCorridor(allNodesCollection, corridorWidth); // Store generated corridors

        return new List<Node>(roomList).Concat(generatedCorridors).ToList(); // Combine room and corridor nodes
    }
}
