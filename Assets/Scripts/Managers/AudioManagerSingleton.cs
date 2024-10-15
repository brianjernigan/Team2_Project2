using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerSingleton : MonoBehaviour
{
    public static AudioManagerSingleton Instance { get; private set; }
    
    [SerializeField] private AudioSource _shotAudio;
    [SerializeField] private AudioSource _enemyHitAudio;
    [SerializeField] private AudioSource _playerHitAudio;
    [SerializeField] private AudioSource _reloadAudio;
    [SerializeField] private AudioSource _emptyMagAudio;
    [SerializeField] private AudioSource _chainAudio;
    [SerializeField] private AudioSource _collectAudio;
    [SerializeField] private AudioSource _themeMusic;
    [SerializeField] private AudioSource _weaponChangeAudio;
    [SerializeField] private AudioSource _doorbellAudio;
    [SerializeField] private AudioSource _levelOneMusic;
    [SerializeField] private AudioSource _trickOrTreatAudio;

    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
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

    public void PlayChainAudio()
    {
        if (_chainAudio.isPlaying)
        {
            _chainAudio.Stop();
        }
        
        _chainAudio.time = 0.673f;
        _chainAudio.Play();
    }

    public void PlayCollectAudio()
    {
        if (_collectAudio.isPlaying)
        {
            _collectAudio.Stop();
        }

        _collectAudio.time = .027f;
        _collectAudio.Play();
    }

    public void PlayThemeMusic()
    {
        _themeMusic.volume = .75f;
        _themeMusic.time = 14f;
        _themeMusic.Play();
    }

    public void StopThemeMusic()
    {
        _themeMusic.Stop();
    }

    public void PlayWeaponChangeAudio()
    {
        if (_weaponChangeAudio.isPlaying)
        {
            _weaponChangeAudio.Stop();
        }
        _weaponChangeAudio.Play();
    }

    public void PlayDoorbellAudio()
    {
        _doorbellAudio.volume = 0.35f;
        _doorbellAudio.time = .082f;
        _doorbellAudio.Play();
        Invoke(nameof(StopDoorbellAudio), 2f);
        PlayTrickOrTreatAudio();
    }

    public void StopDoorbellAudio()
    {
        _doorbellAudio.Stop();
    }

    public void PlayLevelOneMusic()
    {
        _levelOneMusic.volume = 0.1f;
        _levelOneMusic.Play();
    }

    public void PlayTrickOrTreatAudio()
    {
        _trickOrTreatAudio.Play();
    }
}
