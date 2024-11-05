using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private AudioSource audioSource;
    public AudioClip defaultClip; 
    public AudioClip scene1Clip;
    public AudioClip endSceneClip;
    public AudioClip buttonClickSound;

    private void Awake() 
    {
        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on BackgroundMusic object.");
            return;
        }

        audioSource.Play();
    }

    private void Start()
    {
        audioSource.clip = defaultClip;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬 이름 또는 인덱스에 따라 Audio Clip 변경
        if (scene.name == "Scene1")
        {
            ChangeAudioClip(scene1Clip);
        }
        else if (scene.name == "EndScene")
        {
            ChangeAudioClip(endSceneClip);
        }
        else
        {
            ChangeAudioClip(defaultClip);
        }
    }

    public void ChangeAudioClip(AudioClip newClip)
    {
        if (audioSource.clip == newClip) return; // 이미 재생 중인 경우 변경하지 않음

        audioSource.clip = newClip;
        audioSource.Play();
    }

    public void PlayButtonClickSound()
    {
        if (buttonClickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
}
