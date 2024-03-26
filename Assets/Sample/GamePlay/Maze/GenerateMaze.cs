using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Pancake.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class GenerateMaze : MonoBehaviour
{
    [SerializeField] private MazeCell _mazeCellPrefab;
    [SerializeField]
    private int _mazeWidth;

    [SerializeField]
    private int _mazeDepth;

    private MazeCell[,] _mazeGrid;
    [SerializeField] private Transform spawnGrid;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject destination;
    [SerializeField] private List<MazeCell> paths;
    private int _defaultMark = -1;
    private bool _isStopGenMaze;
    private int _endx;
    private int _endy;
    private int _index;
    private MazeCell _setPathElements;

    void Start()
    {
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, spawnGrid);
                _mazeGrid[x, z].transform.localPosition = new Vector3(z, 0, x);
                _mazeGrid[x, z].SetMark(_defaultMark);
                _mazeGrid[x, z].posx = x;
                _mazeGrid[x, z].posz = z;
            }
        }
        GenMaze(null, _mazeGrid[0, 0]);
        SetupPlayer();
        FindPath();
    }
    async void FindPath()
    {
        await UniTask.WaitUntil(() => _isStopGenMaze);
        CalculatePath();
    }
    public void AutoMove()
    {
        _index = paths.Count - 1;
        DoAutoMove();
    }
    void DoAutoMove()
    {
        if (_index < 0)
        {
            return;
        }
        player.transform.DOMove(paths[_index].transform.position, 0.5f).OnComplete((() =>
        {
            _index -= 1;
            DoAutoMove();
        }));
    }
    bool CheckDirection(int x, int y, int mark, int direction, bool isSetPath)
    {
        // 1 is up 2 is down 3 is left 4 is right
        switch (direction)
        {
            case 1:
                Debug.Log(_mazeGrid[x, y].frontWall.activeInHierarchy);
                if (x + 1 < _mazeWidth && !_mazeGrid[x, y].frontWall.activeInHierarchy && _mazeGrid[x + 1, y].mark == mark)
                {
                    if (isSetPath)
                    {
                        _setPathElements = _mazeGrid[x + 1, y];
                    }
                    return true;
                }
                else
                    return false;
            case 2:
                Debug.Log(_mazeGrid[x, y].backWall.activeInHierarchy);
                if (x - 1 > -1 && !_mazeGrid[x, y].backWall.activeInHierarchy && _mazeGrid[x - 1, y].mark == mark)
                {
                    if (isSetPath)
                    {
                        _setPathElements = _mazeGrid[x - 1, y];
                    }
                    return true;
                }
                else
                    return false;
            case 3:
                Debug.Log(_mazeGrid[x, y].leftWall.activeInHierarchy);
                if (y - 1 > -1 && !_mazeGrid[x, y].leftWall.activeInHierarchy && _mazeGrid[x, y - 1].mark == mark)
                {
                    if (isSetPath)
                    {
                        _setPathElements = _mazeGrid[x, y - 1];
                    }
                    return true;
                }
                else
                    return false;
            case 4:
                Debug.Log(_mazeGrid[x, y].rightWall.activeInHierarchy);
                if (y + 1 < _mazeDepth && !_mazeGrid[x, y].rightWall.activeInHierarchy && _mazeGrid[x, y + 1].mark == mark)
                {
                    if (isSetPath)
                    {
                        _setPathElements = _mazeGrid[x, y + 1];
                    }
                    return true;
                }
                else
                    return false;
        }
        return false;
    }
    void SetCheckDirection(int x, int y, int newMark)
    {
        if (CheckDirection(x, y, -1, 1, false))
        {
            SetMarked(x + 1, y, newMark);
        }
        if (CheckDirection(x, y, -1, 2, false))
        {
            SetMarked(x - 1, y, newMark);
        }
        if (CheckDirection(x, y, -1, 3, false))
        {
            SetMarked(x, y - 1, newMark);
        }
        if (CheckDirection(x, y, -1, 4, false))
        {
            SetMarked(x, y + 1, newMark);
        }
    }
    void SetMarked(int x, int y, int newMark)
    {
        if (_mazeGrid[x, y])
        {
            Debug.Log(newMark);
            _mazeGrid[x, y].mark = newMark;
        }
    }
    void CalculatePath()
    {
        for (int newMark = 1; newMark < _mazeWidth * _mazeDepth; newMark++)
        {
            foreach (var maze in _mazeGrid)
            {
                if (maze)
                {
                    if (maze.mark == newMark - 1)
                    {
                        SetCheckDirection(maze.posx, maze.posz, newMark);
                    }
                }
            }
        }
        SetPath();
    }
    void SetPath()
    {
        paths.Clear();
        int checkMark;
        var endGrid = _mazeGrid[_endx, _endy];
        paths.Add(endGrid);
        if (endGrid.mark > 0)
        {
            checkMark = endGrid.mark - 1;
            for (int i = checkMark; i > -1; i--)
            {
                for (int j = 1; j < 5; j++)
                {
                    if (CheckDirection(_endx, _endy, i, j, true))
                    {
                        paths.Add(_setPathElements);
                        _endx = _setPathElements.posx;
                        _endy = _setPathElements.posz;
                        break;
                    }
                }
            }
        }
    }
    void SetupPlayer()
    {

        var randomx = Random.Range(0, _mazeWidth);
        _endx = randomx;
        while (_endx == randomx)
        {
            _endx = Random.Range(0, _mazeWidth);
        }
        var randomy = Random.Range(0, _mazeDepth);
        _endy = randomy;
        while (_endy == randomy)
        {
            _endy = Random.Range(0, _mazeDepth);
        }
        _mazeGrid[randomx, randomy].mark = 0;
        player.transform.position = _mazeGrid[randomx, randomy].transform.position;
        destination.transform.position = _mazeGrid[_endx, _endy].transform.position;
    }

    private void GenMaze(MazeCell previousCell, MazeCell currentCell)
    {
        if (_isStopGenMaze)
        {
            Debug.Log("Dung");
            return;
        }
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;
        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                GenMaze(currentCell, nextCell);
            }
        }
        while (nextCell != null);
        _isStopGenMaze = isAllVisited();
    }
    bool isAllVisited()
    {
        foreach (var maze in _mazeGrid)
        {
            if (!maze.isVisited)
            {
                return false;
            }
        }
        return true;
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);

        return unvisitedCells.OrderBy(_ => Random.Range(1, 5)).FirstOrDefault();

    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.posx;
        int z = (int)currentCell.posz;


        if (x + 1 < _mazeWidth)
        {
            var cellToRight = _mazeGrid[x + 1, z];

            if (cellToRight.isVisited == false)
            {
                yield return cellToRight;
            }
        }

        if (x - 1 >= 0)
        {
            var cellToLeft = _mazeGrid[x - 1, z];

            if (cellToLeft.isVisited == false)
            {
                yield return cellToLeft;
            }
        }

        if (z + 1 < _mazeDepth)
        {
            var cellToFront = _mazeGrid[x, z + 1];

            if (cellToFront.isVisited == false)
            {
                yield return cellToFront;
            }
        }

        if (z - 1 >= 0)
        {
            var cellToBack = _mazeGrid[x, z - 1];

            if (cellToBack.isVisited == false)
            {
                yield return cellToBack;
            }
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null)
        {
            return;
        }

        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }
}
