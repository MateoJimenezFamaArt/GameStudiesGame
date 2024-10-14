using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

internal class BinarySpacePartitioner
{
    RoomNode rootNode;
    public RoomNode RootNode { get => rootNode;}

    public BinarySpacePartitioner(int levelWidth, int levelLenght)
    {
        this.rootNode = new RoomNode(new Vector2Int(0,0),new Vector2Int(levelWidth,levelLenght),null,0);

    }

    public List<RoomNode> PrepareNodesCollection(int maxiterations, int roomWidthmin, int roomLenghtmin)
    {
        Queue<RoomNode> graph = new Queue<RoomNode>();
        List<RoomNode> listToReturn = new List<RoomNode>();
        graph.Enqueue(this.rootNode);
        listToReturn.Add(this.rootNode);
        int iterations = 0;
        while (iterations < maxiterations && graph.Count>0)
        {
            iterations++;
            RoomNode currentNode = graph.Dequeue();
            if (currentNode.Width >= roomWidthmin*2 || currentNode.Lenght >= roomLenghtmin*2);
            {
                SplitTheSpace(currentNode,listToReturn,roomLenghtmin,roomWidthmin, graph);
            }

        }
        return listToReturn;
    }

    private void SplitTheSpace(RoomNode currentNode, List<RoomNode> listToReturn, int roomLenghtmin, int roomWidthmin, Queue<RoomNode> graph)
    {
        Line line = GetLineDividingSpace(
            currentNode.BottomLeftAreaCorner, 
            currentNode.TopRightAreaCorner, 
            roomWidthmin, 
            roomLenghtmin);
        RoomNode node1 , node2;
        if (line.Orientation == Orientation.Horizontal)
        {
            node1 = new RoomNode(currentNode.BottomLeftAreaCorner,
                new Vector2Int(currentNode.TopRightAreaCorner.x, line.Coordinates.y),
                currentNode,
                currentNode.TreeLayerIndex+1);
            node2 = new RoomNode(new Vector2Int(currentNode.BottomLeftAreaCorner.x, line.Coordinates.y),
                currentNode.TopRightAreaCorner,
                currentNode,
                currentNode.TreeLayerIndex+1);
        }
        else
        {
            node1 = new RoomNode(currentNode.BottomLeftAreaCorner,
                new Vector2Int(line.Coordinates.x,currentNode.TopRightAreaCorner.y),
                currentNode,
                currentNode.TreeLayerIndex+1);
            node2 = new RoomNode(new Vector2Int(line.Coordinates.x,currentNode.BottomLeftAreaCorner.y),
                currentNode.TopRightAreaCorner,
                currentNode,
                currentNode.TreeLayerIndex+1);
        }
        AddNewNodeToCollections(listToReturn,graph,node1);
        AddNewNodeToCollections(listToReturn,graph,node2);

    }

    private void AddNewNodeToCollections(List<RoomNode> listToReturn, Queue<RoomNode> graph, RoomNode node)
    {
        listToReturn.Add(node);
        graph.Enqueue(node);
    }

    private Line GetLineDividingSpace(Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int roomWidthmin, int roomLenghtmin)
    {
        Orientation orientation;
        bool lenghtStatus = (topRightAreaCorner.y - bottomLeftAreaCorner.y)>=2*roomLenghtmin;
        bool widthStatus = (topRightAreaCorner.x - bottomLeftAreaCorner.x)>=2*roomWidthmin;
        if(lenghtStatus&&widthStatus)
        {
            orientation = (Orientation)(Random.Range(0,2));
        } else if (widthStatus)
        {
            orientation = Orientation.Vertical;
        }
        else
        {
            orientation = Orientation.Horizontal;
        }
        return new Line(orientation,GetCoordinatesFororientation(
            orientation,
            bottomLeftAreaCorner,
            topRightAreaCorner,
            roomWidthmin,
            roomLenghtmin));
    }

    private Vector2Int GetCoordinatesFororientation(Orientation orientation, Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int roomWidthmin, int roomLenghtmin)
    {
        Vector2Int coordinates = Vector2Int.zero;
        if (orientation == Orientation.Horizontal)
        {
            coordinates = new Vector2Int(
            0,
            Random.Range(
            (bottomLeftAreaCorner.y + roomLenghtmin),
            (topRightAreaCorner.y - roomLenghtmin)));
        }
        else
        {
            coordinates = new Vector2Int(
            Random.Range(
            (bottomLeftAreaCorner.x + roomWidthmin),
            (topRightAreaCorner.x - roomWidthmin)),
            0
            );

        }
        return coordinates;
    }
}

