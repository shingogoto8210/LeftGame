using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OfflineTimeManager : MonoBehaviour
{
    public static OfflineTimeManager instance;   �@//�V���O���g���p�̕ϐ�

    public DateTime loadDateTime = new DateTime(); //�O��Q�[�����~�߂����ɃZ�[�u���Ă��鎞��

    private int elaspedTime;                       //�o�ߎ���

    private const string SAVE_KEY_DATETIME = "OfflineDateTime";  //���Ԃ��Z�[�u�E���[�h����ۂ̕ϐ��B�萔�Ƃ��Đ錾����

    private const string FORMAT = "yyyy/MM//dd HH:mm::ss";       //�����̃t�H�[�}�b�g

    private const string WORKING_JOB_SAVE_KEY = "workingJobNo_"; //���g�����Ԃ̃f�[�^���Z�[�u�E���[�h���邽�߂̕ϐ�

    private GameManager gameManager;

    /// <summary>
    /// ���g���p�̎��ԃf�[�^���Ǘ����邽�߂̃N���X
    /// </summary>
    [Serializable]
    public class JobTimeData
    {
        public int jobNo;            //���g���̒ʂ��ԍ�
        public int elaspedJobTime;   //���g���̎c�莞��
        public string jobTimeString; //DateTime�N���X�𕶎���ɂ��邽�߂̕ϐ�
        
        /// <summary>
        /// DateTime�𕶎���ŕۑ����Ă���̂ŁADateTime�^�ɖ߂��Ď擾
        /// </summary>
        /// <returns></returns>
        public DateTime GetDateTime()
        {
            return DateTime.ParseExact(jobTimeString, FORMAT, null);
        }
    }

    [Header("���g���̎��ԃf�[�^�̃��X�g")]
    public List<JobTimeData> workingJobTimeDatasList = new List<JobTimeData>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //���Ԃ̃Z�[�u�f�[�^�̊m�F�ƃ��[�h
        LoadOfflineDateTime();

        //�I�t���C���ł̌o�ߎ��Ԃ��v�Z
        CalculateOfflineDateTimeElasped(loadDateTime);

        //TODO ���g���̃f�[�^�̃��[�h
    }

    /// <summary>
    /// �Q�[�����I�������Ƃ��Ɏ����I�ɌĂ΂��
    /// </summary>
    private void OnApplicationQuit()
    {

        //���݂̎��Ԃ̃Z�[�u
        SaveOfflineDateTime();

        Debug.Log("�Q�[�����f�B���Ԃ̃Z�[�u����");

        //���g�����̃f�[�^���P�ȏ゠��ꍇ
        for(int i = 0; i < workingJobTimeDatasList.Count; i++)
        {
            //���g���̎��ԃf�[�^��1�����Ԃɂ��ׂăZ�[�u
            SaveWorkingJobTimeData(workingJobTimeDatasList[i].jobNo);
        }
    }

    /// <summary>
    /// �I�t���C���ł̎��Ԃ����[�h
    /// </summary>
    public void LoadOfflineDateTime()
    {

        //�Z�[�u�f�[�^�����邩�m�F
        if (PlayerPrefsHelper.ExistsData(SAVE_KEY_DATETIME))
        {

            //�Z�[�u�f�[�^������ꍇ�A���[�h����
            string oldDateTimeString = PlayerPrefsHelper.LoadStringData(SAVE_KEY_DATETIME);

            //���[�h�����������DateTime�^�ɕϊ����Ď��Ԃ��擾
            loadDateTime = DateTime.ParseExact(oldDateTimeString, FORMAT, null);

            Debug.Log("�Q�[���J�n���F�Z�[�u����Ă������ԁF" + oldDateTimeString);

            Debug.Log("���̎��ԁF" + DateTime.Now.ToString(FORMAT));
        }
        else
        {
            //�Z�[�u�f�[�^���Ȃ��ꍇ�A���݂̎��Ԃ��J�n�����Ƃ��Ď擾���Ă���
            loadDateTime = DateTime.Now;

            Debug.Log("�Z�[�u�f�[�^���Ȃ��̂ō��̎��Ԃ��擾�F" + loadDateTime.ToString(FORMAT));
        }
    }
    /// <summary>
    /// ���݂̎��Ԃ��Z�[�u
    /// </summary>
    private void SaveOfflineDateTime()
    {

        //���݂̎��Ԃ��擾���āC������ɕϊ�
        string dateTimeString = DateTime.Now.ToString(FORMAT);

        //string�^�ŃZ�[�u
        PlayerPrefsHelper.SaveStringData(SAVE_KEY_DATETIME, dateTimeString);

        Debug.Log("�Q�[���I�����F�Z�[�u���ԁF" + dateTimeString);
    }

    /// <summary>
    /// �I�t���C���ł̌o�ߎ��Ԃ��v�Z
    /// </summary>
    /// <param name="oldDateTime"></param>
    /// <returns></returns>
    public int CalculateOfflineDateTimeElasped(DateTime oldDateTime)
    {
        //���݂̎��Ԃ��擾
        DateTime currentDateTime = DateTime.Now;

        //���݂̎��ԂƃZ�[�u����Ă��鎞�Ԃ��m�F
        if(oldDateTime > currentDateTime)
        {

            //�Z�[�u�f�[�^�̎��Ԃ̕������̎��Ԃ����i��ł���ꍇ�ɂ́A���̎��Ԃ����Ȃ���
            oldDateTime = DateTime.Now;
        }

        //�o�߂������Ԃ̍���
        TimeSpan dateTImeElasped = currentDateTime - oldDateTime;

        //�o�ߎ��Ԃ�b�ɂ���iMath.Round ���\�b�h�𗘗p���āAdouble�^��int�^�ɕϊ��B�����_�͂O�̂��炢�ŁC���l�̊ۂߏ����̎w���ToEven�i���l���Q�̐��l�̒��ԂɈʒu����Ƃ��ɁA�ł��߂����R�̒l�j���w��j
        elaspedTime = (int)Math.Round(dateTImeElasped.TotalSeconds, 0, MidpointRounding.ToEven);

        Debug.Log($"�I�t���C���ł̌o�ߎ��ԁF{elaspedTime }�b");

        return elaspedTime;
    }

    /// <summary>
    /// �e���g���̎c�莞�Ԃ̍X�V
    /// </summary>
    /// <param name="jobNo"></param>
    /// <param name="currentJobTime"></param>
    public void UpdateCurrentJobTime(int jobNo, int currentJobTime)
    {

        //List����Y����JobTimeData���������Ď擾���AelaspedJobTime�̒l��currentJobTime�ɍX�V
        workingJobTimeDatasList.Find(x => x.jobNo == jobNo).elaspedJobTime = currentJobTime;
    }

    /// <summary>
    /// List��JobTime��ǉ��B���̃��X�g�ɂ����񂪌��݂��g�������Ă�����e�ɂȂ�
    /// </summary>
    /// <param name="jobTimeData"></param>
    public void AddWorkingJobTimeDatasList(JobTimeData jobTimeData)
    {

        //���g����List�ɒǉ�����O�ɁA���łɃ��X�g�ɂ��邩�m�F���ďd���o�^��h��
        if(!workingJobTimeDatasList.Exists(x => x.jobNo == jobTimeData.jobNo))
        {

            //List�ɂȂ��ꍇ�̂݁A�V�����ǉ�����
            workingJobTimeDatasList.Add(jobTimeData);

            Debug.Log(jobTimeData.elaspedJobTime);
        }
    }

    public void RemoveWorkingJobTimeDatasList(int jobNo)
    {
        workingJobTimeDatasList.Remove(workingJobTimeDatasList[jobNo]);
        
    }

    /// <summary>
    /// ���݂��g������JobTimeData�̍쐬��List�ւ̒ǉ�
    /// </summary>
    /// <param name="tapPointDetail"></param>
    public void CreateWorkingJobTimeDatasList(TapPointDetail tapPointDetail)
    {

        //JobTimeData���C���X�^���X���ď�����
        JobTimeData jobTimeData = new JobTimeData { jobNo = tapPointDetail.jobData.jobNo, elaspedJobTime = tapPointDetail.jobData.jobTime };

        //List��JobTimeData��ǉ�
        AddWorkingJobTimeDatasList(jobTimeData);
    }

    /// <summary>
    /// ���g���̎��ԃZ�[�u
    /// ���g���J�n���ƃQ�[���I�����ɃZ�[�u
    /// </summary>
    /// <param name="jobNo"></param>
    public void SaveWorkingJobTimeData(int jobNo)
    {

        //�Z�[�u�Ώۂ�JobTimeData��List���猟�����Ď擾
        JobTimeData jobTimeData = workingJobTimeDatasList.Find(x => x.jobNo == jobNo);

        //���̎��Ԃ��擾���ĕ�����ɕϊ�
        jobTimeData.jobTimeString = DateTime.Now.ToString(FORMAT);

        //���g���̎��ԃf�[�^�̃Z�[�u
        PlayerPrefsHelper.SaveSetObjectData(WORKING_JOB_SAVE_KEY + jobTimeData.jobNo.ToString(), jobTimeData);

        string str = DateTime.Now.ToString(FORMAT);
        Debug.Log("�d�����F�Z�[�u����"+ str);
        Debug.Log("�Z�[�u���̂��g���̎c�莞�ԁF" + jobTimeData.elaspedJobTime);
    }

    /// <summary>
    /// �s����̐������A���̍s�����JobTimeData�����邩�m�F���A����ꍇ�ɂ̓��[�h����WorkingJobTimeDatasList�ɒǉ�
    /// </summary>
    /// <param name="tapPointDetailsList"></param>
    public void GetWorkingJobTimeDatasList(List<TapPointDetail> tapPointDetailsList)
    {
        for(int i = 0; i < tapPointDetailsList.Count; i++)
        {
            //�Y�����邨�g���̔ԍ��ŃZ�[�u����Ă��鎞�ԃf�[�^�����邩�m�F
            LoadOfflineJobTimeData(tapPointDetailsList[i].jobData.jobNo);
        }
    }

    /// <summary>
    /// ���g�����Ԃ̃��[�h
    /// </summary>
    /// <param name="jobNo"></param>
    public void LoadOfflineJobTimeData(int jobNo)
    {
        //�w�肳�ꂽ���g���̎��ԃf�[�^�̃Z�[�u�f�[�^�����邩�m�F
        if(PlayerPrefsHelper.ExistsData(WORKING_JOB_SAVE_KEY + jobNo.ToString()))
        {
            //�Z�[�u�f�[�^������ꍇ�A�擾���ăN���X�ɕ���
            JobTimeData jobTimeData = PlayerPrefsHelper.LoadGetObjectData<JobTimeData>(WORKING_JOB_SAVE_KEY + jobNo.ToString());

            //List��JobTimeData��ǉ�
            AddWorkingJobTimeDatasList(jobTimeData);

            //������ɂȂ��Ă��鎞�Ԃ�DateTime�\���̂ɕ������Ď擾
            DateTime time = jobTimeData.GetDateTime();

            string str = time.ToString(FORMAT);
            Debug.Log("�d�����ԁF�Z�[�u����Ă������ԁF" + str);
            Debug.Log("���[�h���̎c�莞�ԁF" + jobTimeData.elaspedJobTime);
            
        }
    }
}
