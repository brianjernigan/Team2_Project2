using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager Instance { get; private set; }

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
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Reload();
        }
    }

    public bool HasAmmo()
    {
        return PlayerStats.Instance.CurrentAmmo > 0;
    }

    public void DecreaseAmmo()
    {
        if (PlayerStats.Instance.CurrentAmmo <= 0) return;

        PlayerStats.Instance.CurrentAmmo--;
        OnAmmoChanged?.Invoke();
    }

    private void Reload()
    {
        if (PlayerStats.Instance.CurrentAmmo >= PlayerStats.Instance.CurrentMaxAmmo) return;

        StartCoroutine(ReloadRoutine());
        AudioManager.Instance.PlayReloadAudio();
    }

    private IEnumerator ReloadRoutine()
    {
        yield return new WaitForSeconds(1f);

        PlayerStats.Instance.CurrentAmmo = PlayerStats.Instance.CurrentMaxAmmo;
        OnAmmoChanged?.Invoke();
    }
}
