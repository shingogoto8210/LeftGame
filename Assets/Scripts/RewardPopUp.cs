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
        //TODO �|�b�v�A�b�v�̐ݒ�ƕ\���B�O���̃X�N���v�g���瑀��ł���悤�ɂȂ�ƕs�v�ɂȂ�B
        //SetUpRewardPopUp();
    }

    public void SetUpRewardPopUp(RewardData rewardData)  //TODO �O���̃X�N���v�g����Ăяo���鏀������������C���̃X�N���v�g���������Ă���J�܂̃f�[�^���󂯂Ƃ��悤�Ɉ�����ǉ�����
    {
        //�|�b�v�A�b�v���\���ɂ���
        canvasGroup.alpha = 0;

        //�|�b�v�A�b�v�����X�ɕ\������
        canvasGroup.DOFade(1.0f, 0.5f).SetEase(Ease.Linear);

        //�{�^���Ƀ��\�b�h�̓o�^
        btnSubmit.onClick.AddListener(OnClickCloseRewardPopUp);

        // �J�܂̃|�C���g�\��
        txtRewardPoint.text = rewardData.rewardPoint.ToString();

        // �J�܂̊󏭓x�̕\��
        txtRarity.text = rewardData.rarityType.ToString();

        // �J�܂̉摜�̐ݒ�
        imgReward.sprite = rewardData.spriteReward;
        
        // �\���̍ۂ̉��o
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
