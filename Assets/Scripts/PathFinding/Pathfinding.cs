using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private PathNode[,] nodeGrid;
    private List<PathNode> openList;
    private List<PathNode> closedList;
    private bool isTargetPositionInteractable;
    private bool isTargetObjectSmall;
    private List<PathNode> targetEnterances;

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public static Pathfinding Instance { get; private set; }

    // Creates a static PathFinding Instance from the GameGrid class
    public Pathfinding ()
    {
        Instance = this;
    }

    // Returns a Vector3 List that acts as a cooridinate path for an object to follow
    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition){
        
        Vector2Int startGridPosition = GameGrid.Instance.GetGridPosFromWorld(startWorldPosition);
        Vector2Int endGridPosition = GameGrid.Instance.GetGridPosFromWorld(endWorldPosition);

        List<PathNode> path = FindPath(startGridPosition, endGridPosition);
        if (path == null){
            return null;
        }else{
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (PathNode pathNode in path){
                vectorPath.Add(new Vector3(pathNode.gridPosX, .5f, pathNode.gridPosZ) * GameGrid.Instance.gridSpaceSize);
            }
            return vectorPath;
        }
    }

    // Returns a PathNode List that acts as a cooridinate path for an object to follow
    public List<PathNode> FindPath(Vector2Int startGridPosition, Vector2Int endGridPosition)
    {
        nodeGrid = new PathNode[GameGrid.Instance.gridLength, GameGrid.Instance.gridWidth];
        nodeGrid = GameGrid.Instance.pathNodeArray;
        PathNode startNode = GameGrid.Instance.GetPathNodeInformation(startGridPosition);
        PathNode endNode = GameGrid.Instance.GetPathNodeInformation(endGridPosition);
        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();
        targetEnterances = new List<PathNode>();
        isTargetPositionInteractable = false;
        isTargetObjectSmall = false;
        
        if (endNode.isOccupyingObjectInteratable){
            isTargetPositionInteractable = true;
            if (endNode.occupyingObject.tag == "Army" | endNode.occupyingObject.tag == "Resource"){
                isTargetObjectSmall = true;
            }else {
                targetEnterances = endNode.occupyingObject.GetComponent<ObjectEnterance>().GetEnteranceList();
                if (targetEnterances.Count > 0){
                    PathNode bestEnterance = targetEnterances[0];
                    int minDistance = int.MaxValue;
                    int currentDistance;
                    for (int i = 0; i < targetEnterances.Count; i++)
                    {
                        currentDistance = CalculateDistanceCost(startNode, targetEnterances[i]);
                        if (currentDistance < minDistance)
                        {
                            if (targetEnterances[i].isWalkable){
                                minDistance = currentDistance;
                                bestEnterance = targetEnterances[i];
                            }
                        }
                    }
                    if (bestEnterance == null){
                        startNode.isWalkable = false;
                        return null;
                    }
                    endNode = bestEnterance;
                } 
            }
        }

        for (int x = 0; x < GameGrid.Instance.GetGridWidth(); x++)
        {
            for (int z = 0; z < GameGrid.Instance.GetGridLength(); z++)
            {
                PathNode pathNode = GameGrid.Instance.GetPathNodeInformation(new Vector2Int(x, z));
                pathNode.gCost = 99999;
                pathNode.CalculateFCost();
                pathNode.cameFromCell = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();
        
        while (openList.Count > 0){
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode){
                return CalculatePath(endNode);
            }else if (isTargetObjectSmall){
                if (GetNeighbourList(currentNode).Contains(endNode)){
                    return CalculatePath(currentNode);
                }
            }else if (targetEnterances.Count != 0){
                if (targetEnterances.Contains(currentNode)){
                    return CalculatePath(currentNode);
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode)){
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.isWalkable){
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost){
                    neighbourNode.cameFromCell = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode)){
                        openList.Add(neighbourNode);
                    }
                }

            }

        }
        // Out of nodes on the map
        startNode.isWalkable = false;
        return null;
    }

    // Returns a PathNode List of all available neighbour nodes
    private List <PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        if (currentNode.gridPosX - 1 >= 0){
            // Left
            neighbourList.Add(nodeGrid[currentNode.gridPosX - 1, currentNode.gridPosZ]);
            // Left Down
            if (currentNode.gridPosZ - 1 >= 0) neighbourList.Add(nodeGrid[currentNode.gridPosX - 1, currentNode.gridPosZ - 1]);
            // Left Up
            if (currentNode.gridPosZ + 1 < GameGrid.Instance.GetGridLength()) neighbourList.Add(nodeGrid[currentNode.gridPosX - 1, currentNode.gridPosZ + 1]);
        }
        if (currentNode.gridPosX + 1 < GameGrid.Instance.GetGridWidth()){
            // Right
            neighbourList.Add(nodeGrid[currentNode.gridPosX + 1, currentNode.gridPosZ]);
            // Right Down
            if (currentNode.gridPosZ - 1 >= 0) neighbourList.Add(nodeGrid[currentNode.gridPosX + 1, currentNode.gridPosZ - 1]);
            // Right Up
            if (currentNode.gridPosZ + 1 < GameGrid.Instance.GetGridLength()) neighbourList.Add(nodeGrid[currentNode.gridPosX + 1, currentNode.gridPosZ + 1]);
        }
        if (currentNode.gridPosX - 1 >= 0){
            // Down
            if (currentNode.gridPosZ - 1 >= 0) neighbourList.Add(nodeGrid[currentNode.gridPosX, currentNode.gridPosZ - 1]);
            // Up
            if (currentNode.gridPosZ + 1 < GameGrid.Instance.GetGridLength()) neighbourList.Add(nodeGrid[currentNode.gridPosX, currentNode.gridPosZ + 1]);
        }

        return neighbourList;
    }

    // Calculates the most efficient path from the starting position to the end position based on the A* algorithm
    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.cameFromCell != null){
            path.Add(currentNode.cameFromCell);
            currentNode = currentNode.cameFromCell;
        }
        path.Reverse();
        return path;
    }

    // Calculates a rough estimate of the distance from the staring node to the end position without taking obstacles into account 
    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.gridPosX - b.gridPosX);
        int yDistance = Mathf.Abs(a.gridPosZ - b.gridPosZ);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    // Returns the closest estimated PathNode to the end position from a given list
    private PathNode GetLowestFCostNode (List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++){
            if (pathNodeList[i].fCost < lowestFCostNode.fCost){
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }
}
