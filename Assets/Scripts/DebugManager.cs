using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DebugManager : MonoBehaviour
{
    public static DebugManager instance;

    [SerializeField]
    private Button btnSaveDataReset;

    [SerializeField]
    private Text txtDialog;

    [Header("�f�o�b�O��L���ɂ���ꍇ�̓`�F�b�N������")]
    public bool isDebugModeOn;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //�f�o�b�O�@�\�̗L��/������
        SetUpDebugMode();
    }

    /// <summary>
    /// �f�o�b�O�@�\�̗L��/���������m�F���Đ؂肩��
    /// </summary>
    private void SetUpDebugMode()
    {
        //�{�^����\��
        btnSaveDataReset.gameObject.SetActive(isDebugModeOn);

        //�{�^�����L���Ȃ�
        if (btnSaveDataReset.gameObject.activeSelf)
        {
            btnSaveDataReset.onClick.AddListener(OnClickAllSaveDataReset);
        }

        //���O�p�̃e�L�X�g��\��
        txtDialog.gameObject.SetActive(isDebugModeOn);
    }

    /// <summary>
    /// �Z�[�u�f�[�^�폜�{�^�����������ۂ̏���
    /// </summary>
    private void OnClickAllSaveDataReset()
    {
        OfflineTimeManager.instance.AllRemoveWorkingJobTimeDatasList();
    }

    /// <summary>
    /// �Q�[����ʂɃ��O�\��
    /// </summary>
    /// <param name="log"></param>
    public void DisplayDebugDialog(string log)
    {
        txtDialog.text += log + "\n";
    }

    private void Update()
    {
        //�f�o�b�O�@�\�������̏ꍇ�ɂ͏������s��Ȃ�
        if (!isDebugModeOn)
        {
            return;
        }

        //�V�[���̍ēǂݍ���(���s/��~�̎�Ԃ��Ȃ��j
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReloadScene();
        }
    }

    /// <summary>
    /// ���݂̃V�[�����ēǂݍ���
    /// </summary>
    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
