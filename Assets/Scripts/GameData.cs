using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    /// <summary>
    /// �l���ς݂̖J�܂̓o�^�p�̃N���X
    /// </summary>
    [System.Serializable]
    public class EarnedReward
    {
        public int rewardNo;     //�J�܂̔ԍ�RewardData��rewardNo�Əƍ�����
        public int rewardCount;  //�J�܂̏�����
    }

    [Header("�l�����Ă���J�܂̃��X�g")]
    public List<EarnedReward> earnedRewardsList = new List<EarnedReward>();

    [Header("�J�܂̃|�C���g�̍��v�l")]
    public int totalRewardPoint;

    private const string EARNED_REWARD_SAVE_KEY = "earnedRewardNo";

    private const string TOTAL_POINT_SAVE_KEY = "totalPoint";



    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �J�܃|�C���g�v�Z
    /// </summary>
    /// <param name="amount"></param>
    public void CalculateTotalRewardPoint(int amount)
    {
        //�J�܃|�C���g���v�Z���č��v�l�Z�o
        totalRewardPoint += amount;
    }

    /// <summary>
    /// �l�������J�܂����X�g�ɒǉ�
    /// ���Ƀ��X�g�ɂ���ꍇ�ɂ͉��Z
    /// </summary>
    /// <param name="addRewardNo"></param>
    /// <param name="addRewardCount"></param>
    public void AddEarnedRewardsList(int addRewardNo,int addRewardCount = 1)
    {
        //���łɃ��X�g�ɓo�^������J�܂��m�F����
        if(earnedRewardsList.Exists(x => x.rewardNo == addRewardNo))
        {
            //�o�^������ꍇ�ɂ͉��Z
            earnedRewardsList.Find(x => x.rewardNo == addRewardNo).rewardCount++;
        }
        else
        {
            //�o�^���Ȃ��ꍇ�ɂ͐V�����ǉ�
            earnedRewardsList.Add(new EarnedReward { rewardNo = addRewardNo, rewardCount = addRewardCount });
        }
    }

    /// <summary>
    /// �l�����Ă���J�܂̃��X�g���폜
    /// �ԍ��w�肠��̏ꍇ�ɂ͎w�肳�ꂽ�J�܂̔ԍ��̏����폜
    /// �ԍ��w��Ȃ��̏ꍇ�ɂ͂��ׂč폜
    /// </summary>
    /// <param name="rewardNo"></param>
    public void RemoveEarnedRewardList(int rewardNo = 999)
    {
        if(rewardNo == 999)
        {
            //���ׂĂ̖J�܂̃f�[�^���폜
            earnedRewardsList.Clear();
        }
        else
        {
            //�w�肳�ꂽ�J�܂̔ԍ��̃f�[�^���폜
            earnedRewardsList.Remove(earnedRewardsList.Find(x => x.rewardNo == rewardNo));
        }
    }

    /// <summary>
    /// �l�������J�܂̃f�[�^�̃Z�[�u
    /// </summary>
    /// <param name="rewardNo"></param>
    public void SaveEarnedReward(int rewardNo)
    {
        //EarnedReward earnedReward = earnedRewardsList.Find(x => x.rewardNo == rewardNo);
        PlayerPrefsHelper.SaveSetObjectData<EarnedReward>(EARNED_REWARD_SAVE_KEY + rewardNo,earnedRewardsList.Find(x => x.rewardNo == rewardNo));
        Debug.Log("�l�������J�܂��Z�[�u : rewardNo" + rewardNo +"��"+ earnedRewardsList.Find(x => x.rewardNo == rewardNo).rewardCount + "�̊l��" );

    }

    /// <summary>
    /// �J�܃|�C���g�̃Z�[�u
    /// </summary>
    public void SaveTotalPoint()
    {
        PlayerPrefsHelper.SaveIntData(TOTAL_POINT_SAVE_KEY, totalRewardPoint);
        Debug.Log("�J�܃|�C���g���Z�[�u�FtotalRewardPoint:" + GameData.instance.totalRewardPoint);
    }
}
