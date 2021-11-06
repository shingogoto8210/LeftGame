using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TapPointDetail : MonoBehaviour
{
    [SerializeField]
    private Button btnTapPoint;  //Button�R���|�[�l���g�𐧌䂷�邽�߂̕ϐ�

    [SerializeField]
    private Transform canvasTran;

    [SerializeField]
    private JobsComfirmPopUp jobsComfirmPopUpPrefab;

    private JobsComfirmPopUp jobsComfirmPopUp;

    [SerializeField,Header("���̍s����̂��g���ԍ�")]
    private int myjobNo;

    public JobData jobData;  //���g���̏���o�^

    void Start()
    {

        //�{�^�����������ۂɎ��s���鏈���i���\�b�h�j�������ɂ��ēo�^
        btnTapPoint.onClick.AddListener(OnClickTapPoint);
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
        if(jobsComfirmPopUp == null)
        {
            //�s�挈��p�̃|�b�v�A�b�v�\��
            //Debug.Log("TapPoint �s�挈��p�̃|�b�v�A�b�v�\��");
            jobsComfirmPopUp = Instantiate(jobsComfirmPopUpPrefab, canvasTran, false);
            jobsComfirmPopUp.OpenPopUp(this);
        }
        //2��ڈڍs�́CSetActive���I���ɂ��ĕ\������
        else
        {
            jobsComfirmPopUp.OpenPopUp(this);
        }
        
    }
}
