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
    /// �L�����̐ݒ�
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
    /// �L�������^�b�v�����ۂ̏���
    /// </summary>
    private void OnClickChara()
    {

        //���g�����ʂ����U���g�\��
        gameManager.ResultJobs(tapPointDetail);

        Debug.Log("���g���̌��ʂ�\��");

        Destroy(gameObject);
    }
}
