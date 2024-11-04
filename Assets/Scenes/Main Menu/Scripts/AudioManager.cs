using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    public enum SFX_Enum
    {
        BUTTON_CLICK    
    }    
    public enum Music_Enum
    {
        MUSIC    
    }

    [System.Serializable]
    public class Data
    {
        public AudioClip audio;
        public float volume;
        public bool loop;
    }

    [System.Serializable]
    public class sfxData: Data
    {
        public SFX_Enum type;
    }

    [System.Serializable]
    public class musicData: Data
    {
        public Music_Enum type;
    }

    public List<sfxData> sfxdata = new List<sfxData>();
    public AudioSource sfxSource, musicSource;

    private List<AudioSource> sfxList = new List<AudioSource>();
    private List<AudioSource> musicList = new List<AudioSource>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);    
        }
    }

    public void playSfx(SFX_Enum _type)
    {
        var audioSource = GetAudioSource(sfxList, sfxSource);

        for (int i = 0; i < sfxdata.Count; i++)
        {
            if (sfxdata[i].type.Equals(_type))
            {
                audioSource.clip = sfxdata[i].audio;
                audioSource.volume = sfxdata[i].volume;
                audioSource.loop = sfxdata[i].loop;
                audioSource.Play();
                break;
            }
        }

        //Másik opcio, for nélkül
        //var data = sfxdata.Where(item => item.type.Equals(_type)).FirstOrDefault(); 
    }
    
    public void playMusic()
    {

    }

    public AudioSource GetAudioSource(List<AudioSource> _list, AudioSource source)
    {
        for (int i = 0; i < sfxList.Count; i++)
        {
            if (!sfxList[i].isPlaying)
            {
                return sfxList[i];
            }
        }

        var newsource = Instantiate(source, source.transform.parent);
        newsource.gameObject.SetActive(true); 
        sfxList.Add(newsource);
        return newsource;
    }

    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;
    public float musicVolume, sfxVolume;

    void Start()
    {
        Load();
        audioMixer.GetFloat("Music_Volume", out musicVolume);
        audioMixer.GetFloat("SFX_Volume", out sfxVolume);
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
    }

    public void changeMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music_Volume", volume);
        Save();
    }

    public void changeSfxVolume(float volume)
    {
        audioMixer.SetFloat("SFX_Volume", volume);
        Save();
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            audioMixer.SetFloat("Music_Volume", musicSlider.value);
        }
        else
        {
            musicSlider.value = 1;
            audioMixer.SetFloat("MusicVolume", 1);
        }

        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
            audioMixer.SetFloat("SFX_Volume", sfxSlider.value);
        }
        else
        {
            sfxSlider.value = 1;
            audioMixer.SetFloat("SFX_Volume", 1);
        }
    }
}
