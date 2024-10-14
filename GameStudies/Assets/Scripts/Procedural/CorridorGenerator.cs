using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorsGenerator
{
    public List<CorridorNode> CreateCorridor(List<RoomNode> allNodesCollection, int corridorWidth)
    {
        List<CorridorNode> corridorList = new List<CorridorNode>();
        Queue<RoomNode> structuresToCheck = new Queue<RoomNode>(
            allNodesCollection.OrderByDescending(node => node.TreeLayerIndex).ToList());

        while (structuresToCheck.Count > 0)
        {
            var node = structuresToCheck.Dequeue();
            if (node.ChildrenNodeList.Count < 2) // Ensure at least two children for corridor generation
            {
                continue;
            }

            // Create corridors between each pair of children
            for (int i = 0; i < node.ChildrenNodeList.Count - 1; i++)
            {
                for (int j = i + 1; j < node.ChildrenNodeList.Count; j++)
                {
                    CorridorNode corridor = new CorridorNode(node.ChildrenNodeList[i], node.ChildrenNodeList[j], corridorWidth);
                    corridorList.Add(corridor);
                }
            }
        }

        return corridorList;
    }
}
