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

            //���g���̏�ԂƎc�莞�Ԃ��擾
            (bool isJobEnd, int remainingTime) = JudgeJobsEnd(jobTime);

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
            OfflineTimeManager.instance.CreateWorkingJobTimeDatasList(tapPointDetail);

            // ���g���J�n���Ԃ̃Z�[�u
            OfflineTimeManager.instance.SaveWorkingJobTimeData(tapPointDetail.jobData.jobNo);

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
        //�Q�[���N�����̎��ԂƂ��g�����J�n�������ԂƂ̍����l���Z�o
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
}
