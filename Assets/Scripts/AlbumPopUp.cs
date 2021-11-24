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

        //TODO �l�����Ă���J�܂̐������T���l�C���p�̃Q�[���I�u�W�F�N�g�̐����������J��Ԃ�

        //TODO �l�����Ă���J�ܗp�̃Q�[���I�u�W�F�N�g�i�T���l�C���p�j�𐶐� 
        
        //TODO �T���l�C���p�̃Q�[���I�u�W�F�N�g�ɗ��p����J�܂̃f�[�^���擾���Đݒ�
        
        //TODO �����摜�̐ݒ�
        
        //TODO �J�܈ꗗ��List�ɓo�^
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
}
