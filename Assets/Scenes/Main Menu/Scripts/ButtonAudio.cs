using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ButtonAudio : MonoBehaviour
{

    public AudioManager.SFX_Enum type;
    private void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            AudioManager.Instance.playSfx(type);
        });
    }

}
