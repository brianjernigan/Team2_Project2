using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _hitbox;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetTrigger("Punch");
        }
    }

    public void EnableHitBox()
    {
        if (_hitbox is not null)
        {
            _hitbox.SetActive(true);
            Debug.Log("Activated");
        }
    }

    public void DisableHitBox()
    {
        if (_hitbox is not null)
        {
            _hitbox.SetActive(false);
            Debug.Log("Deactivated");
        }
    }
}
