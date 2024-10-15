using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManagerSingleton : MonoBehaviour
{
    public static AmmoManagerSingleton Instance { get; private set; }

    private bool _isReloading;

    private const float ReloadDuration = 1f;

    public event Action OnAmmoChanged;

    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        OnAmmoChanged?.Invoke();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !_isReloading)
        {
            Reload();
        }
    }

    public bool HasAmmo()
    {
        return PlayerStatManagerSingleton.Instance.CurrentAmmo > 0;
    }

    public void DecreaseAmmo()
    {
        if (PlayerStatManagerSingleton.Instance.CurrentAmmo <= 0) return;

        PlayerStatManagerSingleton.Instance.CurrentAmmo--;
        OnAmmoChanged?.Invoke();
    }

    private void Reload()
    {
        if (PlayerStatManagerSingleton.Instance.CurrentAmmo >= PlayerStatManagerSingleton.Instance.CurrentMaxAmmo) return;

        StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        _isReloading = true;
        AudioManagerSingleton.Instance.PlayReloadAudio();
        
        yield return new WaitForSeconds(ReloadDuration);

        _isReloading = false;
        PlayerStatManagerSingleton.Instance.CurrentAmmo = PlayerStatManagerSingleton.Instance.CurrentMaxAmmo;
        OnAmmoChanged?.Invoke();
    }
}
