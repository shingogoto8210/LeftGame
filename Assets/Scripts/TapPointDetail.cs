using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TapPointDetail : MonoBehaviour
{
    [SerializeField]
    private Button btnTapPoint;  //Buttonコンポーネントを制御するための変数

    [SerializeField]
    private Transform canvasTran;

    [SerializeField]
    private JobsComfirmPopUp jobsComfirmPopUpPrefab;

    private JobsComfirmPopUp jobsComfirmPopUp;

    [SerializeField,Header("この行き先のお使い番号")]
    private int myjobNo;

    public JobData jobData;  //お使いの情報を登録

    void Start()
    {

        //ボタンを押した際に実行する処理（メソッド）を引数にして登録
        btnTapPoint.onClick.AddListener(OnClickTapPoint);
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
        if(jobsComfirmPopUp == null)
        {
            //行先決定用のポップアップ表示
            //Debug.Log("TapPoint 行先決定用のポップアップ表示");
            jobsComfirmPopUp = Instantiate(jobsComfirmPopUpPrefab, canvasTran, false);
            jobsComfirmPopUp.OpenPopUp(this);
        }
        //2回目移行は，SetActiveをオンにして表示する
        else
        {
            jobsComfirmPopUp.OpenPopUp(this);
        }
        
    }
}
