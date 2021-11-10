using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharaDetail : MonoBehaviour
{
    [SerializeField]
    private Button btnChara;

    private GameManager gameManager;

    private TapPointDetail tapPointDetail;

    /// <summary>
    /// キャラの設定
    /// </summary>
    public void SetUpCharaDetail(GameManager gameManager, TapPointDetail tapPointDetail)
    {
        this.gameManager = gameManager;
        this.tapPointDetail = tapPointDetail;

        btnChara.interactable = false;

        btnChara.onClick.AddListener(OnClickChara);

        btnChara.interactable = true;
    }

    /// <summary>
    /// キャラをタップした際の処理
    /// </summary>
    private void OnClickChara()
    {

        //TODO お使い結果をリザルト表示

        Debug.Log("お使いの結果を表示");

        Destroy(gameObject);
    }
}
