using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {
        var basicWallPositions = FindWallInDirection(floorPositions, Direction2D.cardinalDirectionList);
        var cornerWallPositions = FindWallInDirection(floorPositions, Direction2D.diagonalDirectionList);
        
        CreateBasicWall(tilemapVisualizer, basicWallPositions, floorPositions);
        CreateCornerWalls(tilemapVisualizer, cornerWallPositions, floorPositions);
    }

    private static void CreateCornerWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> cornerWallPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach (var position in cornerWallPositions)
        {
            string neighboursBinary = "";

            foreach (var direction in Direction2D.eightDirectionList)
            {
                var neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition))
                {
                    neighboursBinary += "1";
                }
                else
                {
                    neighboursBinary += "0";
                }
            }
            
            Debug.Log(position);

            tilemapVisualizer.PaintSingleCornerWall(position, neighboursBinary);
        }
    }

    private static void CreateBasicWall(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> basicWallPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach (var position in basicWallPositions)
        {
            string neighboursBinary = "";

            foreach (var direction in Direction2D.cardinalDirectionList)
            {
                var neighbourPosition = position + direction;

                if (floorPositions.Contains(neighbourPosition))
                {
                    neighboursBinary += "1";
                }
                else
                {
                    neighboursBinary += "0";
                }
            }
            
            tilemapVisualizer.PaintSingleBasicWall(position, neighboursBinary);
        }
    }

    private static HashSet<Vector2Int> FindWallInDirection(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPosition = new HashSet<Vector2Int>();

        foreach (var position in floorPositions)
        {
            foreach (var direction in directionList)
            {
                var neighbourPosition = position + direction;

                if (floorPositions.Contains(neighbourPosition) == false)
                    wallPosition.Add(neighbourPosition);
            }
        }

        return wallPosition;
    }
}