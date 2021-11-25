using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<TapPointDetail> tapPointDetailsList = new List<TapPointDetail>();   //ゲーム内にある行き先を束ねて管理するための変数

    [SerializeField]
    private Transform canvasTran;                                                    //行き先確認用ポップアップなどの生成位置の指定の変数。TapPointDetailクラスより移管

    [SerializeField]
    private JobsConfirmPopUp jobsConfirmPopUpPrefab;　　　　　　　　　　　　　　　　 //行き先確認ポップアップのプレファブをアサインする変数。TapPointDetailクラスより移管

    [SerializeField]
    private CharaDetail charaDetailPrefab;　　　　　　　　　　　　　　　　　　　　　 //お使い完了時に生成するキャラのプレファブをアサインする変数。TapPointDetailクラスより移管

    [SerializeField]
    private List<CharaDetail> charaDetailsList = new List<CharaDetail>();      //お使い完了のキャラを束ねて管理するための変数

    [SerializeField]
    private　RewardPopUp rewardPopUpPrefab;  //褒賞表示用のポップアップのプレファブをアサインする変数。

    [SerializeField]
    private EventDataSO rewardDataSO;

    [SerializeField]
    private JobTypeRewardRatesDataSO jobTypeRewardRatesDataSO;

    [SerializeField]
    private UnityEngine.UI.Button btnAlbum;

    [SerializeField]
    private AlbumPopUp albumPopUpPrefab;

    private AlbumPopUp albumPopUp;

    void Start()
    {
        // 褒賞データの最大数をGameDataクラスに登録
        GameData.instance.GetMaxRewardDataCount(rewardDataSO.rewardDatasList.Count);

        // 獲得している褒賞データの確認とロード
        GameData.instance.LoadEarnedReward();

        // 獲得している褒賞データがある場合
        if (GameData.instance.GetEarnedRewardsListCount() > 0)
        {
            // 褒賞ポイントのロード
            GameData.instance.LoadTotalRewardPoint();
        }
        
        // お使いの時間データの確認とロード
        OfflineTimeManager.instance.GetWorkingJobTimeDatasList(tapPointDetailsList);
        
        //各TapPointDetailの設定
        TapPointSetUp();

        btnAlbum.onClick.AddListener(OnClickAlbum);
    }
    
    /// <summary>
    /// 各TapPointDetailの設定。お使いの状況に合わせて、仕事中か仕事終了かを判断してキャラを生成するか、お使いを再開するか決定
    /// </summary>
    private void TapPointSetUp()
    {
        //Listに登録されているすべてのTapPointDetailクラスに対して一回ずつ処理を行う
        for(int i = 0; i < tapPointDetailsList.Count; i++)
        {

            //TapPointDetailクラスの設定を行う
            tapPointDetailsList[i].SetUpTapPointDetail(this);

            //各TapPointDetailに登録されているJobDataに該当するJobTimeDataを取得
            OfflineTimeManager.JobTimeData jobTime = OfflineTimeManager.instance.workingJobTimeDatasList.Find((x) => x.jobNo == tapPointDetailsList[i].jobData.jobNo);

            //この行き先がお使い中でなければ次の処理へ移る
            if(jobTime == null)
            {
                continue;
            }

            //Debug.Log(jobTime.elaspedJobTime);

            //お使いの状態と残り時間を取得
            (bool isJobEnd, int remainingTime) = JudgeJobsEnd(jobTime);

            Debug.Log("WorkingJobNo"+jobTime.jobNo+"の残り時間"+remainingTime);

            //ロードした時間とセーブした時間を計算して、まだお使いの時間が経過していない場合には、③を実行する。お使いが完了している場合には①と②を実行する
            if (isJobEnd)
            {
                //①お使い完了。お使いのリストとセーブデータを削除　キャラをタップしてから消す
                //OfflineTimeManager.instance.RemoveWorkingJobTimeDatasList(jobTime.jobNo);

                //②キャラ生成して結果を確認
                GenerateCharaDetail(tapPointDetailsList[i]);

            }
            else
            {
                //③お使い未了。行き先の画像をお使い中の画像に変更して、お使い処理を再開。
                JudgeSubmitJob(true, tapPointDetailsList[i], remainingTime);
            }
        }
    }

    /// <summary>
    /// TapPointをクリックした際にお使い確認用のポップアップを開く。TapPointDetailクラスより移管
    /// </summary>
    /// <param name="tapPointDetail"></param>
    public void GenerateJobsConfirmPopUp(TapPointDetail tapPointDetail)
    {

        //お使い確認用のポップアップを作成
        JobsConfirmPopUp jobsConfirmPouUp = Instantiate(jobsConfirmPopUpPrefab, canvasTran, false);

        //ポップアップのメソッドを実行する。メソッドの引数を利用してポップアップにTapPointDetailクラスの情報を送ることで、JobDataの情報を活用できるようにする
        jobsConfirmPouUp.OpenPopUp(tapPointDetail,this);
    }

    /// <summary>
    /// お使いを引き受けたか確認
    /// </summary>
    /// <param name="isSubmit"></param>
    /// <param name="tapPointDetail"></param>
    /// <param name="remainingTime"></param>
    public void JudgeSubmitJob(bool isSubmit,TapPointDetail tapPointDetail, int remainingTime = -1)
    {
        if (isSubmit)
        {

            // お使いの登録
            OfflineTimeManager.instance.CreateWorkingJobTimeDatasList(tapPointDetail,remainingTime);

            //お使いの準備とお使いの開始
            if(remainingTime == -1)
            {
                //いままでおつかいしていないなら、お使いごとの初期値のお使いの時間を設定
                tapPointDetail.PreparateJobs(tapPointDetail.jobData.jobTime);
            }
            else
            {
                //お使いの途中の場合には、残りのお使いの時間を設定
                tapPointDetail.PreparateJobs(remainingTime);
            }

            // お使い開始時間のセーブ
            OfflineTimeManager.instance.SaveWorkingJobTimeData(tapPointDetail.jobData.jobNo);
        }
        else
        {
            Debug.Log("お使いには行かない");
        }
    }

    /// <summary>
    /// キャラ生成
    /// </summary>
    /// <param name="tapPointDetail"></param>
    public void GenerateCharaDetail(TapPointDetail tapPointDetail)
    {
        Debug.Log("お使い用のキャラの生成");

        CharaDetail chara = Instantiate(charaDetailPrefab, tapPointDetail.transform,false);

        //キャラの設定
        chara.SetUpCharaDetail(this, tapPointDetail);
    }

    private(bool,int) JudgeJobsEnd(OfflineTimeManager.JobTimeData jobTimeData)
    {
        //ゲーム起動時の時間とお使いをセーブした時間との差分値を算出
        int elaspedTime = OfflineTimeManager.instance.CalculateOfflineDateTimeElasped(jobTimeData.GetDateTime()) * 100;
        Debug.Log("お使い時間の差分：" + elaspedTime / 100 + "：秒");

        //残り時間算出
        int remainingTime = jobTimeData.elaspedJobTime;
        Debug.Log("remainingTime:" + remainingTime);

        //経過時間がお使いにかかる時間よりも同じか多いなら
        if(remainingTime <= elaspedTime)
        {
            //お使い完了
            return (true, 0);
        }
        //お使い未了。残り時間から経過時間を減算して残り時間にする
        return (false, remainingTime - elaspedTime);
    }

    /// <summary>
    /// お使いの成果発表
    /// キャラをタップすると呼び出す
    /// </summary>
    /// <param name="tapPointDetail"></param>
    public void ResultJobs(TapPointDetail tapPointDetail)
    {
        Debug.Log("成果　発表");

        //お使いの難しさから褒賞決定
        RewardData rewardData = GetLotteryForRewards(tapPointDetail.jobData.jobType);

        Debug.Log("決定した褒賞の通し番号：" + rewardData.rewardNo);

        // 褒賞ポイントを計算
        GameData.instance.CalculateTotalRewardPoint(rewardData.rewardPoint);

        // 獲得した褒賞を獲得済みリストに登録。すでにある場合には所持数を加算
        GameData.instance.AddEarnedRewardsList(rewardData.rewardNo);

        // 獲得した褒賞のセーブ
        GameData.instance.SaveEarnedReward(rewardData.rewardNo);
        
        // 褒賞ポイントのセーブ
        GameData.instance.SaveTotalRewardPoint();

        //褒賞表示用のポップアップ生成
        //褒賞のデータを作成したら、RewardPopUpスクリプトの制御を行うため，変数に代入する処理に変更する
        RewardPopUp rewardPopUp = Instantiate(rewardPopUpPrefab, canvasTran, false);

        // 褒賞のデータを作成したら、RewardPopUpスクリプトのメソッドを実行して、褒賞のデータを引数で渡す
        rewardPopUp.SetUpRewardPopUp(rewardData);
        //Instantiate(rewardPopUpPrefab,canvasTran,false).SetUpRewardPopUp(rewardData)でもOK

        // お使いのリストとお使いのセーブデータを削除。キャラをタップしてから消す
        OfflineTimeManager.instance.RemoveWorkingJobTimeDatasList(tapPointDetail.jobData.jobNo);
    }

    /// <summary>
    /// お使いの褒賞の抽選
    /// </summary>
    /// <param name="jobType"></param>
    /// <returns></returns>
    private RewardData GetLotteryForRewards(JobType jobType)
    {

        //難易度による希少度の合計値を算出して、ランダムな値を抽出
        int randomRarityValue = UnityEngine.Random.Range(0, jobTypeRewardRatesDataSO.jobTypeRewardRatesDatasList[(int)jobType].rewardRates.Sum());

        Debug.Log("今回のお使いの難易度：" + jobType + "/難易度による希少度の合計値：" + jobTypeRewardRatesDataSO.jobTypeRewardRatesDatasList[(int)jobType].rewardRates.Sum());
        Debug.Log("希少度を決定するためのランダムな値：" + randomRarityValue);

        //抽選用の初期値を設定
        RarityType rarityType = RarityType.Common;
        int total = 0;

        //抽出した値がどの希少度になるか確認
        for (int i = 0; i < jobTypeRewardRatesDataSO.jobTypeRewardRatesDatasList.Count; i++)
        {
            //希少度の重みづけを行うために加算
            total += jobTypeRewardRatesDataSO.jobTypeRewardRatesDatasList[(int)jobType].rewardRates[i];
            Debug.Log("希少度を決定するためのランダムな値：" + randomRarityValue + "<=" + "希少度の重み付の合計値：" + total); ;
            
        //totalの値がどの希少度に該当するかを順番に確認
        if(randomRarityValue <= total)
            {
                //希少度を決定
                rarityType = (RarityType)i;
                break;
            }
        }

        Debug.Log("今回の希少度：" + rarityType);

        //今回対象となる希少度のデータだけのリストを作成
        List<RewardData> rewardDatas = new List<RewardData>(rewardDataSO.rewardDatasList.Where(x => x.rarityType == rarityType).ToList());

        //同じ希少度の褒賞の提供割合の値の合計値を算出して、ランダムな値を抽出
        int randomRewardValue = UnityEngine.Random.Range(0, rewardDatas.Select(x => x.rarityRate).ToArray().Sum());

        Debug.Log("希少度内の褒賞用のランダムな値：" + randomRewardValue);

        total = 0;

        //抽出した値がどの褒賞になるか確認
        for (int i = 0; i < rewardDatas.Count; i++)
        {
            //totalの値が褒賞に該当するまで加算
            total += rewardDatas[i].rarityRate;
            Debug.Log("希少度内の褒賞用のランダムな値：" + randomRewardValue + "<=" + "褒賞の重みづけの合計値："+total);

            if(randomRewardValue <= total)
            {
                //褒賞確定
                return rewardDatas[i];
            }
        }
        return null;
    }

    //アルバムボタンを押した際の動作
    private void OnClickAlbum()
    {

        //アルバムポップアップがまだ生成されていなければ
        if (albumPopUp == null)
        {
            //アルバムボタンをアニメ演出する
            btnAlbum.transform.DOPunchScale(Vector3.one * 1.1f, 0.1f).SetEase(Ease.InOutQuart);

            //アルバムポップアップを生成する。変数に代入することにより、ボタンの重複タップによる複数のアルバムポップアップの生成を防止する
            albumPopUp = Instantiate(albumPopUpPrefab, canvasTran, false);

            //生成したアルバムポップアップの設定を行うためのメソッドを実行し、必要な情報を引数で渡す
            albumPopUp.SetUpAlbumPopUp(this, canvasTran.position, btnAlbum.transform.position);
        }
    }

    /// <summary>
    /// RewardNoからRewardDataを取得
    /// </summary>
    /// <param name="rewardNo"></param>
    /// <returns></returns>
    public RewardData GetRewardDataFromRewardNo(int rewardNo)
    {
        return rewardDataSO.rewardDatasList.Find(x => x.rewardNo == rewardNo);
    }
}
