using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Surface : MonoBehaviour
{
    [SerializeField] private GameObject slot;
    private float _lenght;
    private float _height;
    [SerializeField] int searchRow;
    [SerializeField] int searchColumn;
    [SerializeField] int maxLength;
    [SerializeField] int maxHeight;
    public bool isItem;
    [SerializeField] ItemSlot[] _itemSlots;
    private ItemSlot[,] _slotArray;
    [SerializeField] private ItemSlot slotcheck;
    [SerializeField] private List<ItemSlot> surfaceCheck = new List<ItemSlot>();
    public void SpawnSlot()
    {
        _slotArray = new ItemSlot[maxLength, maxHeight];
        _lenght = transform.lossyScale.x;
        _height = transform.lossyScale.z;
        var startIntX = _lenght / 2;
        var startInsZ = _height / 2;
        maxLength = Convert.ToInt32(Mathf.Floor(_lenght / slot.transform.localScale.x));
        maxHeight = Convert.ToInt32(Mathf.Floor(_height / slot.transform.localScale.z));
        var halfCenterX = slot.transform.localScale.x / 2;
        var halfCenterZ = slot.transform.localScale.z / 2;
        _itemSlots = new ItemSlot[maxLength * maxHeight];
        var posi = transform.position;
        for (int j = 0; j < maxHeight; j++)
        {
            for (int i = 0; i < maxLength; i++)
            {
                var getObj = Instantiate(slot);
                if (!isItem)
                {
                    getObj.transform.localPosition = new Vector3(posi.x - startIntX + (halfCenterX + (slot.transform.localScale.x * i)), posi.y + (transform.lossyScale.y / 2) - (slot.transform.localScale.y / 2), posi.z - startInsZ + (halfCenterZ + (slot.transform.localScale.z * j)));
                }
                getObj.transform.SetParent(this.transform);
                getObj.GetComponent<ItemSlot>().column = i;
                getObj.GetComponent<ItemSlot>().row = j;
                _itemSlots[j * maxLength + i] = getObj.GetComponent<ItemSlot>();
                _slotArray[i, j] = getObj.GetComponent<ItemSlot>();
            }
        }

    }
    public void ClearSlots()
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
    public bool SlotCheck(ItemSlot itemSlot, int rowArea, int columArea)
    {
        surfaceCheck.Clear();
        var count = 0;
        foreach (var check in _slotArray)
        {
            if (check == itemSlot)
            {
                var setRow = rowArea % 2 == 0 ? rowArea + 1 : rowArea;
                var setColum = columArea % 2 == 0 ? columArea + 1 : columArea;
                int indexCheckRow = -(setRow / 2);
                for (int i = 0; i < setRow; i++)
                {
                    int indexCheckColum = -(setColum / 2);
                    for (int j = 0; j < setColum; j++)
                    {
                        var a = check.row + indexCheckRow;
                        var b = check.column + indexCheckColum;
                        if (a < maxHeight && a > -1)
                        {
                            if (b < maxLength && b > -1)
                            {
                                if (_slotArray[b, a].isCollide != true)
                                {
                                    count++;
                                    surfaceCheck.Add(_slotArray[b, a]);
                                }
                            }
                        }
                        indexCheckColum++;
                    }
                    indexCheckRow++;
                }
                if (count == (setRow * setColum))
                {
                    return true;
                }
                else
                {
                    surfaceCheck.Clear();
                    return false;
                }
            }
        }
        return false;
    }
    private void OnEnable()
    {
        Observer.FingerUp += CheckColide;
    }
    private void OnDisable()
    {
        Observer.FingerUp -= CheckColide;
    }
    void CheckColide()
    {
        if (surfaceCheck.Count == 0) return;
        foreach (var s in _slotArray)
        {
            s.isCollide = false;
        }
        foreach (var setColi in surfaceCheck)
        {
            setColi.isCollide = true;
        }
    }
    public void Search()
    {
        if (_slotArray[searchRow, searchColumn] != null)
        {
            slotcheck = _slotArray[searchRow, searchColumn];
        }
    }
    private void Awake()
    {
        _slotArray = new ItemSlot[maxLength, maxHeight];
        for (int i = 0; i < maxHeight; i++)
        {
            for (int j = 0; j < maxLength; j++)
            {
                if (_itemSlots[(i * maxLength) + j])
                {
                    var getObj = _itemSlots[(i * maxLength) + j];
                    _slotArray[j, i] = getObj;
                }
            }
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(Surface), true)]
    [CanEditMultipleObjects]
    public class SpawnSlots : Editor
    {
        private Surface _surface;
        private void OnEnable()
        {
            _surface = target as Surface;
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            if (GUILayout.Button("CreateSlots", GUILayout.MinHeight(40), GUILayout.MinWidth(100)))
            {
                _surface.SpawnSlot();
            }
            if (GUILayout.Button("Clear", GUILayout.MinHeight(40), GUILayout.MinWidth(100)))
            {
                _surface.ClearSlots();
            }
            if (GUILayout.Button("Search", GUILayout.MinHeight(40), GUILayout.MinWidth(100)))
            {
                _surface.Search();
            }
            base.OnInspectorGUI();
        }
    }
#endif
}
