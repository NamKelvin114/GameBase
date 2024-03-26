using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DG.Tweening;
using Pancake;
using UnityEngine;
using UnityEngine.UI;

public class PopupTask : BasePopup
{
    [SerializeField] private VerticalLayoutGroup content;
    [SerializeField] private int numberTasks;
    [SerializeField] private SubTask subTask;
    [SerializeField] private TaskData taskData;
    [SerializeField] ScrollRect scrollRect;
    [Pancake.ReadOnly] [SerializeField] private List<SubTask> listTasks = new List<SubTask>();
    private string[] _currentTasks;
    private bool _isClaimming;
    private float _timeToMove = 1;
    private Sequence _moveSequence;
    private Guid _guid;
    protected override void BeforeShow()
    {
        Observer.Claim += DoClaim;
        Data.SetTaskProcess(ETypeTask.CollectGold, 100);
        if (listTasks.Count == 0)
        {
            for (int i = 0; i < numberTasks; i++)
            {
                var insTask = Instantiate(subTask, content.transform);
                listTasks.Add(insTask);
            }
        }
        PopupManager.Instance.ShowPopup<PopupMoney>();
        base.BeforeShow();
    }
    protected override void BeforeHide()
    {
        Observer.Claim -= DoClaim;
        base.BeforeHide();
    }
    private void DoClaim(SubTask getSubTask)
    {
        if (!_isClaimming)
        {
            var offset = 1000;
            _moveSequence = DOTween.Sequence();
            content.enabled = false;
            var g = listTasks.Find(c => c == getSubTask);
            var currentPos = g.GetComponent<RectTransform>().localPosition;
            _moveSequence.Append(g.rectTransform().DOLocalMoveX(currentPos.x + offset, _timeToMove).SetEase(Ease.Linear).OnStart((() =>
            {
                scrollRect.vertical = false;
                _isClaimming = true;
                Data.IncreaseMoney(g.coinReward, g.claimBtn.GetComponent<RectTransform>().position);
            })).OnComplete((() =>
            {
                g.rectTransform().localPosition = new Vector3(currentPos.x - offset, currentPos.y, currentPos.z);
            }))).Append(g.rectTransform().DOLocalMoveX(currentPos.x, _timeToMove).SetEase(Ease.Linear)).OnComplete((() =>
            {
                scrollRect.vertical = true;
                _isClaimming = false;
                content.enabled = true;
            }));
        }
    }
    protected override void ShowContent()
    {
        if (Data.MaxCurrentTask == 0)
        {
            for (int i = 0; i < listTasks.Count; i++)
            {
                listTasks[i].Init(taskData.tasks[i].icon, taskData.tasks[i].taskTitle, taskData.tasks[i].taskType, taskData.tasks[i].coinReward,
                    taskData.tasks[i].idTask, taskData.tasks[i].numberRequired);
                Data.MaxCurrentTask = taskData.tasks[i].idTask;
            }
            SaveTask();
        }
        else
        {
            LoadListTask();
            for (int i = 0; i < listTasks.Count; i++)
            {
                var getTask = taskData.tasks.Find(t => t.idTask.ToString() == _currentTasks[i]);
                if (getTask != null)
                {
                    listTasks[i].Init(getTask.icon, getTask.taskTitle, getTask.taskType, getTask.coinReward,
                        getTask.idTask, getTask.numberRequired);
                }
            }
        }
        base.ShowContent();
    }
    void SaveTask()
    {
        Data.ListCurrentTask = "";
        for (int i = 0; i < listTasks.Count; i++)
        {
            Data.ListCurrentTask += $"{listTasks[i].id},";
        }
    }
    void LoadListTask()
    {
        _currentTasks = null;
        _currentTasks = Data.ListCurrentTask.Split(",");
    }
}
