using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���g���̃f�[�^
/// </summary>
[System.Serializable]
public class JobData
{
    [Tooltip("���g���̒ʂ��ԍ�")]
    public int jobNo;

    [Tooltip("���g���ɂ����鎞��")]
    public int jobTime;

    [Tooltip("���g���̖��O")]
    public string jobTitle;

    [Tooltip("���g���̓�Փx")]
    public JobType jobType;
}
