using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TapPointDetail : MonoBehaviour
{
    [SerializeField]
    private Button btnTapPoint;     //Button�R���|�[�l���g�𐧌䂷�邽�߂̕ϐ�

    private JobsConfirmPopUp jobsComfirmPopUp;

    [SerializeField,Header("���̍s����̂��g���ԍ�")]
    private int myjobNo;

    public JobData jobData;        //���g���̏���o�^

    [SerializeField]
    private Image imgTapPoint;     //�s����̉摜��ύX���邽�߂̃R���|�[�l���g�̑��

    [SerializeField]
    private Sprite jobSprite;      //���g�����̉摜�̓o�^�p

    [SerializeField]
    private Sprite defaultSprite;  //�����̍s����̉摜�̓o�^�p

    [SerializeField]
    private Tween tween;           //DOTween�̏����������邽�߂̕ϐ�

    [SerializeField]
    private int currentJobTime;    //���g�������Ă��鎞�Ԃ̌v���p

    private bool isJobs;           //���g�������ǂ����𔻒肷��l�Ctrue�Ȃ�΂��g�����Ƃ��ė��p����

    /// <summary>
    /// isJobs�ϐ��̃v���p�e�B
    /// </summary>
    public bool IsJobs
    {
        set
        {
            isJobs = value;
        }
        get
        {
            return isJobs;
        }
    }

    private GameManager gameManager;

    /// <summary>
    /// TapPointDetail�̐ݒ�
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpTapPointDetail(GameManager gameManager)
    {
        //�{�^�����������ۂɎ��s���鏈���i���\�b�h�j�������Ɏw�肵�ēo�^
        btnTapPoint.onClick.AddListener(OnClickTapPoint);

        this.gameManager = gameManager;

    }

    public void SetUpTapPointFromJobDataSO( List<JobData> jobDatasList)
    {
        jobData = jobDatasList.Find(x => x.jobNo == myjobNo);

    }

    /// <summary>
    /// �^�b�v�|�C���g���^�b�v�����ۂ̏���
    /// </summary>
    private void OnClickTapPoint()
    {

        //Debug.Log("TapPoint �^�b�v");

        //�^�b�v�A�j�����o
        //Debug.Log("TapPoint �^�b�v�A�j�����o");

        //DOTween�̋@�\�̂P�ł���DOpunchScale���\�b�h�𗘗p���ăA�j�����o��ǉ�
        transform.DOPunchScale(Vector3.one * 1.25f, 0.15f).SetEase(Ease.OutBounce);

        //�|�b�v�A�b�v���܂���������Ă��Ȃ��Ƃ�
        //if(jobsComfirmPopUp == null)
        //{
        //�s�挈��p�̃|�b�v�A�b�v�\��
        //Debug.Log("TapPoint �s�挈��p�̃|�b�v�A�b�v�\��");
        //jobsComfirmPopUp = Instantiate(jobsComfirmPopUpPrefab, canvasTran, false);
        //jobsComfirmPopUp.OpenPopUp(this);
        //}
        //2��ڈڍs�́CSetActive���I���ɂ��ĕ\������
        //else
        //{
        //jobsComfirmPopUp.OpenPopUp(this);
        //}

        //GameManager�N���X�ɂ���s����m�F�|�b�v�A�b�v�𐶐����郁�\�b�h�����s����
        gameManager.GenerateJobsConfirmPopUp(this);
    }

    /// <summary>
    /// ���g���̏���
    /// </summary>
    public void PreparateJobs(int remainingTime)
    {
        ChangeJobSprite();
        IsJobs = true;
        StartCoroutine(WorkingJobs(remainingTime));
    }

    /// <summary>
    /// ���g�����̉摜�ɕύX
    /// </summary>
    public void ChangeJobSprite()
    {
        imgTapPoint.sprite = jobSprite;

        ChangeActiveButton(false);

        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        tween = transform.DOPunchPosition(new Vector3(10.0f, 0, 0), 3.0f, 10, 3)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    /// <summary>
    /// ���g���̊J�n�B���Ԍo�ߏ���
    /// </summary>
    /// <param name="normaJobTime"></param>
    /// <returns></returns>
    public IEnumerator WorkingJobs(int normaJobTime)
    {

        //�c���Ă��邨�g���̎��Ԃ�ݒ�
        currentJobTime = normaJobTime;

        //���g�����I��邩���m�F
        while (IsJobs)
        {
            //TODO �����Ƃ��Ď��Ԃ��m�F����
            currentJobTime--;

            OfflineTimeManager.instance.UpdateCurrentJobTime(jobData.jobNo, currentJobTime);

            //Debug.Log(currentJobTime);

            //�c�莞�Ԃ�0�ȉ��ɂȂ�����
            if(currentJobTime <= 0)
            {
                
                KillTween();
                IsJobs = false;
                ChangeActiveButton(true);
                //OfflineTimeManager.instance.RemoveWorkingJobTimeDatasList(jobData.jobNo);
            }

            yield return null;
        }

        //���g���Ɋւ������������Ԃɖ߂�
        ReturnDefaultState();

        //�d���I��
        Debug.Log("���g���I��");

        //�L��������
        //GenerateCharaDetail();

        //GameManager�N���X�ɂ���L�����𐶐����郁�\�b�h�����s����
        gameManager.GenerateCharaDetail(this);
    }

    /// <summary>
    /// Tween��j��
    /// </summary>
    public void KillTween()
    {
        tween.Kill();
        //Debug.Log("tween���~�߂�");
    }

    /// <summary>
    /// ���g���Ɋւ������������Ԃɖ߂�
    /// </summary>
    public void ReturnDefaultState()
    {

        //���g�����̉摜�����̉摜�ɖ߂�
        imgTapPoint.sprite = defaultSprite;

        //���g���̎��Ԃ����Z�b�g
        currentJobTime = 0;

        //�I�u�W�F�N�g�̃T�C�Y�������l�ɖ߂�
        transform.localScale = Vector3.one;
    }

    //private void GenerateCharaDetail()
    //{
      //  Instantiate(charaDetailPrefab,transform, false);
    //}

    private void ChangeActiveButton(bool isActive)
    {
        btnTapPoint.enabled = isActive;
    }
}
