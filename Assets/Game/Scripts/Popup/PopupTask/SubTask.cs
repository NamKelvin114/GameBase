using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubTask : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI taskTitle;
    [SerializeField] private Image processFill;
    [SerializeField] private TextMeshProUGUI textProcess;
    [SerializeField] private ETypeTask taskType;
    [SerializeField] public GameObject claimBtn;
    [SerializeField] private GameObject goBtn;
    public int id { get; set; }
    public int coinReward { get; set; }
    private int _numberRequired;
    public void Init(Sprite getIcon, string getTaskTitle, ETypeTask getTaskType, int getCoinReward, int getId, int getNumberRequired)
    {
        icon.sprite = getIcon;
        taskTitle.text = getTaskTitle;
        taskType = getTaskType;
        coinReward = getCoinReward;
        id = getId;
        _numberRequired = getNumberRequired;
        CheckProcess();
    }
    public void CheckProcess()
    {
        textProcess.text = $"{Data.GetTaskProcess(taskType)}/{_numberRequired}";
        var getProcess = (float)Data.GetTaskProcess(taskType) / _numberRequired;
        processFill.fillAmount = getProcess;
        if (getProcess >= 1)
        {
            claimBtn.gameObject.SetActive(true);
            goBtn.gameObject.SetActive(false);
        }
        else
        {
            claimBtn.gameObject.SetActive(false);
            goBtn.gameObject.SetActive(true);
        }
    }
    public void Claim()
    {
        Observer.Claim?.Invoke(this);
    }
}
