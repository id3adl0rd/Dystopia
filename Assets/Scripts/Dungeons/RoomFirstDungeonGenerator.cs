using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Dungeons.Params.DungeonObject;
using Inventory;
using Inventory.Model;
using UI.Inventory;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class RoomFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]
    private int dungeonWidth = 20, dungeonHeight = 20;
    [SerializeField]
    [Range(0, 10)]
    private int offset = 1;
    [SerializeField]
    private bool randomWalkRooms = false;

    [SerializeField] private Camera _camera;
    [SerializeField] private Camera _miniMap;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private GameObject _exitPrefab;
    [SerializeField] private GameObject _invUI;
    [SerializeField] private GameObject _notifyUI;
    [SerializeField] private PlayerFOV _fov;


    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, 
            new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        if (randomWalkRooms)
        {
            floor = CreateRoomsRandomly(roomsList);
        }
        else
        {
            floor = CreateSimpleRooms(roomsList);   
        }

        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomsList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);
        
        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);

        PopulateDungeon(roomsList, roomCenters);
    }

    [SerializeField] private GameObject _playerObject;
    [SerializeField] private DungeonObjectSO[] _objects;
    
    private void PopulateDungeon(List<BoundsInt> roomsList, List<Vector2Int> roomCenters)
    {
        //var room = (Vector2Int)Vector3Int.RoundToInt(roomsList[0].center);
        //var _playerInstatiated = Instantiate(_playerObject, new Vector2(room.x, room.y), Quaternion.identity);
        
        var exitROom = (Vector2Int)Vector3Int.RoundToInt(roomsList[roomsList.Count - 1].center);
        Instantiate(_exitPrefab, new Vector2(exitROom.x, exitROom.y), Quaternion.identity);
        
        //_camera.GetComponent<SmoothCamera>().SetTarget(_playerInstatiated);
        //_miniMap.GetComponent<SmoothCamera>().SetTarget(_playerInstatiated);

        //_playerInstatiated.GetComponent<InventoryController>()._inventoryUI = _invUI;

        //_virtualCamera.Follow = _playerInstatiated.transform;

        foreach (var roomcool in roomsList)
        {
            if (roomcool.center == roomsList[0].center) continue;
        }

        for (int i = 0; i < roomsList.Count; i++)
        {
            if (i == 0) continue; 
            
            var roomBounds = roomsList[i];
            
            foreach (var position in roomsList)
            {
                if (position.x >= (roomBounds.xMin + offset) && position.x <= (roomBounds.xMax - offset) &&
                    position.y >= (roomBounds.yMin - offset) && position.y <= (roomBounds.yMax - offset))
                {
                    var currentPrefab = _objects[Random.Range(0, _objects.Length)];
                    
                    if (currentPrefab._chance <= Random.Range(0, 100))
                    {
                        Instantiate(currentPrefab._gameObject, new Vector2(position.x, position.y), Quaternion.identity);
                    }
                }
            }
        }
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList)
    {
        var room = (Vector2Int)Vector3Int.RoundToInt(roomsList[0].center);
        var _playerInstatiated = Instantiate(_playerObject, new Vector2(room.x, room.y), Quaternion.identity);
        _camera.GetComponent<SmoothCamera>().SetTarget(_playerInstatiated);
        _miniMap.GetComponent<SmoothCamera>().SetTarget(_playerInstatiated);
        _virtualCamera.Follow = _playerInstatiated.transform;
        _playerInstatiated.GetComponent<ShakeCameraController>()._vcam = _virtualCamera;
        _playerInstatiated.GetComponent<InventoryController>()._inventoryUI = _invUI.GetComponentInChildren<InventoryPage>();
        _playerInstatiated.GetComponent<NotifyController>()._gameObject = _notifyUI;
        _playerInstatiated.GetComponent<CharacterAim>().playerFOV = _fov;
        
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i = 0; i < roomsList.Count; i++)
        {
            var roomBounds = roomsList[i];
            var roomCenter =
                new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkData, roomCenter);
            foreach (var position in roomFloor)
            {
                if (position.x >= (roomBounds.xMin + offset) && position.x <= (roomBounds.xMax - offset) && position.y >= (roomBounds.yMin - offset) && position.y <= (roomBounds.yMax - offset))
                {
                    floor.Add(position);

                    if (i == 0) continue;
                    
                    var currentPrefab = _objects[Random.Range(0, _objects.Length)];
                    if (currentPrefab._chance >= Random.Range(0, 100))
                    {
                        Instantiate(currentPrefab._gameObject, new Vector2(position.x - 0.48f, position.y - 0.48f), Quaternion.identity);
                    } 
                }
            }
        }

        return floor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }

        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);

        while (position.y != destination.y)
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if (destination.y < position.y)
            {
                position += Vector2Int.down;
            }

            corridor.Add(position);
        }

        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if (destination.x < position.x)
            {
                position += Vector2Int.left;
            }

            corridor.Add(position);
        }
        
        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;

        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }

        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        foreach (var room in roomsList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                    
                    var currentPrefab = _objects[Random.Range(0, _objects.Length)];
                    if (currentPrefab._chance >= Random.Range(0, 100))
                    {
                        Instantiate(currentPrefab._gameObject, new Vector2(position.x, position.y), Quaternion.identity);
                    } 
                }
            }
        }

        return floor;
    }
}