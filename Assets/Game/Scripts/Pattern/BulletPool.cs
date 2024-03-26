using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pancake;
using UnityEngine;
[Serializable]
public struct BulletData
{
    public EBulletType bulletType;
    public GameObject bullet;
}
public class BulletPool : MonoBehaviour
{
    [SerializeField] private List<BulletData> bulletDatas;
    [ReadOnly] public List<GameObject> _bulletPool = new List<GameObject>();
    public static BulletPool Instance;
    private void Awake()
    {
        Instance = this;
    }
    public GameObject GetBullet(EBulletType bulletType)
    {
        int count = 0;
        var getBulletType = _bulletPool.Where(b => b.GetComponent<IBullet>().BulletType == bulletType).ToList();
        foreach (var checkBullet in getBulletType)
        {
            var b = checkBullet.GetComponent<IBullet>();
            if (b.BulletType == bulletType)
            {
                if (!checkBullet.activeInHierarchy)
                {
                    return checkBullet;
                }
                else
                {
                    count++;
                }
            }
        }
        if (count == getBulletType.Count)
        {
            GameObject getBullet;
            SpawnBullet(bulletType, out getBullet);
            return getBullet;
        }
        return null;
    }
    void SpawnBullet(EBulletType bulletType, out GameObject bullet)
    {
        var getBulletToSpawn = bulletDatas.Find(t => t.bulletType == bulletType);
        var spawnBullet = Instantiate(getBulletToSpawn.bullet, transform);
        _bulletPool.Add(spawnBullet);
        bullet = spawnBullet;
    }
}
public enum EBulletType
{
    Normal,
    FireRocket,
    Light,
    IceRocket,
    Slash,
    Lazer,
}
