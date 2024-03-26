using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pancake;
using Unity.VisualScripting;
using UnityEngine;

public class Area : MonoBehaviour
{
    public EAreaType areaType;
    [SerializeField] private List<EnemySpawner> enemySpawners;
    [ReadOnly] [SerializeField] int totalEnemies;
    [SerializeField] private List<GameObject> gates;
    [SerializeField] Checkin checkinGate;
    public int _currentEnemy;
    private void Awake()
    {
        checkinGate.gameObject.SetActive(true);
        areaType = EAreaType.Waiting;
        foreach (var spawner in enemySpawners)
        {
            foreach (var wave in spawner.spawnerCustoms)
            {
                totalEnemies += wave.numberEnemies;
            }
        }
    }
    private void OnEnable()
    {
        Observer.CheckArea += UpdateArea;
    }
    private void OnDisable()
    {
        Observer.CheckArea -= UpdateArea;
    }
    void UpdateArea()
    {
        if (areaType == EAreaType.PLaying)
        {
            _currentEnemy += 1;
            if (_currentEnemy >= totalEnemies)
            {
                foreach (var gate in gates)
                {
                    gate.transform.DOLocalMoveX(gate.transform.localPosition.x + 50, 2).OnComplete((() =>
                    {
                        Destroy(gate);
                    }));
                }
                foreach (var spawner in enemySpawners)
                {
                    spawner.startSpawn = false;
                    Destroy(spawner.gameObject);
                }
                _currentEnemy = 0;
                areaType = EAreaType.Done;
                Observer.CheckWinLevel?.Invoke();
                checkinGate.gameObject.SetActive(false);
            }
        }
    }
    public void StartEntry()
    {
        foreach (var gate in gates)
        {
            gate.gameObject.SetActive(true);
        }
        foreach (var spawn in enemySpawners)
        {
            spawn.startSpawn = true;
        }
        areaType = EAreaType.PLaying;
    }
}
public enum EAreaType
{
    Waiting,
    PLaying,
    Done,
}
