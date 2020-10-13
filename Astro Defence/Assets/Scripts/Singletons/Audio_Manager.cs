using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class AudioClipDictionary
{
    public string name;
    public AudioClip clip;
}

public class Audio_Manager : MonoBehaviour
{
    public static Audio_Manager SharedInstance;

    public List<AudioClipDictionary> effectClips = new List<AudioClipDictionary>();
    [Space(10)]

    public AudioClip backingTrack;
    [Space(10)]

    public AudioSource backingTrackSource;
    public AudioSource soundEffectSource;
    public AudioSource backupSource; //getting weird audio glitches if a power up is granted whilst an asteroid blows up. Added this audio source to hopefully combat that.
    [Space(10)]

    public bool playBackingTrackOnStart = true;
    public bool PlaySoundEffects = true;

    private void Awake()
    {
        DontDestroyOnLoad(this); //keeps the backing track playing if we restart the scene

        SharedInstance = this;

        if (playBackingTrackOnStart)
            PlayBackingTrack(true);
    }

    public void PlayBackingTrack(bool loop)
    {
        backingTrackSource.loop = loop;

        backingTrackSource.clip = backingTrack;
        backingTrackSource.Play();
    }

    public void PlaySoundEffect(string effectName)
    {
        if (!PlaySoundEffects)
            return;

        AudioClip clip = effectClips.Where(item => item.name == effectName).FirstOrDefault().clip;
       
        if(clip == null)
        {
            Debug.LogError(string.Format("Audio clip {0} not found.", effectName));
            return;
        }

        if (!soundEffectSource.isPlaying)
        {
            soundEffectSource.clip = clip;
            soundEffectSource.Play();
        }
        else
        {
            backupSource.clip = clip;
            backupSource.Play();
        }
    }
}
