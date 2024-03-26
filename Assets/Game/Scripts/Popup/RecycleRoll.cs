using System;
using System.Collections.Generic;
using PolyAndCode.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class RecycleRoll : MonoBehaviour
{

    public RectTransform board;
    [SerializeField] protected RectTransform subContent;
    [SerializeField] protected RectTransform content;
    [SerializeField] protected GridLayoutGroup gridLG;
    [SerializeField] protected ScrollRect scrollRect;
    [SerializeField] private ContentSizeFitter contentSizeFitter;
    [SerializeField] protected int maxSub;
    [SerializeField] protected int number;
    public List<GameObject> poolSub = new List<GameObject>();
    protected GameObject currentSub;
    protected int maxSubInBoard;
    protected int indexPerSub;
    protected int indexMaxSub;
    private Vector2 _currentPosi;
    private float offset = 0.95f;
    private bool _isDrag = true;
    private int temp;
    private float _currentPosRect;
    private float _prevPos;
    private float _maxSpace;
    private bool _isMoveTop;
    private int _tempMaxSub;
    private bool _isMaxSub;
    private float _limitMaxSub;
    private void Start()
    {
        poolSub.Clear();
        scrollRect.onValueChanged.AddListener(ScrollChangeValue);
        maxSubInBoard = Mathf.CeilToInt(board.rect.height / (gridLG.cellSize.y + gridLG.spacing.y));
        indexMaxSub = 2 * maxSubInBoard;
        temp = maxSubInBoard;
        _maxSpace = (maxSubInBoard / 2 - 1) * (gridLG.spacing.y + gridLG.cellSize.y);
        MakeListSub();
    }
    void ScrollChangeValue(Vector2 value)
    {
        //Debug.Log(scrollRect.verticalNormalizedPosition);

    }

    void MakeListSub()
    {

        content.sizeDelta = new Vector2(gridLG.cellSize.x, (gridLG.cellSize.y + gridLG.spacing.y) * indexMaxSub);
        for (int i = 0; i < 2 * maxSubInBoard; i++)
        {
            currentSub = Instantiate(subContent, content).gameObject;
            indexPerSub += 1;
            currentSub.GetComponent<SubMap>().countText.text = $"{indexPerSub}";
            poolSub.Add(currentSub);
            //SetUpCurrentSub();
        }
        scrollRect.verticalNormalizedPosition = 0;
        _prevPos = scrollRect.verticalNormalizedPosition;
        //contentSizeFitter.enabled = false;
    }
    public abstract void SetUpCurrentSub();
    private void Update()
    {
        if (_isMaxSub)
        {
            scrollRect.verticalNormalizedPosition = Mathf.Clamp(scrollRect.verticalNormalizedPosition, 0, _limitMaxSub);
        }
        var isGoingTop = scrollRect.verticalNormalizedPosition - _prevPos >= 0 ? true : false;
        if (isGoingTop)
        {
            MoveTop();
        }
        else
        {
            MoveBottom();
        }
        _prevPos = scrollRect.verticalNormalizedPosition;
        MoveTop();

    }
    void MoveTop()
    {
        if (scrollRect.verticalNormalizedPosition >= offset && indexPerSub < maxSub)
        {
            scrollRect.enabled = false;
            scrollRect.verticalNormalizedPosition = 0.2f;
            var temp = indexMaxSub / 2;
            indexPerSub -= (temp + 1) + (int)(temp * 0.2f);
            Debug.Log("dung");
            UpdateContent(true);
            scrollRect.enabled = true;
        }
    }
    void MoveBottom()
    {
        if (scrollRect.verticalNormalizedPosition <= 0.05f && indexPerSub > indexMaxSub)
        {
            scrollRect.enabled = false;
            scrollRect.verticalNormalizedPosition = 0.8f;
            var temp = indexMaxSub / 2;
            indexPerSub -= temp - 1;
            indexPerSub += (int)(temp * 0.2f);
            if (indexPerSub < maxSub)
            {
                _isMaxSub = false;
            }
            _tempMaxSub = indexPerSub;
            Debug.Log("tempmaxc" + _tempMaxSub);
            Debug.Log("dung");
            UpdateContent(false);
            scrollRect.enabled = true;
        }
    }
    void UpdateContent(bool _isTop)
    {
        if (_isTop)
        {
            for (int i = 0; i < poolSub.Count; i++)
            {
                indexPerSub += 1;
                if (indexPerSub == maxSub)
                {
                    Debug.Log("gia tri i"+ i);
                    _limitMaxSub = (float)(i+1) / indexMaxSub;
                    Debug.Log("limit " + _limitMaxSub);
                    _isMaxSub = true;
                }
                poolSub[i].GetComponent<SubMap>().countText.text = $"{indexPerSub}";
            }
        }
        else
        {
            for (int i = poolSub.Count - 1; i > -1; i--)
            {
                _tempMaxSub -= 1;
                poolSub[i].GetComponent<SubMap>().countText.text = $"{_tempMaxSub}";
            }
        }
    }

}
