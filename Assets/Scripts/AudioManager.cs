using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    System.Random _rnd = new System.Random();

    public static AudioManager instance;
    bool isFading;

    public float currentMusicVol;
    public float currentAmbientVol;
    public float currentSFXVol;

    [Header("Mixers")]
    public AudioMixer mixer;

    public AudioSource bkgMusic;
    public AudioClip[] bkgTracks;

    public AudioSource bkgAmbient;

    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(this);
            }
        }

        //mixer.SetFloat("MasterVol", Mathf.Log10(0.01f) * 20);
    }

    private void Update()
    {
        if (!bkgMusic.isPlaying)
        {
            AudioClip newClip = bkgTracks[_rnd.Next(0, bkgTracks.Length)];

            bkgMusic.clip = newClip;
            bkgMusic.Play();
        }
    }

    public void GameStart()
    {
        bkgAmbient.Play();
    }

    public void GameEnd()
    {
        bkgAmbient.Pause();
    }

    public IEnumerator StartFade(AudioMixer audioMixer, string exposedParam, float duration, float targetVolume)
    {
        isFading = true;
        Debug.Log("Starting fade to " + targetVolume + "dB");
        float currentTime = 0;
        float currentVol;
        audioMixer.GetFloat(exposedParam, out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }
        Debug.Log("Faded to " + targetVolume + "dB");

        isFading = false;
        yield break;
    }

    public void SetMusicVol(float musicLvl)
    {
        currentMusicVol = musicLvl;
        mixer.SetFloat("MusicVol", Mathf.Log10(musicLvl) * 20);
        PlayerPrefs.SetFloat("MusicVol", musicLvl);
        PlayerPrefs.Save();
    }

    public void SetAmbientVol(float ambientLvl)
    {
        currentAmbientVol = ambientLvl;
        mixer.SetFloat("AmbientVol", Mathf.Log10(ambientLvl) * 20);
        PlayerPrefs.SetFloat("AmbientVol", ambientLvl);
        PlayerPrefs.Save();
    }

    public void SetSFXVol(float sfxLvl)
    {
        currentSFXVol = sfxLvl;
        mixer.SetFloat("SFXVol", Mathf.Log10(sfxLvl) * 20);
        PlayerPrefs.SetFloat("SFXVol", sfxLvl);
        PlayerPrefs.Save();
    }
}
