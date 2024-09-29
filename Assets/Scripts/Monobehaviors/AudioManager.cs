using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _shotAudio;
    [SerializeField] private AudioSource _enemyHitAudio;
    [SerializeField] private AudioSource _playerHitAudio;
    [SerializeField] private AudioSource _reloadAudio;
    [SerializeField] private AudioSource _emptyMagAudio;

    public void PlayShotAudio()
    {
        if (_shotAudio.isPlaying)
        {
            _shotAudio.Stop();
        }
        
        _shotAudio.time = 0.145f;
        _shotAudio.Play();
    }

    public void PlayEnemyHitAudio()
    {
        if (_enemyHitAudio.isPlaying)
        {
            _enemyHitAudio.Stop();
        }

        _enemyHitAudio.Play();
    }

    public void PlayPlayerHitAudio()
    {
        if (_playerHitAudio.isPlaying)
        {
            _playerHitAudio.Stop();
        }

        _playerHitAudio.time = 0.117f;
        _playerHitAudio.Play();
    }

    public void PlayReloadAudio()
    {
        _reloadAudio.time = 0.67f;
        _reloadAudio.Play();
    }

    public void PlayEmptyMagAudio()
    {
        if (_emptyMagAudio.isPlaying)
        {
            _emptyMagAudio.Stop();
        }
        
        _emptyMagAudio.time = 0.022f;
        _emptyMagAudio.Play();
    }
}