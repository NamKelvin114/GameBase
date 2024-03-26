using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pancake;
using TMPro;
using UnityEngine;
using Random = Pancake.Random;

public class PopupMoney : BasePopup
{
    [SerializeField] private RectTransform spawnMoney;
    [SerializeField] private int numberCoinToSpawn;
    [SerializeField] RectTransform moneyDestination;
    [SerializeField] private GameObject moneyPrefab;
    [SerializeField] private TextMeshProUGUI moneyText;
    private List<RectTransform> _moneySpawn = new List<RectTransform>();
    private int _increaseAmount;
    protected override void BeforeShow()
    {
        UpdateText();
        Observer.IncreaseCoin += DoInCreaseMoney;
        base.BeforeShow();
    }
    private void DoInCreaseMoney(int increaseMoney, Vector3 spawnPosi)
    {
        spawnMoney.position = spawnPosi;
        _increaseAmount = increaseMoney;
        SpawnMoney();
    }
    void SpawnMoney()
    {
        for (int i = 0; i < numberCoinToSpawn; i++)
        {
            var insMoney = Instantiate(moneyPrefab, spawnMoney);
            var posi = Random.InUnitCircle * 50;
            insMoney.rectTransform().DOLocalMove(posi, 0.2f).SetEase(Ease.Linear);
            _moneySpawn.Add(insMoney.rectTransform());
        }
        DOTween.Sequence().AppendInterval(1f).OnComplete((() => MoveMoney()));
    }
    void MoveMoney()
    {
        var amountPerunit = _increaseAmount / numberCoinToSpawn;
        int count = 0;
        // while (count < _moneySpawn.Count)
        // {
        //     _moneySpawn[count].DOMove(moneyDestination.position, 1).SetEase(Ease.Linear).OnComplete(() =>
        //     {
        //         Data.CurrentCoint += amountPerunit;
        //         UpdateText();
        //         count += 1;
        //     });
        // }
        spawnMoney.RemoveAllChildren();

    }
    void UpdateText()
    {
        moneyText.text = Data.CurrentCoint.ToString();
    }
    protected override void BeforeHide()
    {
        base.BeforeHide();
    }
}
