using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<TapPointDetail> tapPointDetailsList = new List<TapPointDetail>();   //�Q�[�����ɂ���s����𑩂˂ĊǗ����邽�߂̕ϐ�

    [SerializeField]
    private Transform canvasTran;                                                    //�s����m�F�p�|�b�v�A�b�v�Ȃǂ̐����ʒu�̎w��̕ϐ��BTapPointDetail�N���X���ڊ�

    [SerializeField]
    private JobsConfirmPopUp jobsConfirmPopUpPrefab;�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@ //�s����m�F�|�b�v�A�b�v�̃v���t�@�u���A�T�C������ϐ��BTapPointDetail�N���X���ڊ�

    [SerializeField]
    private CharaDetail charaDetailPrefab;�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@ //���g���������ɐ�������L�����̃v���t�@�u���A�T�C������ϐ��BTapPointDetail�N���X���ڊ�

    [SerializeField]
    private List<CharaDetail> charaDetailsList = new List<CharaDetail>();      //���g�������̃L�����𑩂˂ĊǗ����邽�߂̕ϐ�

    [SerializeField]
    private�@RewardPopUp rewardPopUpPrefab;  //�J�ܕ\���p�̃|�b�v�A�b�v�̃v���t�@�u���A�T�C������ϐ��B

    [SerializeField]
    private EventDataSO rewardDataSO;

    [SerializeField]
    private JobTypeRewardRatesDataSO jobTypeRewardRatesDataSO;
    void Start()
    {
        //TODO �J�܊֘A�̏���

        // ���g���̎��ԃf�[�^�̊m�F�ƃ��[�h
        OfflineTimeManager.instance.GetWorkingJobTimeDatasList(tapPointDetailsList);
        
        //�eTapPointDetail�̐ݒ�
        TapPointSetUp();
    }
    
    /// <summary>
    /// �eTapPointDetail�̐ݒ�B���g���̏󋵂ɍ��킹�āA�d�������d���I�����𔻒f���ăL�����𐶐����邩�A���g�����ĊJ���邩����
    /// </summary>
    private void TapPointSetUp()
    {
        //List�ɓo�^����Ă��邷�ׂĂ�TapPointDetail�N���X�ɑ΂��Ĉ�񂸂������s��
        for(int i = 0; i < tapPointDetailsList.Count; i++)
        {

            //TapPointDetail�N���X�̐ݒ���s��
            tapPointDetailsList[i].SetUpTapPointDetail(this);

            //�eTapPointDetail�ɓo�^����Ă���JobData�ɊY������JobTimeData���擾
            OfflineTimeManager.JobTimeData jobTime = OfflineTimeManager.instance.workingJobTimeDatasList.Find((x) => x.jobNo == tapPointDetailsList[i].jobData.jobNo);

            //���̍s���悪���g�����łȂ���Ύ��̏����ֈڂ�
            if(jobTime == null)
            {
                continue;
            }

            //Debug.Log(jobTime.elaspedJobTime);

            //���g���̏�ԂƎc�莞�Ԃ��擾
            (bool isJobEnd, int remainingTime) = JudgeJobsEnd(jobTime);

            Debug.Log("WorkingJobNo"+jobTime.jobNo+"�̎c�莞��"+remainingTime);

            //���[�h�������ԂƃZ�[�u�������Ԃ��v�Z���āA�܂����g���̎��Ԃ��o�߂��Ă��Ȃ��ꍇ�ɂ́A�B�����s����B���g�����������Ă���ꍇ�ɂ͇@�ƇA�����s����
            if (isJobEnd)
            {
                //�@���g�������B���g���̃��X�g�ƃZ�[�u�f�[�^���폜�@�L�������^�b�v���Ă������
                OfflineTimeManager.instance.RemoveWorkingJobTimeDatasList(jobTime.jobNo);

                //�A�L�����������Č��ʂ��m�F
                GenerateCharaDetail(tapPointDetailsList[i]);

            }
            else
            {
                //�B���g�������B�s����̉摜�����g�����̉摜�ɕύX���āA���g���������ĊJ�B
                JudgeSubmitJob(true, tapPointDetailsList[i], remainingTime);
            }
        }
    }

    /// <summary>
    /// TapPoint���N���b�N�����ۂɂ��g���m�F�p�̃|�b�v�A�b�v���J���BTapPointDetail�N���X���ڊ�
    /// </summary>
    /// <param name="tapPointDetail"></param>
    public void GenerateJobsConfirmPopUp(TapPointDetail tapPointDetail)
    {

        //���g���m�F�p�̃|�b�v�A�b�v���쐬
        JobsConfirmPopUp jobsConfirmPouUp = Instantiate(jobsConfirmPopUpPrefab, canvasTran, false);

        //�|�b�v�A�b�v�̃��\�b�h�����s����B���\�b�h�̈����𗘗p���ă|�b�v�A�b�v��TapPointDetail�N���X�̏��𑗂邱�ƂŁAJobData�̏������p�ł���悤�ɂ���
        jobsConfirmPouUp.OpenPopUp(tapPointDetail,this);
    }

    /// <summary>
    /// ���g���������󂯂����m�F
    /// </summary>
    /// <param name="isSubmit"></param>
    /// <param name="tapPointDetail"></param>
    /// <param name="remainingTime"></param>
    public void JudgeSubmitJob(bool isSubmit,TapPointDetail tapPointDetail, int remainingTime = -1)
    {
        if (isSubmit)
        {

            // ���g���̓o�^
            OfflineTimeManager.instance.CreateWorkingJobTimeDatasList(tapPointDetail,remainingTime);

            //���g���̏����Ƃ��g���̊J�n
            if(remainingTime == -1)
            {
                //���܂܂ł��������Ă��Ȃ��Ȃ�A���g�����Ƃ̏����l�̂��g���̎��Ԃ�ݒ�
                tapPointDetail.PreparateJobs(tapPointDetail.jobData.jobTime);
            }
            else
            {
                //���g���̓r���̏ꍇ�ɂ́A�c��̂��g���̎��Ԃ�ݒ�
                tapPointDetail.PreparateJobs(remainingTime);
            }

            // ���g���J�n���Ԃ̃Z�[�u
            OfflineTimeManager.instance.SaveWorkingJobTimeData(tapPointDetail.jobData.jobNo);
        }
        else
        {
            Debug.Log("���g���ɂ͍s���Ȃ�");
        }
    }

    /// <summary>
    /// �L��������
    /// </summary>
    /// <param name="tapPointDetail"></param>
    public void GenerateCharaDetail(TapPointDetail tapPointDetail)
    {
        Debug.Log("���g���p�̃L�����̐���");

        CharaDetail chara = Instantiate(charaDetailPrefab, tapPointDetail.transform,false);

        //�L�����̐ݒ�
        chara.SetUpCharaDetail(this, tapPointDetail);
    }

    private(bool,int) JudgeJobsEnd(OfflineTimeManager.JobTimeData jobTimeData)
    {
        //�Q�[���N�����̎��ԂƂ��g�����Z�[�u�������ԂƂ̍����l���Z�o
        int elaspedTime = OfflineTimeManager.instance.CalculateOfflineDateTimeElasped(jobTimeData.GetDateTime()) * 100;
        Debug.Log("���g�����Ԃ̍����F" + elaspedTime / 100 + "�F�b");

        //�c�莞�ԎZ�o
        int remainingTime = jobTimeData.elaspedJobTime;
        Debug.Log("remainingTime:" + remainingTime);

        //�o�ߎ��Ԃ����g���ɂ����鎞�Ԃ��������������Ȃ�
        if(remainingTime <= elaspedTime)
        {
            //���g������
            return (true, 0);
        }
        //���g�������B�c�莞�Ԃ���o�ߎ��Ԃ����Z���Ďc�莞�Ԃɂ���
        return (false, remainingTime - elaspedTime);
    }

    /// <summary>
    /// ���g���̐��ʔ��\
    /// �L�������^�b�v����ƌĂяo��
    /// </summary>
    /// <param name="tapPointDetail"></param>
    public void ResultJobs(TapPointDetail tapPointDetail)
    {
        Debug.Log("���ʁ@���\");

        //���g���̓������J�܌���
        RewardData rewardData = GetLotteryForRewards(tapPointDetail.jobData.jobType);

        Debug.Log("���肵���J�܂̒ʂ��ԍ��F" + rewardData.rewardNo);

        //TODO �J�܃|�C���g���v�Z

        //TODO �l�������J�܂��l���ς݃��X�g�ɓo�^�B���łɂ���ꍇ�ɂ͏����������Z

        //TODO �l�������J�܂̃Z�[�u

        //TODO �J�܃|�C���g�̃Z�[�u

        //�J�ܕ\���p�̃|�b�v�A�b�v����
        //TODO �J�܂̃f�[�^���쐬������ARewardPopUp�X�N���v�g�̐�����s�����߁C�ϐ��ɑ�����鏈���ɕύX����
        Instantiate(rewardPopUpPrefab, canvasTran, false);

        //TODO �J�܂̃f�[�^���쐬������ARewardPopUp�X�N���v�g�̃��\�b�h�����s���āA�J�܂̃f�[�^�������œn��

        //TODO ���g���̃��X�g�Ƃ��g���̃Z�[�u�f�[�^���폜�B�L�������^�b�v���Ă������
    }

    /// <summary>
    /// ���g���̖J�܂̒��I
    /// </summary>
    /// <param name="jobType"></param>
    /// <returns></returns>
    private RewardData GetLotteryForRewards(JobType jobType)
    {

        //��Փx�ɂ��󏭓x�̍��v�l���Z�o���āA�����_���Ȓl�𒊏o
        int randomRarityValue = UnityEngine.Random.Range(0, jobTypeRewardRatesDataSO.jobTypeRewardRatesDatasList[(int)jobType].rewardRates.Sum());

        Debug.Log("����̂��g���̓�Փx�F" + jobType + "/��Փx�ɂ��󏭓x�̍��v�l�F" + jobTypeRewardRatesDataSO.jobTypeRewardRatesDatasList[(int)jobType].rewardRates.Sum());
        Debug.Log("�󏭓x�����肷�邽�߂̃����_���Ȓl�F" + randomRarityValue);

        //���I�p�̏����l��ݒ�
        RarityType rarityType = RarityType.Common;
        int total = 0;

        //���o�����l���ǂ̊󏭓x�ɂȂ邩�m�F
        for (int i = 0; i < jobTypeRewardRatesDataSO.jobTypeRewardRatesDatasList.Count; i++)
        {
            //�󏭓x�̏d�݂Â����s�����߂ɉ��Z
            total += jobTypeRewardRatesDataSO.jobTypeRewardRatesDatasList[(int)jobType].rewardRates[i];
            Debug.Log("�󏭓x�����肷�邽�߂̃����_���Ȓl�F" + randomRarityValue + "<=" + "�󏭓x�̏d�ݕt�̍��v�l�F" + total); ;
            
        //total�̒l���ǂ̊󏭓x�ɊY�����邩�����ԂɊm�F
        if(randomRarityValue <= total)
            {
                //�󏭓x������
                rarityType = (RarityType)i;
                break;
            }
        }

        Debug.Log("����̊󏭓x�F" + rarityType);

        //����ΏۂƂȂ�󏭓x�̃f�[�^�����̃��X�g���쐬
        List<RewardData> rewardDatas = new List<RewardData>(rewardDataSO.rewardDatasList.Where(x => x.rarityType == rarityType).ToList());

        //�����󏭓x�̖J�܂̒񋟊����̒l�̍��v�l���Z�o���āA�����_���Ȓl�𒊏o
        int randomRewardValue = UnityEngine.Random.Range(0, rewardDatas.Select(x => x.rarityRate).ToArray().Sum());

        Debug.Log("�󏭓x���̖J�ܗp�̃����_���Ȓl�F" + randomRewardValue);

        total = 0;

        //���o�����l���ǂ̖J�܂ɂȂ邩�m�F
        for (int i = 0; i < rewardDatas.Count; i++)
        {
            //total�̒l���J�܂ɊY������܂ŉ��Z
            total += rewardDatas[i].rarityRate;
            Debug.Log("�󏭓x���̖J�ܗp�̃����_���Ȓl�F" + randomRewardValue + "<=" + "�J�܂̏d�݂Â��̍��v�l�F"+total);

            if(randomRewardValue <= total)
            {
                //�J�܊m��
                return rewardDatas[i];
            }
        }
        return null;
    }
}
