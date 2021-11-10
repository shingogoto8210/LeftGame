using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OfflineTimeManager : MonoBehaviour
{
    public static OfflineTimeManager instance;   　//シングルトン用の変数

    public DateTime loadDateTime = new DateTime(); //前回ゲームを止めた時にセーブしている時間

    private int elaspedTime;                       //経過時間

    private const string SAVE_KEY_DATETIME = "OfflineDateTime";  //時間をセーブ・ロードする際の変数。定数として宣言する

    private const string FORMAT = "yyyy/MM//dd HH:mm::ss";       //日時のフォーマット

    private const string WORKING_JOB_SAVE_KEY = "workingJobNo_"; //お使い時間のデータをセーブ・ロードするための変数

    private GameManager gameManager;

    /// <summary>
    /// お使い用の時間データを管理するためのクラス
    /// </summary>
    [Serializable]
    public class JobTimeData
    {
        public int jobNo;            //お使いの通し番号
        public int elaspedJobTime;   //お使いの残り時間
        public string jobTimeString; //DateTimeクラスを文字列にするための変数
        
        /// <summary>
        /// DateTimeを文字列で保存しているので、DateTime型に戻して取得
        /// </summary>
        /// <returns></returns>
        public DateTime GetDateTime()
        {
            return DateTime.ParseExact(jobTimeString, FORMAT, null);
        }
    }

    [Header("お使いの時間データのリスト")]
    public List<JobTimeData> workingJobTimeDatasList = new List<JobTimeData>();

    private void Awake()
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

        //時間のセーブデータの確認とロード
        LoadOfflineDateTime();

        //オフラインでの経過時間を計算
        CalculateOfflineDateTimeElasped(loadDateTime);

        //TODO お使いのデータのロード
    }

    /// <summary>
    /// ゲームが終了したときに自動的に呼ばれる
    /// </summary>
    private void OnApplicationQuit()
    {

        //現在の時間のセーブ
        SaveOfflineDateTime();

        Debug.Log("ゲーム中断。時間のセーブ完了");
    }

    /// <summary>
    /// オフラインでの時間をロード
    /// </summary>
    public void LoadOfflineDateTime()
    {

        //セーブデータがあるか確認
        if (PlayerPrefsHelper.ExistsData(SAVE_KEY_DATETIME))
        {

            //セーブデータがある場合、ロードする
            string oldDateTimeString = PlayerPrefsHelper.LoadStringData(SAVE_KEY_DATETIME);

            //ロードした文字列をDateTime型に変換して時間を取得
            loadDateTime = DateTime.ParseExact(oldDateTimeString, FORMAT, null);

            Debug.Log("ゲーム開始時：セーブされていた時間：" + oldDateTimeString);

            Debug.Log("今の時間：" + DateTime.Now.ToString(FORMAT));
        }
        else
        {
            //セーブデータがない場合、現在の時間を開始時刻として取得しておく
            loadDateTime = DateTime.Now;

            Debug.Log("セーブデータがないので今の時間を取得：" + loadDateTime.ToString(FORMAT));
        }
    }
    /// <summary>
    /// 現在の時間をセーブ
    /// </summary>
    private void SaveOfflineDateTime()
    {

        //現在の時間を取得して，文字列に変換
        string dateTimeString = DateTime.Now.ToString(FORMAT);

        //string型でセーブ
        PlayerPrefsHelper.SaveStringData(SAVE_KEY_DATETIME, dateTimeString);

        Debug.Log("ゲーム終了時：セーブ時間：" + dateTimeString);
    }

    /// <summary>
    /// オフラインでの経過時間を計算
    /// </summary>
    /// <param name="oldDateTime"></param>
    /// <returns></returns>
    public int CalculateOfflineDateTimeElasped(DateTime oldDateTime)
    {
        //現在の時間を取得
        DateTime currentDateTime = DateTime.Now;

        //現在の時間とセーブされている時間を確認
        if(oldDateTime > currentDateTime)
        {

            //セーブデータの時間の方が今の時間よりも進んでいる場合には、今の時間を入れなおす
            oldDateTime = DateTime.Now;
        }

        //経過した時間の差分
        TimeSpan dateTImeElasped = currentDateTime - oldDateTime;

        //経過時間を秒にする（Math.Round メソッドを利用して、double型をint型に変換。小数点は０のくらいで，数値の丸め処理の指定はToEven（数値が２つの数値の中間に位置するときに、最も近い偶然の値）を指定）
        elaspedTime = (int)Math.Round(dateTImeElasped.TotalSeconds, 0, MidpointRounding.ToEven);

        Debug.Log($"オフラインでの経過時間：{elaspedTime }秒");

        return elaspedTime;
    }

    /// <summary>
    /// 各お使いの残り時間の更新
    /// </summary>
    /// <param name="jobNo"></param>
    /// <param name="currentJobTime"></param>
    public void UpdateCurrentJobTime(int jobNo, int currentJobTime)
    {

        //Listから該当のJobTimeDataを検索して取得し、elaspedJobTimeの値をcurrentJobTimeに更新
        workingJobTimeDatasList.Find(x => x.jobNo == jobNo).elaspedJobTime = currentJobTime;
    }

    /// <summary>
    /// ListにJobTimeを追加。このリストにある情報が現在お使いをしている内容になる
    /// </summary>
    /// <param name="jobTimeData"></param>
    public void AddWorkingJobTimeDatasList(JobTimeData jobTimeData)
    {

        //お使いをListに追加する前に、すでにリストにあるか確認して重複登録を防ぐ
        if(!workingJobTimeDatasList.Exists(x => x.jobNo == jobTimeData.jobNo))
        {

            //Listにない場合のみ、新しく追加する
            workingJobTimeDatasList.Add(jobTimeData);

            Debug.Log(jobTimeData.elaspedJobTime);
        }
    }

    /// <summary>
    /// 現在お使い中のJobTimeDataの作成とListへの追加
    /// </summary>
    /// <param name="tapPointDetail"></param>
    public void CreateWorkingJobTimeDatasList(TapPointDetail tapPointDetail)
    {

        //JobTimeDataをインスタンスして初期化
        JobTimeData jobTimeData = new JobTimeData { jobNo = tapPointDetail.jobData.jobNo, elaspedJobTime = tapPointDetail.jobData.jobTime };

        //ListにJobTimeDataを追加
        AddWorkingJobTimeDatasList(jobTimeData);
    }
}
