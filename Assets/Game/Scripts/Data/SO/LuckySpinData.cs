using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(menuName = "Config/LuckySpin")]
public class LuckySpinData : ScriptableObject
{
    public List<SlotLuckyData> SlotLuckyDatas;
}
[Serializable]
public class SlotLuckyData
{
    public TypeLuckySpin typeSlot;
    public int minRatio;
    public int maxRatio;
    public Sprite icon;
    public int numberReward;
    public void Reset()
    {
        typeSlot = TypeLuckySpin.Gold;
        minRatio = 0;
        maxRatio = 0;
        icon = null;
        numberReward = 0;
    }
}
public enum TypeLuckySpin
{
    Gold
}
#if UNITY_EDITOR
[CustomEditor(typeof(LuckySpinData), true)]
[CanEditMultipleObjects]
[Serializable]
public class LuckySpinDataEditor : Editor
{
    private LuckySpinData _luckySpinData;
    private void OnEnable()
    {
        _luckySpinData = target as LuckySpinData;
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        if (GUILayout.Button("ResetData", GUILayout.MinWidth(100), GUILayout.MinHeight(50)))
        {
            foreach (var slotSpin in _luckySpinData.SlotLuckyDatas)
            {
                slotSpin.Reset();
            }
        }
        base.OnInspectorGUI();
    }
}
#endif
