using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManagerSingleton : MonoBehaviour
{
    public static AmmoManagerSingleton Instance { get; private set; }

    public bool IsReloading { get; set; }

    private const float ReloadDuration = 1.75f;

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
        if (Input.GetKeyDown(KeyCode.LeftShift) && !IsReloading)
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
        IsReloading = true;
        AudioManagerSingleton.Instance.PlayReloadAudio();
        
        yield return new WaitForSeconds(ReloadDuration);

        IsReloading = false;
        PlayerStatManagerSingleton.Instance.CurrentAmmo = PlayerStatManagerSingleton.Instance.CurrentMaxAmmo;
        OnAmmoChanged?.Invoke();
    }
}
