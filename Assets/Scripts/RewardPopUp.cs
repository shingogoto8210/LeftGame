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

    void Start()
    {
        //TODO �|�b�v�A�b�v�̐ݒ�ƕ\���B�O���̃X�N���v�g���瑀��ł���悤�ɂȂ�ƕs�v�ɂȂ�B
        SetUpRewardPopUp();
    }

    public void SetUpRewardPopUp()  //TODO �O���̃X�N���v�g����Ăяo���鏀������������C���̃X�N���v�g���������Ă���J�܂̃f�[�^���󂯂Ƃ��悤�Ɉ�����ǉ�����
    {
        //�|�b�v�A�b�v���\���ɂ���
        canvasGroup.alpha = 0;

        //�|�b�v�A�b�v�����X�ɕ\������
        canvasGroup.DOFade(1.0f, 0.5f).SetEase(Ease.Linear);

        //�{�^���Ƀ��\�b�h�̓o�^
        btnSubmit.onClick.AddListener(OnClickCloseRewardPopUp);

        //TODO �J�܂̃|�C���g�\��

        //TODO �J�܂̊󏭓x�̕\��

        //TODO �J�܂̉摜�̐ݒ�

        //TODO �\���̍ۂ̉��o
    }

�@�@/// <summary>
  /// �|�b�v�A�b�v��\��
  /// </summary>
    private void OnClickCloseRewardPopUp()
    {
        //��\���ɂȂ�����|�b�v�A�b�v��j��
        canvasGroup.DOFade(0, 0.5f).SetEase(Ease.Linear).OnComplete(() => Destroy(gameObject));
    }
}
