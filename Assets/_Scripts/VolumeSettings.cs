using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class VolumeSettings : MonoBehaviour
{

    [SerializeField] private AudioMixer volMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();
    }
    public void SetMasterVolume()
    {
        float volume = masterSlider.value;
        volMixer.SetFloat("master", Mathf.Log10(volume)*20);
    }
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        volMixer.SetFloat("music", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        volMixer.SetFloat("soundfx", Mathf.Log10(volume) * 20);
    }

}
