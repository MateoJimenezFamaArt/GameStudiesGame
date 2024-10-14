using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LevelGenerator
{
    
    List<RoomNode> allNodesCollection = new List<RoomNode>();
    private int levelWidth;
    private int levelLenght;

    public LevelGenerator(int levelWidth, int levelLenght)
    {
        this.levelWidth = levelWidth;
        this.levelLenght = levelLenght;
    }

    public List<Node> CalculateLevel(int maxiterations, int roomWidthmin, int roomLenghtmin, float roomBottomCornerModifier, float roomTopCornerModifier, int roomOffset, int corridorWidth)
    {
        BinarySpacePartitioner bsp = new BinarySpacePartitioner(levelWidth, levelLenght);
        allNodesCollection = bsp.PrepareNodesCollection(maxiterations, roomWidthmin, roomLenghtmin); 
        List<Node> roomSpaces = StructureHelper.TraverseGraphToExtractLowestLeafes(bsp.RootNode);
        RoomGenerator roomGenerator = new RoomGenerator(maxiterations, roomLenghtmin, roomWidthmin);
        List<RoomNode> roomList = roomGenerator.GenerateRoomsInGivenSpaces(roomSpaces, roomBottomCornerModifier, roomTopCornerModifier, roomOffset);


        CorridorsGenerator corridorGenerator = new CorridorsGenerator();
        var corridorList = corridorGenerator.CreateCorridor(allNodesCollection,corridorWidth);

        return new List<Node>(roomList).Concat(corridorList).ToList();
    }


}

