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

    [SerializeField]
    private Image imgReward;

    [SerializeField]
    private RewardDetail rewardDetailPrefab;

    [SerializeField]
    private Transform rewardDetailTran;

    [SerializeField]
    private List<RewardDetail> rewardDetailList = new List<RewardDetail>();

    /// <summary>
    /// AlbumPopUp�̐ݒ�ƕ\��
    /// </summary>
    /// <param name="btnTran"></param>
    /// <param name="canvasTran"></param>
    public void SetUpAlbumPopUp(GameManager gameManager,Vector3 centerPos,Vector3 btnPos)
    {
        //�|�b�v�A�b�v�̈ʒu���{�^���̈ʒu�ɐݒ�
        transform.position = btnPos;

        //�Ō�Ƀ|�b�v�A�b�v�����ۂɗ��p���邽�߂ɁA���݂̈ʒu��ێ�
        closePos = transform.position;

        //�{�^���Ƀ��\�b�h�̓o�^
        btnClose.onClick.AddListener(OnClickCloseAlbumPopUp);

        //�Q�[���I�u�W�F�N�g�̃T�C�Y���O�ɂ��Č����Ȃ���Ԃɂ���
        transform.localScale = Vector3.zero;

        //Sequence�����������ė��p�ł����Ԃɂ���
        Sequence sequence = DOTween.Sequence();

        //�|�b�v�A�b�v���{�^���̈ʒu�����ʂ̒����iCanvas�@�Q�[���I�u�W�F�N�g�̈ʒu�j�Ɉړ�������
        sequence.Append(transform.DOMove(centerPos, 0.3f).SetEase(Ease.Linear));

        //�|�b�v�A�b�v�����X�ɑ傫�����Ȃ���\���B�w�肵���T�C�Y�ɂȂ�����A���̃|�b�v�A�b�v�̑傫���ɖ߂�
        sequence.Join(transform.DOScale(Vector2.one * 1.2f, 0.5f).SetEase(Ease.InBack)).OnComplete(() => { transform.DOScale(Vector2.one, 0.2f); });

        // �l�����Ă���J�܂̐������T���l�C���p�̃Q�[���I�u�W�F�N�g�̐����������J��Ԃ�
        for (int i = 0; i < GameData.instance.GetEarnedRewardsListCount(); i++)
        {
            // �l�����Ă���J�ܗp�̃Q�[���I�u�W�F�N�g�i�T���l�C���p�j�𐶐� 
            RewardDetail rewardDetail = Instantiate(rewardDetailPrefab, rewardDetailTran, false);


            // �T���l�C���p�̃Q�[���I�u�W�F�N�g�ɗ��p����J�܂̃f�[�^���擾���Đݒ�
            rewardDetail.SetUpRewardDetail(gameManager.GetRewardDataFromRewardNo(i), this);
            
            // �����摜�̐ݒ�
            if (rewardDetailList.Count == 0)
            {
                imgReward.sprite = gameManager.GetRewardDataFromRewardNo(i).spriteReward;
            }

            // �J�܈ꗗ��List�ɓo�^
            rewardDetailList.Add(rewardDetail);

        }
    }

    private void OnClickCloseAlbumPopUp()
    {
        // Sequence�����������ė��p�ł����Ԃɂ���
        Sequence sequence = DOTween.Sequence();

        //�|�b�v�A�b�v�̑傫�������X�ɂO�ɂ��Č����Ȃ���Ԃɂ�����
        sequence.Append(transform.DOScale(Vector2.zero, 0.3f).SetEase(Ease.Linear));

        //����ɍ��킹�ă|�b�v�A�b�v���A���o���{�^���̈ʒu�Ɉړ�������B�ړ���Ƀ|�b�v�A�b�v��j��
        sequence.Join(transform.DOMove(closePos, 0.3f).SetEase(Ease.Linear)).OnComplete(() => Destroy(gameObject));
    }

    /// <summary>
    /// �A���o���ꗗ�őI�����ꂽ�J�܂̉摜���|�b�v�A�b�v�ɕ\��
    /// </summary>
    /// <param name="spriteReward"></param>
    public void DisplayReward(Sprite spriteReward)
    {
        imgReward.sprite = spriteReward;
    }
}
