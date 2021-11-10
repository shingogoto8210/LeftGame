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
        //TODO お使いの時間データの確認とロード

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

            //TODO お使い開始時間のセーブ

            //お使いの準備とお使い開始
            tapPointDetail.PreparateJobs(tapPointDetail.jobData.jobTime);
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
}
