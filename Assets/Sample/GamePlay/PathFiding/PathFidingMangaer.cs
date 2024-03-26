using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PathFidingMangaer : MonoBehaviour
{
    [SerializeField] private GridStat gridStat;
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    private GridStat[,] elements;
    [SerializeField] private GameObject player;
    [SerializeField] private int startposix;
    [SerializeField] private int startposiy;
    [SerializeField] private int endposix;
    [SerializeField] private int endposiy;
    [SerializeField] private List<Transform> path;
    [SerializeField] private GridStat[] saveGrid;
    public float distance = 0.2f;
    private int _pathIndex = 0;
    public Vector3 road;
    public bool isMove;
    private GridStat _setPathElements;

    private void SpawnGrid()
    {
        path.Clear();
        isMove = false;
        elements = new GridStat[columns, rows];
        saveGrid = new GridStat[columns * rows];
        var setGridStat = gridStat.transform.localScale;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                var getObj = SpawnObj();
                getObj.x = j;
                getObj.y = i;
                getObj.mark = -1;
                elements[j, i] = getObj;
                saveGrid[(i * columns) + j] = getObj;
                getObj.gameObject.transform.position = new Vector3(setGridStat.x * (j + (j * distance)), 0, setGridStat.z * (i + (i * distance)));
            }
        }
    }
    private void Start()
    {
        elements = new GridStat[columns, rows];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (saveGrid[(i * 10) + j])
                {
                    var getObj = saveGrid[(i * 10) + j];
                    elements[j, i] = getObj;
                }
            }
        }
    }
    private void Update()
    {
        if (isMove)
        {
            DoMove();
        }
    }
    void DoMove()
    {
        road = path[_pathIndex].position + new Vector3(0, 1, 0);
        var dir = road - player.transform.position;
        player.transform.Translate(dir.normalized * 6 * Time.deltaTime);
        // Quaternion lookRotaion = Quaternion.LookRotation(dir, Vector3.up);
        // player.transform.rotation = Quaternion.Euler(0, lookRotaion.eulerAngles.y, 0);
        if (Vector3.Distance(player.transform.position, road) <= 0.05f)
        {
            SetNextPath();
        }
    }
    void SetNextPath()
    {
        player.transform.position = road;
        if (_pathIndex >= path.Count - 1)
        {
            _pathIndex = 0;
            Debug.Log("xomng");
            isMove = false;
            startposix = endposix;
            startposiy = endposiy;
        }
        else
        {
            _pathIndex++;
        }
    }
    GridStat SpawnObj()
    {
        var obj = Instantiate(gridStat, transform);
        return obj;
    }
    bool CheckDirection(int x, int y, int mark, int direction, bool isSetPath)
    {
        // 1 is up 2 is down 3 is left 4 is right
        switch (direction)
        {
            case 1:
                if (y + 1 < rows && elements[x, y + 1] && elements[x, y + 1].mark == mark)
                {
                    if (isSetPath)
                    {
                        _setPathElements = elements[x, y + 1];
                    }
                    return true;
                }
                else
                    return false;
            case 2:
                if (y - 1 > -1 && elements[x, y - 1] && elements[x, y - 1].mark == mark)
                {
                    if (isSetPath)
                    {
                        _setPathElements = elements[x, y - 1];
                    }
                    return true;
                }
                else
                    return false;
            case 3:
                if (x - 1 > -1 && elements[x - 1, y] && elements[x - 1, y].mark == mark)
                {
                    if (isSetPath)
                    {
                        _setPathElements = elements[x - 1, y];
                    }
                    return true;
                }
                else
                    return false;
            case 4:
                if (x + 1 < columns && elements[x + 1, y] && elements[x + 1, y].mark == mark)
                {
                    if (isSetPath)
                    {
                        _setPathElements = elements[x + 1, y];
                    }
                    return true;
                }
                else
                    return false;
        }
        return false;
    }
    public void SetSlerp()
    {
        Eva(elements[startposix, startposiy].transform.position, elements[endposix, endposiy].transform.position, 2);

    }
    void Eva(Vector3 start, Vector3 end, float centerosset)
    {
        
    }
    void SetCheckDirection(int x, int y, int newMark)
    {
        if (CheckDirection(x, y, -1, 1, false))
        {
            SetMarked(x, y + 1, newMark);
        }
        if (CheckDirection(x, y, -1, 2, false))
        {
            SetMarked(x, y - 1, newMark);
        }
        if (CheckDirection(x, y, -1, 3, false))
        {
            SetMarked(x - 1, y, newMark);
        }
        if (CheckDirection(x, y, -1, 4, false))
        {
            SetMarked(x + 1, y, newMark);
        }
    }
    void SetMarked(int x, int y, int newMark)
    {
        if (elements[x, y])
        {
            elements[x, y].mark = newMark;
        }
    }
    void InitializeSetUp()
    {
        foreach (var element in elements)
        {
            if (element)
            {
                element.mark = -1;
            }
        }
        elements[startposix, startposiy].mark = 0;
    }
    void CalculatePath()
    {
        InitializeSetUp();
        int i = 0;
        foreach (var elementCheck in elements)
        {
            if (elementCheck)
            {
                i++;
            }
        }
        for (int newMark = 1; newMark < rows * columns; newMark++)
        {
            foreach (var elementCheck in elements)
            {
                if (elementCheck)
                {
                    if (elementCheck.mark == newMark - 1)
                    {
                        SetCheckDirection(elementCheck.x, elementCheck.y, newMark);
                    }
                }
            }
        }
    }
    void SetPath()
    {
        path.Clear();
        int checkMark;
        int endx = endposix;
        int endy = endposiy;
        if (elements[endposix, endposiy] && elements[endposix, endposiy].mark > 0)
        {
            checkMark = elements[endposix, endposiy].mark - 1;
            for (int i = checkMark; i > -1; i--)
            {
                _setPathElements = null;
                for (int j = 1; j <= 4; j++)
                {
                    if (CheckDirection(endx, endy, i, j, true))
                    {
                        path.Add(_setPathElements.transform);
                        endx = _setPathElements.x;
                        endy = _setPathElements.y;
                        break;
                    }
                }
            }
            path.Add(elements[endposix, endposiy].transform);
            Sort();
            isMove = true;
        }
        else
        {
            Debug.Log("No destination");
            return;
        }
    }
    void Sort()
    {
        Transform term;
        for (int i = 0; i < path.Count; i++)
        {
            for (int j = i + 1; j < path.Count; j++)
            {
                if (path[i].GetComponent<GridStat>().mark > path[j].GetComponent<GridStat>().mark)
                {
                    term = path[i];
                    path[i] = path[j];
                    path[j] = term;
                }
            }
        }
    }
    GridStat FindClosert(Transform targetPosi, List<GridStat> list)
    {
        float distance = rows * columns;
        int indexNumber = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (Vector3.Distance(targetPosi.position, list[i].gameObject.transform.position) < distance)
            {
                distance = Vector3.Distance(targetPosi.position, list[i].transform.position);
                indexNumber = i;
            }
        }
        return list[indexNumber];
    }
    void ClearGrid()
    {
        if (transform.childCount == 0) return;
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var getChild = transform.GetChild(i);
            if (getChild.gameObject.activeInHierarchy)
            {
                DestroyImmediate(getChild.gameObject);
            }
        }
    }
    void UpdatePlayerPosi()
    {
        var startpos = elements[startposix, startposiy].transform.position;
        player.transform.position = new Vector3(startpos.x, startpos.y + 1, startpos.z);
    }
    void ShowPath()
    {
        CalculatePath();
        SetPath();
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(PathFidingMangaer), true)]
    [CanEditMultipleObjects]
    public class SpawnGridEditor : Editor
    {
        private PathFidingMangaer _pathFidingMangaer;
        private void OnEnable()
        {
            _pathFidingMangaer = target as PathFidingMangaer;
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            if (GUILayout.Button("CreateGrid", GUILayout.MinHeight(40), GUILayout.MinWidth(100)))
            {
                _pathFidingMangaer.SpawnGrid();
            }
            if (GUILayout.Button("Clear", GUILayout.MinHeight(40), GUILayout.MinWidth(100)))
            {
                _pathFidingMangaer.ClearGrid();
            }
            if (GUILayout.Button("UpdatePlayerPosi", GUILayout.MinHeight(40), GUILayout.MinWidth(100)))
            {
                _pathFidingMangaer.UpdatePlayerPosi();
            }
            if (GUILayout.Button("ShowPath", GUILayout.MinHeight(40), GUILayout.MinWidth(100)))
            {
                _pathFidingMangaer.ShowPath();
            }
           if (GUILayout.Button("TestLerp", GUILayout.MinHeight(40), GUILayout.MinWidth(100)))
            {
                _pathFidingMangaer.SetSlerp();
            }
            base.OnInspectorGUI();
        }
    }
  #endif
}
