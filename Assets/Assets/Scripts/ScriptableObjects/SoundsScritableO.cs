using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "SoundsScritableO", menuName = "ScriptableObject/Audio/SoundsScritableO", order = 2)]

public class SoundsScritableO : ScriptableObject
{
    [SerializeField] private AudioClip myAudio;
    [SerializeField] private AudioMixerGroup myGroup;

    public void CreateSound()
    {
        GameObject audioGameObject = new GameObject();
        AudioSource myAudioSource = audioGameObject.AddComponent<AudioSource>();

        myAudioSource.outputAudioMixerGroup = myGroup;
        myAudioSource.PlayOneShot(myAudio);

        Destroy(audioGameObject,1f) ;
    }

}
