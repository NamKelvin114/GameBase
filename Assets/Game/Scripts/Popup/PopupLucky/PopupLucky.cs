using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pancake;
using UnityEngine;
using UnityEngine.UI;

public class PopupLucky : BasePopup
{
    [SerializeField] private RectTransform board;
    [SerializeField] private RectTransform slot;
    [SerializeField] private Transform spawnSlot;
    [SerializeField] private int offset;
    [SerializeField] private List<SlotLucky> slots;
    [SerializeField] private LuckySpinData luckySpinData;
    private List<int> _stepEnds = new List<int>();
    private Color _currentColor;
    private int _rowRun = 1;
    private int _indexSpin = 0;
    private int _slotx;
    private int _sloty;
    private int _coutToChangeSpeed;
    private bool _isRollBack;
    private float _speed = 0.02f;
    private int _slotIndexToGet;
    private SlotLucky _slotFinal;
    private int _finalStep = 5;
    private float _minSpeed = 2;
    private int _temp;
    private bool _isDone;
    private bool _isReadyToEnd;
    private int _countStepEnd;
    private bool _isDoingSpin;
    protected override void BeforeShow()
    {
        _slotFinal = new SlotLucky();
        _currentColor = slot.gameObject.GetComponent<Image>().color;
        _slotx = (int)board.rect.width / (int)(slot.rect.width + offset);
        _sloty = (int)board.rect.height / (int)(slot.rect.height + offset);
        float reduncex = (((board.rect.width - (_slotx * (slot.rect.width + offset))) + offset) / 2);
        float reduncey = (((board.rect.height - (_sloty * (slot.rect.height + offset))) + offset) / 2);
        spawnSlot.localPosition = new Vector3(spawnSlot.localPosition.x + reduncex, spawnSlot.localPosition.y - reduncey, spawnSlot.localPosition.z);
        DoSpawnSlot();
        Init();
        base.BeforeShow();
    }
    void DoSpawnSlot()
    {
        for (int i = 0; i < _sloty; i++)
        {
            if (i == 0 || i == _sloty - 1)
            {
                for (int j = 0; j < _slotx; j++)
                {
                    Spawn(i, j);
                }
            }
            else
            {
                for (int j = 0; j < _slotx; j += _slotx - 1)
                {
                    Spawn(i, j);
                }
            }
        }
    }
    void Init()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].gameObject.GetComponent<SlotLucky>().Init(luckySpinData.SlotLuckyDatas[i]);
        }
    }
    void Spawn(int i, int j)
    {
        var getSlot = Instantiate(slot, spawnSlot).gameObject.GetComponent<SlotLucky>();
        var getPos = getSlot.rectTransform().localPosition;
        var getOffset = getSlot.rectTransform().rect;
        getSlot.rectTransform().localPosition = new Vector3(getPos.x + ((getOffset.width + offset) * j), getPos.y -
            ((getOffset.height + offset) * i), getPos.z);
        slots.Add(getSlot);
    }
    public void DoSpinLucky()
    {
        if (!_isDoingSpin)
        {
            DeFault();
            DoRandomSlot();
            _countStepEnd = _stepEnds.Count - 1;
            Spin();
        }
    }
    void DeFault()
    {
        _slotFinal = new SlotLucky();
        _isDone = false;
        _indexSpin = 0;
        _isRollBack = false;
        _rowRun = 1;
        _slotIndexToGet = 0;
        _stepEnds.Clear();
        _isReadyToEnd = false;
        _speed = 0.02f;
        _countStepEnd = 0;
        _coutToChangeSpeed = 0;
    }
    void DoRandomSlot()
    {
        var rand = Pancake.Random.Range(0, 101);
        foreach (var dataSlot in luckySpinData.SlotLuckyDatas)
        {
            if (dataSlot.minRatio <= rand && rand <= dataSlot.maxRatio)
            {
                _slotFinal.data = dataSlot;
                Debug.Log("1");
                break;
            }
        }
        if (_slotFinal.data != null)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].data == _slotFinal.data)
                {
                    _slotIndexToGet = i;
                    _temp = _slotIndexToGet;
                    //stepEnds.Add(_slotIndexToGet);
                    break;
                }
            }
            for (int i = 0; i < _finalStep; i++)
            {
                DoSetStepEnd();
            }
        }
    }
    void DoSetStepEnd()
    {
        if (slots.Count - _slotx <= _temp && _temp <= slots.Count - 1)
        {
            _temp += 1;
            _stepEnds.Add(_temp);
            if (_temp == slots.Count - 1)
            {
                _temp = slots.Count - (_slotx + 1);
            }
        }
        if (_slotx <= _temp && _temp <= slots.Count - _slotx)
        {
            var _isRight = _temp - _slotx % 2 == 0 ? true : false;
            if (_isRight)
            {
                _temp -= 2;
                _stepEnds.Add(_temp);
            }
            else
            {
                _temp += 2;
                _stepEnds.Add(_temp);
            }
        }
        if (0 <= _temp && _temp < _slotx)
        {
            _temp -= 1;
            if (_temp < 0)
            {
                _temp = _slotx;
            }
            _stepEnds.Add(_temp);
        }
    }

    void Spin()
    {
        if (!_isDone)
        {
            Debug.Log("2");
            _isDoingSpin = true;
            if (_indexSpin == 0)
            {
                _coutToChangeSpeed += 1;
            }
            if (_isReadyToEnd)
            {
                if (_indexSpin == _slotIndexToGet)
                {
                    _isDone = true;
                    _isDoingSpin = false;
                }
            }
            if (_coutToChangeSpeed >= _finalStep)
            {
                if (_countStepEnd > -1)
                {
                    if (_indexSpin == _stepEnds[_countStepEnd])
                    {
                        _isReadyToEnd = true;
                        _speed += (_minSpeed - _speed) / _stepEnds.Count;
                        _countStepEnd -= 1;
                    }
                }
            }
            foreach (var setSlot in slots)
            {
                if (setSlot != slots[_indexSpin])
                {
                    setSlot.GetComponent<Image>().color = _currentColor;
                }
            }
            slots[_indexSpin].GetComponent<Image>().color = new Color32(255, 110, 110, 255);
            DOTween.Sequence().AppendInterval(_speed).OnComplete((() =>
            {
                if (_rowRun == 1)
                {
                    if (_indexSpin == _slotx - 1)
                    {
                        _rowRun += 1;
                        _indexSpin += 2;
                        _isRollBack = false;
                    }
                    else
                    {
                        _indexSpin += 1;
                    }
                    Spin();
                    return;
                }
                if (_rowRun == _sloty)
                {
                    if (_indexSpin == slots.Count - _slotx)
                    {
                        _rowRun -= 1;
                        _indexSpin -= 2;
                        _isRollBack = true;
                    }
                    else
                    {
                        _indexSpin -= 1;
                    }
                    Spin();
                    return;
                }
                if (_isRollBack)
                {
                    _rowRun -= 1;
                    if (_rowRun == 1)
                    {
                        _indexSpin = 0;
                    }
                    else
                    {
                        _indexSpin -= 2;
                    }
                }
                else
                {
                    _rowRun += 1;
                    if (_rowRun == _sloty)
                    {
                        _indexSpin += _slotx;
                    }
                    else
                    {
                        _indexSpin += 2;
                    }
                }
                Spin();
            }));
        }
    }
}
