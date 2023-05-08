using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider mySliderMaster;
    [SerializeField] private Slider mySliderEffects;
    [SerializeField] private Slider mySliderMusic;
    public void ChangeValuesMaster()
    {
        float newValue = mySliderMaster.value;
        myMixer.SetFloat("MasterVolume",Mathf.Log10( newValue)*20f);
    }
    public void ChangeValuesEffects()
    {
        float newValue = mySliderEffects.value;
        myMixer.SetFloat("EffectsVol", Mathf.Log10(newValue) * 20f);
    }
    public void ChangeValuesMusic()
    {
        float newValue = mySliderMusic.value;
        myMixer.SetFloat("MusicVol", Mathf.Log10(newValue) * 20f);
    }
    public void MuteChanel()
    {
        myMixer.SetFloat("MasterVolume", Mathf.Log10(-80) * 20f);
    }
}
