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

   
    void Start()
    {
        //TODO 褒賞関連の処理

        // お使いの時間データの確認とロード
        OfflineTimeManager.instance.GetWorkingJobTimeDatasList(tapPointDetailsList);
        
        //各TapPointDetailの設定
        TapPointSetUp();
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

            //お使いの状態と残り時間を取得
            (bool isJobEnd, int remainingTime) = JudgeJobsEnd(jobTime);

            //ロードした時間とセーブした時間を計算して、まだお使いの時間が経過していない場合には、③を実行する。お使いが完了している場合には①と②を実行する
            if (isJobEnd)
            {
                //①お使い完了。お使いのリストとセーブデータを削除　キャラをタップしてから消す
                OfflineTimeManager.instance.RemoveWorkingJobTimeDatasList(jobTime.jobNo);

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
            OfflineTimeManager.instance.CreateWorkingJobTimeDatasList(tapPointDetail);

            // お使い開始時間のセーブ
            OfflineTimeManager.instance.SaveWorkingJobTimeData(tapPointDetail.jobData.jobNo);

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
        //ゲーム起動時の時間とお使いを開始した時間との差分値を算出
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
}
