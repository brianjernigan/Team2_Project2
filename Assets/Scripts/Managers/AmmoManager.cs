using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager Instance { get; private set; }

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
        return PlayerStatManager.Instance.CurrentAmmo > 0;
    }

    public void DecreaseAmmo()
    {
        if (PlayerStatManager.Instance.CurrentAmmo <= 0) return;

        PlayerStatManager.Instance.CurrentAmmo--;
        OnAmmoChanged?.Invoke();
    }

    private void Reload()
    {
        if (PlayerStatManager.Instance.CurrentAmmo >= PlayerStatManager.Instance.CurrentMaxAmmo) return;

        StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        IsReloading = true;
        AudioManager.Instance.PlayReloadAudio();
        
        yield return new WaitForSeconds(ReloadDuration);

        IsReloading = false;
        PlayerStatManager.Instance.CurrentAmmo = PlayerStatManager.Instance.CurrentMaxAmmo;
        OnAmmoChanged?.Invoke();
    }
}
