using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GunInventory
{
    [SerializeField] private List<GunElement> _guns;

    public List<GunElement> Guns
    {
        get => _guns;
    }

    public void AddWeapon(GunElement newGun)
    {
        Guns.Add(newGun);
    }
}

public class GunSystem : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] private GunInventory _gunInventory;

    [Header("Gun")]
    [SerializeField] private Transform _handGunModelParent;

    [SerializeField] private GunElement _handGun;

    private Transform _camera;

    private float _shootTimer;

    private bool _isReloading;

    [Header("FX")]
    private ParticleSystem _muzzleFlash;

    [Header("Audio")]
    [SerializeField] private AudioSource _shootAudioSource;

    void Start()
    {
        _camera = Camera.main.transform;

        _handGun.Initialize();

        _shootTimer = _handGun.ShootRate;

        _handGun.OnReload.AddListener(
            () => StartCoroutine(Reload())
        );

        _gunInventory.AddWeapon(_handGun);

        ChangeGunVisual();
    }

    void Update()
    {
        // TROCA ARMA
        float scroll =
            Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            ChangeWeapon(scroll);
        }

        // RELOAD
        if (!_handGun.IsMelee)
        {
            if (Input.GetButtonDown("Reload"))
            {
                if (_handGun.Ammunation > 0)
                {
                    _handGun.OnReload.Invoke();
                }
            }
        }

        _shootTimer += Time.deltaTime;

        if (_isReloading)
            return;

        if (_shootTimer < _handGun.ShootRate)
            return;

        // ATAQUE
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // MELEE
        if (_handGun.IsMelee)
        {
            if (_shootAudioSource != null &&
                _handGun.ShootSound != null)
            {
                _shootAudioSource.PlayOneShot(
                    _handGun.ShootSound
                );
            }

            if (Physics.Raycast(
                _camera.position,
                _camera.forward,
                out RaycastHit hit,
                _handGun.MeleeRange))
            {
                if (hit.collider.TryGetComponent(
                    out IShootable shootable))
                {
                    shootable.Hitted(
                        _handGun.Damage,
                        hit.point
                    );
                }
            }

            _shootTimer = 0f;

            return;
        }

        // ARMA NORMAL
        if (!_handGun.UseAmmunation())
            return;

        _shootTimer = 0f;

        // SOM
        if (_shootAudioSource != null &&
            _handGun.ShootSound != null)
        {
            _shootAudioSource.PlayOneShot(
                _handGun.ShootSound
            );
        }

        // MUZZLE FLASH
        if (_muzzleFlash != null)
        {
            _muzzleFlash.Stop(
                true,
                ParticleSystemStopBehavior
                .StopEmittingAndClear
            );

            _muzzleFlash.Play();
        }

        // RAYCAST
        if (Physics.Raycast(
            _camera.position,
            _camera.forward,
            out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(
                out IShootable shootable))
            {
                shootable.Hitted(
                    _handGun.Damage,
                    hit.point
                );
            }
        }
    }

    private void ChangeWeapon(float nextIndex)
    {
        if (_gunInventory.Guns.Count <= 1)
            return;

        int currentIndex =
            _gunInventory.Guns.IndexOf(_handGun);

        currentIndex +=
            (int)Mathf.Sign(nextIndex);

        if (currentIndex >= _gunInventory.Guns.Count)
        {
            currentIndex = 0;
        }
        else if (currentIndex < 0)
        {
            currentIndex =
                _gunInventory.Guns.Count - 1;
        }

        _handGun =
            _gunInventory.Guns[currentIndex];

        ChangeGunVisual();
    }

    public void ChangeGunVisual()
    {
        if (_handGunModelParent.childCount > 0)
        {
            Destroy(
                _handGunModelParent
                .GetChild(0)
                .gameObject
            );
        }

        GameObject gun =
            Instantiate(
                _handGun.GunModel,
                _handGunModelParent
            );

        gun.layer =
            LayerMask.NameToLayer("Gun");

        gun.transform.localPosition =
            Vector3.zero;

        _muzzleFlash =
            gun.GetComponentInChildren
            <ParticleSystem>();
    }

    IEnumerator Reload()
    {
        _isReloading = true;

        yield return new WaitForSeconds(
            _handGun.ReloadTime
        );

        _handGun.Reload();

        _shootTimer =
            _handGun.ShootRate;

        _isReloading = false;
    }

    public void AddNewGun(GunElement newGun)
    {
        _handGun = newGun;

        _handGun.Initialize();

        _shootTimer =
            _handGun.ShootRate;

        _handGun.OnReload.AddListener(
            () => StartCoroutine(Reload())
        );

        _gunInventory.AddWeapon(newGun);

        ChangeGunVisual();
    }
}