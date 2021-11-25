using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardDetail : MonoBehaviour
{
    [SerializeField]
    private Image imgReward;

    [SerializeField]
    private RewardData rewardData;

    [SerializeField]
    private Button btnRewardDetail;

    private AlbumPopUp albumPopUp;

    /// <summary>
    /// RewardDetail�̐ݒ�
    /// </summary>
    /// <param name="gameManager"></param>
    /// <param name="earnedRewardNo"></param>
    public void SetUpRewardDetail(RewardData rewardData, AlbumPopUp albumPopUp)
    {
        this.rewardData = rewardData;
        this.albumPopUp = albumPopUp;

        imgReward.sprite = this.rewardData.spriteReward;

        btnRewardDetail.onClick.AddListener(OnClickRewardDetail);
    }

    /// <summary>
    /// btnRewardDetail���������ۂ̏���
    /// </summary>
    public void OnClickRewardDetail()
    {
        albumPopUp.DisplayReward(rewardData.spriteReward);
    }
}
