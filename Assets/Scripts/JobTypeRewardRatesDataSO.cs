using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���g���̎�ށi��Փx�j�ɂ��J�܂̒񋟊����f�[�^�x�[�X
/// </summary>
[CreateAssetMenu(fileName = "JobTypeRewardRatesDataSO", menuName = "Create JobTypeRewardRatesDataSO")]
public class JobTypeRewardRatesDataSO : ScriptableObject
{
    public List<JobTypeRewardRatesData> jobTypeRewardRatesDatasList = new List<JobTypeRewardRatesData>();
}
