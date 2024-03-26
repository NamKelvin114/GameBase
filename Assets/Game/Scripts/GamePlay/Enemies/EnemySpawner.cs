using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Random = System.Random;

public class EnemySpawner : MonoBehaviour
{
    public List<SpawnerCustom> spawnerCustoms;
    [SerializeField] private List<Transform> spawnPos;
    [SerializeField] private TextMeshProUGUI timeSpawnText;
    private float _timeSpawn;
    private int _index;
    private SpawnerCustom getWave;
    private int _nextCount = 1;
    private int _prevCount = 0;
    public bool startSpawn;
    private void Start()
    {
        SetUp();
    }
    void SetUp()
    {
        _nextCount = 1;
        _prevCount = 0;
        getWave = spawnerCustoms[_index];
        _timeSpawn = getWave.timeSpawn;
    }
    private void Update()
    {
        if (startSpawn)
        {
            DoSpawn();
        }
    }
    void DoSpawn()
    {
        if (_index < spawnerCustoms.Count)
        {
            _timeSpawn -= Time.deltaTime;
            if (_timeSpawn <= 0)
            {
                timeSpawnText.text = "Enemies are coming";
                if (_nextCount != _prevCount)
                {
                    var randomEnemy = Pancake.Random.Range(0, getWave.enemiesPref.Count);
                    var spawnEnemy = Instantiate(getWave.enemiesPref[randomEnemy].enemyPref, transform.position,quaternion.identity);
                    int randPos = Pancake.Random.Range(0, spawnPos.Count);
                    _prevCount = _nextCount;
                    spawnEnemy.transform.DOMove(spawnPos[randPos].transform.position, getWave.moveSpeed).OnComplete((() =>
                    {
                        spawnEnemy.GetComponent<BaseEnemy>().Init(getWave.enemiesPref[randomEnemy].EnemyStats);
                        if (_prevCount == getWave.numberEnemies)
                        {
                            _index++;
                            SetUp();
                        }
                        else
                        {
                            _nextCount++;
                        }
                    }));
                }
            }
            else
            {
                timeSpawnText.text = $"{(int)_timeSpawn + 1}";
            }

        }
    }
}
[Serializable]
public struct EnemyWave
{
    public EnemyStats EnemyStats;
    public GameObject enemyPref;
}
[Serializable]
public class SpawnerCustom
{
    [Header("CustomWave\n\n")]
    public ETypeSpawn typeSpawn;
    public int numberEnemies;
    public int timeSpawn;
    public List<EnemyWave> enemiesPref;
    public float moveSpeed;
}
public enum ETypeSpawn
{
    Enemy,
    Boss,
}
