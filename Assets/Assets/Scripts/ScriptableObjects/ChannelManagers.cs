using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
[CreateAssetMenu(fileName = "ChannelManagers", menuName = "ScriptableObject/Audio/ChannelManagers",order =1)]
public class ChannelManagers : ScriptableObject
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private string channelVolume;
    [SerializeField] private float currentVolume;
    private bool isMuted;

    public void UpdateVolume(Slider slider)
    {
        currentVolume = slider.value;
        myMixer.SetFloat(channelVolume, Mathf.Log10(currentVolume) * 20f);
    }
    public void UpdateVolume(TMP_Text myText)
    {
        myText.text = ((int)(currentVolume*100)).ToString() + "%";
    }
    public void MuteaAudio()
    {
        if (isMuted)
        {
            myMixer.SetFloat(channelVolume, Mathf.Log10(currentVolume) * 20f);
            isMuted = false;
        }
        else
        {
            myMixer.SetFloat(channelVolume, -80);
            isMuted = true;
        }
    }
}
