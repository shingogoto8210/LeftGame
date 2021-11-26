using UnityEngine;
/// <summary>
/// �w�肵���N���X��string�^��Json�`����PlayerPrefs�N���X�ɃZ�[�u�E���[�h���邽�߂̃w���p�[�N���X
/// </summary>
public static class PlayerPrefsHelper
{
    /// <summary>
    /// �w�肵���L�[�̃f�[�^�����݂��Ă��邩�m�F
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
   public static bool ExistsData(string key)
    {
        //�w�肵���L�[�̃f�[�^�����݂��Ă��邩�m�F���āC���݂��Ă���ꍇ��true,���݂��Ă��Ȃ��ꍇ��false��߂�
        return PlayerPrefs.HasKey(key);
    }

    /// <summary>
    /// �w�肳�ꂽ�I�u�W�F�N�g�̃f�[�^���Z�[�u
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public static void SaveSetObjectData<T>(string key,T obj)
    {

        //�I�u�W�F�N�g�̃f�[�^��Json�`���ɕϊ�
        string json = JsonUtility.ToJson(obj);

        //�Z�b�g
        PlayerPrefs.SetString(key, json);

        //�Z�b�g����key��json���Z�[�u
        PlayerPrefs.Save();
    }

    /// <summary>
    /// �w�肳�ꂽ�I�u�W�F�N�g�̃f�[�^�����[�h
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T LoadGetObjectData<T>(string key)
    {

        //�Z�[�u����Ă���f�[�^�����[�h
        string json = PlayerPrefs.GetString(key);

        //�ǂݍ��ތ^���w�肵�ĕϊ����Ď擾
        return JsonUtility.FromJson<T>(json);
    }

    /// <summary>
    /// �w�肳�ꂽ�L�[�̃f�[�^���폜
    /// </summary>
    /// <param name="key"></param>
    public static void RemoveObjectData(string key)
    {

        //�w�肳�ꂽ�L�[�̃f�[�^���폜
        PlayerPrefs.DeleteKey(key);

        Debug.Log("�Z�[�u�f�[�^���폜�@���s�F" + key);
    }

    /// <summary>
    /// ���ׂẴZ�[�u�f�[�^���폜
    /// </summary>
    public static void AllClearSaveData()
    {

        //���ׂẴZ�[�u�f�[�^���폜
        PlayerPrefs.DeleteAll();

        Debug.Log("�S�Z�[�u�f�[�^���폜�@���s");
        DebugManager.instance.DisplayDebugDialog("�S�Z�[�u�f�[�^���폜�@���s");
    }

    /// <summary>
    /// �����f�[�^�̃Z�[�u
    /// </summary>
    /// <param name="key"></param>
    /// <param name="saveValue"></param>
    public static void SaveIntData(string key, int saveValue)
    {

        //�����f�[�^�̃Z�b�g�ƃZ�[�u
        PlayerPrefs.SetInt(key, saveValue);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// �����f�[�^�̃��[�h
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static int LoadIntData(string key)
    {

        //�����f�[�^�̃��[�h
        return PlayerPrefs.GetInt(key);
    }

    /// <summary>
    /// ������f�[�^�̃Z�[�u�i���DateTime�\���̂̃Z�[�u�Ɏg���j
    /// </summary>
    /// <param name="key"></param>
    /// <param name="saveValue"></param>
    public static void SaveStringData(string key,string saveValue)
    {
        PlayerPrefs.SetString(key, saveValue);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// ������f�[�^�̃��[�h�i���DateTime�\���̂̃��[�h�Ɏg���j
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string LoadStringData(string key)
    {
        return PlayerPrefs.GetString(key);
    }
}
