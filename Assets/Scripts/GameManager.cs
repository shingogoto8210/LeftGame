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
        //TODO ���g���̎��ԃf�[�^�̊m�F�ƃ��[�h

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

            //TODO ���g���J�n���Ԃ̃Z�[�u

            //���g���̏����Ƃ��g���J�n
            tapPointDetail.PreparateJobs(tapPointDetail.jobData.jobTime);
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
}
