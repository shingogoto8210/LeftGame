using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AlbumPopUp : MonoBehaviour
{
    private Vector3 closePos;

    [SerializeField]
    private Button btnClose;

    /// <summary>
    /// AlbumPopUpの設定と表示
    /// </summary>
    /// <param name="btnTran"></param>
    /// <param name="canvasTran"></param>
    public void SetUpAlbumPopUp(GameManager gameManager,Vector3 centerPos,Vector3 btnPos)
    {
        //ポップアップの位置をボタンの位置に設定
        transform.position = btnPos;

        //最後にポップアップを閉じる際に利用するために、現在の位置を保持
        closePos = transform.position;

        //ボタンにメソッドの登録
        btnClose.onClick.AddListener(OnClickCloseAlbumPopUp);

        //ゲームオブジェクトのサイズを０にして見えない状態にする
        transform.localScale = Vector3.zero;

        //Sequenceを初期化して利用でいる状態にする
        Sequence sequence = DOTween.Sequence();

        //ポップアップをボタンの位置から画面の中央（Canvas　ゲームオブジェクトの位置）に移動させつつ
        sequence.Append(transform.DOMove(centerPos, 0.3f).SetEase(Ease.Linear));

        //ポップアップを徐々に大きくしながら表示。指定したサイズになったら、元のポップアップの大きさに戻す
        sequence.Join(transform.DOScale(Vector2.one * 1.2f, 0.5f).SetEase(Ease.InBack)).OnComplete(() => { transform.DOScale(Vector2.one, 0.2f); });

        //TODO 獲得している褒賞の数だけサムネイル用のゲームオブジェクトの生成処理を繰り返す

        //TODO 獲得している褒賞用のゲームオブジェクト（サムネイル用）を生成 
        
        //TODO サムネイル用のゲームオブジェクトに利用する褒賞のデータを取得して設定
        
        //TODO 初期画像の設定
        
        //TODO 褒賞一覧のListに登録
    }

    private void OnClickCloseAlbumPopUp()
    {
        // Sequenceを初期化して利用できる状態にする
        Sequence sequence = DOTween.Sequence();

        //ポップアップの大きさを徐々に０にして見えない状態にさせつつ
        sequence.Append(transform.DOScale(Vector2.zero, 0.3f).SetEase(Ease.Linear));

        //それに合わせてポップアップをアルバムボタンの位置に移動させる。移動後にポップアップを破壊
        sequence.Join(transform.DOMove(closePos, 0.3f).SetEase(Ease.Linear)).OnComplete(() => Destroy(gameObject));

    }
}
