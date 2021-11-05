using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JobsComfirmPopUp : MonoBehaviour
{
    [SerializeField]
    private Button btnSubmit;

    [SerializeField]
    private Button btnCancel;

    [SerializeField]
    private CanvasGroup canvasGroup;

    void Start()
    {
        //ボタンをすべて非活性化にする
        SwitchButtons(false);

        //ボタンにメソッドを登録
        btnSubmit.onClick.AddListener(OnClickSubmit);
        btnCancel.onClick.AddListener(OnClickCancel);

        canvasGroup.alpha = 0.0f;

        //ポップアップを表示して，ボタンを活性化する
        canvasGroup.DOFade(1.0f, 0.3f)
        .SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            SwitchButtons(true);
        });
    }

    /// <summary>
    /// 決定
    /// </summary>
    private void OnClickSubmit()
    {
        ClosePopUp(true);

        Debug.Log("お使いに行く");
    }

    /// <summary>
    /// キャンセル
    /// </summary>
    private void OnClickCancel()
    {
        ClosePopUp(true);

        Debug.Log("お使いには行かない");
    }

    /// <summary>
    /// ポップアップを表示する
    /// </summary>
    public void OpenPopUp()
    {

        gameObject.SetActive(true);

        //ボタンをすべて非活性化にする
        SwitchButtons(false);

        canvasGroup.alpha = 0.0f;

        //ポップアップを表示して，ボタンを活性化する
        canvasGroup.DOFade(1.0f, 0.3f)
        .SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            SwitchButtons(true);
        });
    }
    
    /// <summary>
    /// ポップアップを閉じる
    /// </summary>
    /// <param name="isSubmit"></param>
    private void ClosePopUp(bool isSubmit)
    {
        SwitchButtons(false);

        canvasGroup.DOFade(0f, 0.3f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                //TODO ボタン操作の結果を反映する

                //Destroy(gameObject);
                gameObject.SetActive(false);
            });
    }

    /// <summary>
    /// すべてのボタンの活性化/非活性化の制御
    /// </summary>
    /// <param name="isSwitch"></param>
    private void SwitchButtons(bool isSwitch)
    {
        btnSubmit.interactable = isSwitch;
        btnCancel.interactable = isSwitch;
    }
}
