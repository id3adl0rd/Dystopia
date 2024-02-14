using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator
{
	[SerializeField]
	protected RandomGenerationData randomWalkData;
	protected override void RunProceduralGeneration()
	{
		HashSet<Vector2Int> floorPositions = RunRandomWalk(randomWalkData, startPosition);
		tilemapVisualizer.Clear();
		tilemapVisualizer.PaintFloorTiles(floorPositions);
		WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
	}

	protected HashSet<Vector2Int> RunRandomWalk(RandomGenerationData _data, Vector2Int position)
	{
		var currentPosition = position;
		HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

		for (int i = 0; i < _data.iterations; i++)
		{
			var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, _data.walkLength);
			floorPositions.UnionWith(path);

			if (_data.startRandomlyEachIteration)
				currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
		}

		return floorPositions;
	}
}
