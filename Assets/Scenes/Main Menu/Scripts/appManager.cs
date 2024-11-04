using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class appManager : MonoBehaviour
{
    public float duration;
    public UnityEngine.UI.Image fadeImage;

    public GameObject popup;
    public Button settingsButton;
    public AudioManager audioManager;


    private void OnEnable()
    {
        StartCoroutine(FadeCoroutine(Color.black, Color.clear));
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void SettingsButton()
    {
        popup.SetActive(false);
        settingsButton.onClick.AddListener(Settings);
    }

    public void Settings()
    {
        bool isActive = popup.activeSelf;
        popup.SetActive(!isActive);
    }
    public void SettingsSave()
    {
        audioManager.Save();
        popup.SetActive(false);
    }

    public void LoadScene(int _index)
    {
        StartCoroutine(FadeCoroutine(Color.clear, Color.black, ()=>
        {
            SceneManager.LoadScene(_index);
        }));
    }

    private IEnumerator FadeCoroutine(Color _from, Color _to, Action _onEnd = null)
    {
        float time = 0;
        while (time <= duration)
        {
            time += Time.deltaTime;
            fadeImage.color = Color.Lerp(_from, _to, time / duration);
            yield return null;
        }

        _onEnd?.Invoke();
    }

}
