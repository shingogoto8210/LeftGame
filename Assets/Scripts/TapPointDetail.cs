using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TapPointDetail : MonoBehaviour
{
    [SerializeField]
    private Button btnTapPoint;     //Buttonコンポーネントを制御するための変数

    private JobsConfirmPopUp jobsComfirmPopUp;

    [SerializeField,Header("この行き先のお使い番号")]
    private int myjobNo;

    public JobData jobData;        //お使いの情報を登録

    [SerializeField]
    private Image imgTapPoint;     //行き先の画像を変更するためのコンポーネントの代入

    [SerializeField]
    private Sprite jobSprite;      //お使い中の画像の登録用

    [SerializeField]
    private Sprite defaultSprite;  //初期の行き先の画像の登録用

    [SerializeField]
    private Tween tween;           //DOTweenの処理を代入するための変数

    [SerializeField]
    private int currentJobTime;    //お使いをしている時間の計測用

    private bool isJobs;           //お使い中かどうかを判定する値，trueならばお使い中として利用する

    /// <summary>
    /// isJobs変数のプロパティ
    /// </summary>
    public bool IsJobs
    {
        set
        {
            isJobs = value;
        }
        get
        {
            return isJobs;
        }
    }

    private GameManager gameManager;

    /// <summary>
    /// TapPointDetailの設定
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpTapPointDetail(GameManager gameManager)
    {
        //ボタンを押した際に実行する処理（メソッド）を引数に指定して登録
        btnTapPoint.onClick.AddListener(OnClickTapPoint);

        this.gameManager = gameManager;

    }

    public void SetUpTapPointFromJobDataSO( List<JobData> jobDatasList)
    {
        jobData = jobDatasList.Find(x => x.jobNo == myjobNo);

    }

    /// <summary>
    /// タップポイントをタップした際の処理
    /// </summary>
    private void OnClickTapPoint()
    {

        //Debug.Log("TapPoint タップ");

        //タップアニメ演出
        //Debug.Log("TapPoint タップアニメ演出");

        //DOTweenの機能の１つであるDOpunchScaleメソッドを利用してアニメ演出を追加
        transform.DOPunchScale(Vector3.one * 1.25f, 0.15f).SetEase(Ease.OutBounce);

        //ポップアップがまだ生成されていないとき
        //if(jobsComfirmPopUp == null)
        //{
        //行先決定用のポップアップ表示
        //Debug.Log("TapPoint 行先決定用のポップアップ表示");
        //jobsComfirmPopUp = Instantiate(jobsComfirmPopUpPrefab, canvasTran, false);
        //jobsComfirmPopUp.OpenPopUp(this);
        //}
        //2回目移行は，SetActiveをオンにして表示する
        //else
        //{
        //jobsComfirmPopUp.OpenPopUp(this);
        //}

        //GameManagerクラスにある行き先確認ポップアップを生成するメソッドを実行する
        gameManager.GenerateJobsConfirmPopUp(this);
    }

    /// <summary>
    /// お使いの準備
    /// </summary>
    public void PreparateJobs(int remainingTime)
    {
        ChangeJobSprite();
        IsJobs = true;
        StartCoroutine(WorkingJobs(remainingTime));
    }

    /// <summary>
    /// お使い中の画像に変更
    /// </summary>
    public void ChangeJobSprite()
    {
        imgTapPoint.sprite = jobSprite;

        ChangeActiveButton(false);

        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        tween = transform.DOPunchPosition(new Vector3(10.0f, 0, 0), 3.0f, 10, 3)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    /// <summary>
    /// お使いの開始。時間経過処理
    /// </summary>
    /// <param name="normaJobTime"></param>
    /// <returns></returns>
    public IEnumerator WorkingJobs(int normaJobTime)
    {

        //残っているお使いの時間を設定
        currentJobTime = normaJobTime;

        //お使いが終わるかを確認
        while (IsJobs)
        {
            //TODO 条件として時間を確認する
            currentJobTime--;

            OfflineTimeManager.instance.UpdateCurrentJobTime(jobData.jobNo, currentJobTime);

            //Debug.Log(currentJobTime);

            //残り時間が0以下になったら
            if(currentJobTime <= 0)
            {
                
                KillTween();
                IsJobs = false;
                ChangeActiveButton(true);
                //OfflineTimeManager.instance.RemoveWorkingJobTimeDatasList(jobData.jobNo);
            }

            yield return null;
        }

        //お使いに関する情報を初期状態に戻す
        ReturnDefaultState();

        //仕事終了
        Debug.Log("お使い終了");

        //キャラ生成
        //GenerateCharaDetail();

        //GameManagerクラスにあるキャラを生成するメソッドを実行する
        gameManager.GenerateCharaDetail(this);
    }

    /// <summary>
    /// Tweenを破壊
    /// </summary>
    public void KillTween()
    {
        tween.Kill();
        //Debug.Log("tweenを止める");
    }

    /// <summary>
    /// お使いに関する情報を初期状態に戻す
    /// </summary>
    public void ReturnDefaultState()
    {

        //お使い中の画像を元の画像に戻す
        imgTapPoint.sprite = defaultSprite;

        //お使いの時間をリセット
        currentJobTime = 0;

        //オブジェクトのサイズを初期値に戻す
        transform.localScale = Vector3.one;
    }

    //private void GenerateCharaDetail()
    //{
      //  Instantiate(charaDetailPrefab,transform, false);
    //}

    private void ChangeActiveButton(bool isActive)
    {
        btnTapPoint.enabled = isActive;
    }
}
