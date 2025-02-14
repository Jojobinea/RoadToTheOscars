using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip _bgSound;
    [SerializeField] private AudioSource _bgSource;

    void Awake()
    {
        _bgSource.clip = _bgSound;        
        _bgSource.loop = true;
        _bgSource.playOnAwake = false;
    }

    private void Start()
    {
        Debug.Log("starting to play");
        PlayMusic();
    }

    public void PlayMusic()
    {
        if (!_bgSource.isPlaying)
        {
            _bgSource.Play();
        }
    }

    public void StopMusic()
    {
        if (_bgSource.isPlaying)
        {
            _bgSource.Stop();
        }
    }
}
