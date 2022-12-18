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
        
        // Convert the start and end positions from world coordinates to grid coordinates
        Vector2Int startGridPosition = GameGrid.Instance.GetGridPosFromWorld(startWorldPosition);
        Vector2Int endGridPosition = GameGrid.Instance.GetGridPosFromWorld(endWorldPosition);

        // Find the path in grid coordinates and convert it to world coordinates
        List<PathNode> path = FindPath(startGridPosition, endGridPosition);
        if (path == null){
            // Return null if no path is found
            return null;
        }else{
            List<Vector3> vectorPath = new List<Vector3>();
            // Convert the path to a list of Vector3 objects
            foreach (PathNode pathNode in path){
                vectorPath.Add(new Vector3(pathNode.gridPosX, .5f, pathNode.gridPosZ) * GameGrid.Instance.gridSpaceSize);
            }
            return vectorPath;
        }
    }

    // Returns a PathNode List that acts as a cooridinate path for an object to follow
    public List<PathNode> FindPath(Vector2Int startGridPosition, Vector2Int endGridPosition)
    {
        // Initialize the node grid, open list, and closed list
        nodeGrid = new PathNode[GameGrid.Instance.gridLength, GameGrid.Instance.gridWidth];
        nodeGrid = GameGrid.Instance.pathNodeArray;

        // Get the start and end nodes
        PathNode startNode = GameGrid.Instance.GetPathNodeInformation(startGridPosition);
        PathNode endNode = GameGrid.Instance.GetPathNodeInformation(endGridPosition);
        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();
        targetEnterances = new List<PathNode>();
        isTargetPositionInteractable = false;
        isTargetObjectSmall = false;
        
        // If the end node is occupied by an interactable object, check whether it is small or has entrances
        if (endNode.isOccupyingObjectInteratable){
            isTargetPositionInteractable = true;
            if (endNode.occupyingObject.tag == "Army" | endNode.occupyingObject.tag == "Resource"){
                isTargetObjectSmall = true;
            }else {
                targetEnterances = endNode.occupyingObject.GetComponent<ObjectEnterance>().GetEnteranceList();
                // If the object has entrances, find the entrance closest to the start position
                if (targetEnterances.Count > 0){
                    PathNode bestEnterance = targetEnterances[0];
                    int minDistance = int.MaxValue;
                    int currentDistance;
                    for (int i = 0; i < targetEnterances.Count; i++)
                    {
                        // Calculate the distance from the start node to the current entrance
                        currentDistance = CalculateDistanceCost(startNode, targetEnterances[i]);
                        if (currentDistance < minDistance)
                        {
                            // If the entrance is walkable and closer than the current best entrance, set it as the new best entrance
                            if (targetEnterances[i].isWalkable){
                                minDistance = currentDistance;
                                bestEnterance = targetEnterances[i];
                            }
                        }
                    }
                    // If no walkable entrance was found, return null
                    if (bestEnterance == null){
                        startNode.isWalkable = false;
                        return null;
                    }
                    // Set the end node to the best entrance
                    endNode = bestEnterance;
                } 
            }
        }

        // Set the gCost and hCost of all nodes to large numbers, and set their cameFromCell values to null
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

        // Set the gCost of the start node to 0 and calculate its hCost and fCost
        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();
        
        // Continue searching for a path until the open list is empty or the end node has been added to the closed list
        while (openList.Count > 0){
            // Sort the open list by fCost
            PathNode currentNode = GetLowestFCostNode(openList);

            // If the current node is the end node, retrace the path and return 
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
