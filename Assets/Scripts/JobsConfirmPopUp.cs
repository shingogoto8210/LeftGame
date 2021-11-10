using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JobsConfirmPopUp : MonoBehaviour
{
    [SerializeField]
    private Button btnSubmit;

    [SerializeField]
    private Button btnCancel;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Text txtJobTime;

    [SerializeField]
    private Text txtJobType;

    [SerializeField]
    private Text txtJobTitle;

    private TapPointDetail tapPointDetail;

    private GameManager gameManager;

    /// <summary>
    /// �|�b�v�A�b�v��\������
    /// </summary>
    public void OpenPopUp(TapPointDetail tapPointDetail, GameManager gameManager)
    {
        this.gameManager = gameManager;

        gameObject.SetActive(true);

        //�{�^�������ׂĔ񊈐����ɂ���
        SwitchButtons(false);

        //�{�^���Ƀ��\�b�h��o�^
        btnSubmit.onClick.AddListener(OnClickSubmit);
        btnCancel.onClick.AddListener(OnClickCancel);

        canvasGroup.alpha = 0.0f;

        this.tapPointDetail = tapPointDetail;

        txtJobTime.text = (tapPointDetail.jobData.jobTime / 60).ToString("F0");
        txtJobType.text = tapPointDetail.jobData.jobType.ToString();
        txtJobTitle.text = tapPointDetail.jobData.jobTitle;

        //�|�b�v�A�b�v��\�����āC�{�^��������������
        canvasGroup.DOFade(1.0f, 0.3f)
        .SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            SwitchButtons(true);
        });
    }

    /// <summary>
    /// ����
    /// </summary>
    private void OnClickSubmit()
    {
        ClosePopUp(true);

        Debug.Log("���g���ɍs��");
    }

    /// <summary>
    /// �L�����Z��
    /// </summary>
    private void OnClickCancel()
    {
        ClosePopUp(false);

        Debug.Log("���g���ɂ͍s���Ȃ�");
    }

   
    
    /// <summary>
    /// �|�b�v�A�b�v�����
    /// </summary>
    /// <param name="isSubmit"></param>
    private void ClosePopUp(bool isSubmit)
    {
        SwitchButtons(false);

        canvasGroup.DOFade(0f, 0.3f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                //TODO �{�^������̌��ʂ𔽉f����
                //if (isSubmit)
                //{
                    //���g���̏����J�n
                  //  tapPointDetail.PreparateJobs();
                //}

                //GameManager�N���X�̂��g���̈����󂯉ۂ̑I�����������s����
                gameManager.JudgeSubmitJob(isSubmit, tapPointDetail);

                //Destroy(gameObject);
                gameObject.SetActive(false);
            });
    }

    /// <summary>
    /// ���ׂẴ{�^���̊�����/�񊈐����̐���
    /// </summary>
    /// <param name="isSwitch"></param>
    private void SwitchButtons(bool isSwitch)
    {
        btnSubmit.interactable = isSwitch;
        btnCancel.interactable = isSwitch;
    }
}
