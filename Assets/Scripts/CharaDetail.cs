using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharaDetail : MonoBehaviour
{
    [SerializeField]
    private Button btnChara;

    void Start()
    {
        btnChara.interactable = false;

        btnChara.onClick.AddListener(OnClickChara);

        btnChara.interactable = true;
    }

    private void OnClickChara()
    {
        Debug.Log("‚¨Žg‚¢‚ÌŒ‹‰Ê‚ð•\Ž¦");

        Destroy(gameObject);
    }
}
