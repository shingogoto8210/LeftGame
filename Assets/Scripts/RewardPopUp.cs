using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class RewardPopUp : MonoBehaviour
{
    [SerializeField]
    private Button btnSubmit;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Image imgReward;

    [SerializeField]
    private Text txtRewardPoint;

    [SerializeField]
    private Text txtRarity;

    void Start()
    {
        //TODO ポップアップの設定と表示。外部のスクリプトから操作できるようになると不要になる。
        //SetUpRewardPopUp();
    }

    public void SetUpRewardPopUp(RewardData rewardData)  //TODO 外部のスクリプトから呼び出せる準備が整ったら，そのスクリプト側が送ってくる褒賞のデータが受けとれるように引数を追加する
    {
        //ポップアップを非表示にする
        canvasGroup.alpha = 0;

        //ポップアップを徐々に表示する
        canvasGroup.DOFade(1.0f, 0.5f).SetEase(Ease.Linear);

        //ボタンにメソッドの登録
        btnSubmit.onClick.AddListener(OnClickCloseRewardPopUp);

        // 褒賞のポイント表示
        txtRewardPoint.text = rewardData.rewardPoint.ToString();

        // 褒賞の希少度の表示
        txtRarity.text = rewardData.rarityType.ToString();

        // 褒賞の画像の設定
        imgReward.sprite = rewardData.spriteReward;
        
        // 表示の際の演出
    }

　　/// <summary>
  /// ポップアップ非表示
  /// </summary>
    private void OnClickCloseRewardPopUp()
    {
        //非表示になったらポップアップを破壊
        canvasGroup.DOFade(0, 0.5f).SetEase(Ease.Linear).OnComplete(() => Destroy(gameObject));
    }
}
