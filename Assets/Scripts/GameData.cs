using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    /// <summary>
    /// 獲得済みの褒賞の登録用のクラス
    /// </summary>
    [System.Serializable]
    public class EarnedReward
    {
        public int rewardNo;     //褒賞の番号RewardDataのrewardNoと照合する
        public int rewardCount;  //褒賞の所持数
    }

    [Header("獲得している褒賞のリスト")]
    public List<EarnedReward> earnedRewardsList = new List<EarnedReward>();

    [Header("褒賞のポイントの合計値")]
    public int totalRewardPoint;

    private const string EARNED_REWARD_SAVE_KEY = "earnedRewardNo";

    private const string TOTAL_POINT_SAVE_KEY = "totalPoint";

    private int maxRewardDataCount;



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
    /// 褒賞ポイント計算
    /// </summary>
    /// <param name="amount"></param>
    public void CalculateTotalRewardPoint(int amount)
    {
        //褒賞ポイントを計算して合計値算出
        totalRewardPoint += amount;
    }

    /// <summary>
    /// 獲得した褒賞をリストに追加
    /// 既にリストにある場合には加算
    /// </summary>
    /// <param name="addRewardNo"></param>
    /// <param name="addRewardCount"></param>
    public void AddEarnedRewardsList(int addRewardNo,int addRewardCount = 1)
    {
        //すでにリストに登録がある褒賞か確認する
        if(earnedRewardsList.Exists(x => x.rewardNo == addRewardNo))
        {
            //登録がある場合には加算
            earnedRewardsList.Find(x => x.rewardNo == addRewardNo).rewardCount++;
        }
        else
        {
            //登録がない場合には新しく追加
            earnedRewardsList.Add(new EarnedReward { rewardNo = addRewardNo, rewardCount = addRewardCount });
        }
    }

    /// <summary>
    /// 獲得している褒賞のリストを削除
    /// 番号指定ありの場合には指定された褒賞の番号の情報を削除
    /// 番号指定なしの場合にはすべて削除
    /// </summary>
    /// <param name="rewardNo"></param>
    public void RemoveEarnedRewardList(int rewardNo = 999)
    {
        if(rewardNo == 999)
        {
            //すべての褒賞のデータを削除
            earnedRewardsList.Clear();
        }
        else
        {
            //指定された褒賞の番号のデータを削除
            earnedRewardsList.Remove(earnedRewardsList.Find(x => x.rewardNo == rewardNo));
        }
    }

    /// <summary>
    /// 獲得した褒賞のデータのセーブ
    /// </summary>
    /// <param name="rewardNo"></param>
    public void SaveEarnedReward(int rewardNo)
    {
        //EarnedReward earnedReward = earnedRewardsList.Find(x => x.rewardNo == rewardNo);
        PlayerPrefsHelper.SaveSetObjectData<EarnedReward>(EARNED_REWARD_SAVE_KEY + rewardNo, earnedRewardsList.Find(x => x.rewardNo == rewardNo));
        Debug.Log("獲得した褒賞をセーブ : rewardNo" + rewardNo + "を" + earnedRewardsList.Find(x => x.rewardNo == rewardNo).rewardCount + "体獲得");
    }
    
    /// <summary>
    /// 褒賞ポイントのロード
    /// </summary>
    public void LoadTotalRewardPoint()
    {
        totalRewardPoint = PlayerPrefsHelper.LoadIntData(TOTAL_POINT_SAVE_KEY);
    }

    /// <summary>
    /// 褒賞データの最大数の登録
    /// </summary>
    /// <param name="maxCount"></param>
    public void GetMaxRewardDataCount(int maxCount)
    {
        maxRewardDataCount = maxCount;
    }

    /// <summary>
    /// 獲得した褒賞データのロード
    /// </summary>
    /// <param name="rewardNo"></param>
    public void LoadEarnedReward()
    {
        //褒賞のデータベースに登録されているすべての褒賞のデータを１つずつ順番に確認
        for (int i = 0; i < maxRewardDataCount; i++)
        {
            //データベースにある褒賞のデータが、セーブされている褒賞のデータとして存在しているか確認
            if (PlayerPrefsHelper.ExistsData(EARNED_REWARD_SAVE_KEY + i))
            {
                //セーブデータがある場合のみロード
                EarnedReward earnedReward = PlayerPrefsHelper.LoadGetObjectData<EarnedReward>(EARNED_REWARD_SAVE_KEY + i);

                //リストに追加
                AddEarnedRewardsList(earnedReward.rewardNo, earnedReward.rewardCount);
            }
        }
    }

    /// <summary>
    /// 獲得している褒賞データの最大数の取得
    /// </summary>
    /// <returns></returns>
    public int GetEarnedRewardsListCount()
    {
        return earnedRewardsList.Count;
    }

    /// <summary>
    /// 褒賞ポイントのセーブ
    /// </summary>
    public void SaveTotalRewardPoint()
    {
        PlayerPrefsHelper.SaveIntData(TOTAL_POINT_SAVE_KEY, totalRewardPoint);
        Debug.Log("褒賞ポイントをセーブ：totalRewardPoint:" + GameData.instance.totalRewardPoint);
    }
}
