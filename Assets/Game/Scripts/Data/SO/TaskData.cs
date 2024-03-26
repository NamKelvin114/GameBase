using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(menuName = "Config/Task")]
public class TaskData : ScriptableObject
{
    public List<TaskInformation> tasks;
    public int firstCoinReward;
    public int increaseCoin;
#if UNITY_EDITOR
    [ContextMenu("UpdateTask")]
    private void UpdateListTask()
    {
        QuickSort(tasks,0,tasks.Count-1);
        int tempCoin = firstCoinReward;
        foreach (var task in tasks)
        {
            task.coinReward = tempCoin;
            tempCoin += increaseCoin;
        }
    }
  #endif
    void QuickSort(List<TaskInformation> task, int l, int r)
    {
        int p = task[(l+r)/2].idTask;
        int i = l, j = r;
        while (i < j){
            while (task[i].idTask < p){
                i++;
            }
            while (task[j].idTask > p){
                j--;
            }
            if (i <= j){
                TaskInformation temp = task[i];
                task[i] = task[j];
                task[j] = temp;
                i++;
                j--;
            }
        }
        if (i < r){
            QuickSort(task, i, r);
        }
        if (l < j){
            QuickSort(task, l, j);
        }
    }
}
[Serializable]
public class TaskInformation
{
    public int idTask;
    public int numberRequired;
    public int coinReward;
    public Sprite icon;
    public string taskTitle;
    public ETypeTask taskType;
}
public enum ETypeTask
{
    CollectGold,
    KillEnemy,
    PlayGame,
}

