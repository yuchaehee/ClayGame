using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    // 일단 임시로 해놓았습니다.. 더 추가 해야합니다..

    public AudioClip audioTouch;
    public AudioClip audioUnlock;
    public AudioClip audioGrow;
    public AudioClip audioSell;
    public AudioClip audioBuy;
    public Slider sfxSlider;
    public Slider bgmSlider;

    AudioSource bgmSound;
    AudioSource sfxSound;

    private void Awake()
    {
        // [0] 은 bgm 관리, [1] 은 효과음 관리
        bgmSound = GetComponentsInChildren<AudioSource>()[0];
        sfxSound = GetComponentsInChildren<AudioSource>()[1];
    }

    private void Update()
    {
        bgmSound.volume = bgmSlider.value;
        sfxSound.volume = sfxSlider.value;
    }

    public void PlaySound(string action)
    {
        switch (action)
        {
            case "TOUCH":
                sfxSound.clip = audioTouch;
                break;
            case "UNLOCK":
                sfxSound.clip = audioUnlock;
                break;
            case "GROW":
                sfxSound.clip = audioGrow;
                break;
            case "SELL":
                sfxSound.clip = audioSell;
                break;
            case "BUY":
                sfxSound.clip = audioBuy;
                break;
        }

        sfxSound.Play();
    }
}
