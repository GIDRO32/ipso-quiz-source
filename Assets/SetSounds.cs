using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetSounds : MonoBehaviour
{
    public AudioMixer Sound_Mixer;
    public Slider Sound_Slider;
    void Start()
    {
        Sound_Slider.value = PlayerPrefs.GetFloat("Sound", 0.75f);
    }
    public void setSoundVol(float sliderValue)
    {
        Sound_Mixer.SetFloat("SoundVol", Mathf.Log10(sliderValue)*20);
        PlayerPrefs.SetFloat("Sound", sliderValue);
    }
}