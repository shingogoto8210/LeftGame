using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RewardDataList",menuName ="Create RewardDataList")]
public class EventDataSO : ScriptableObject
{
    public List<RewardData> rewardDatasList = new List<RewardData>();
}
