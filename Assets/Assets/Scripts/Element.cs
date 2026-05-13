using UnityEngine;
using UnityEngine.Events;

public class Element
{

}

[System.Serializable]
public class GunElement : Element
{
    public UnityEvent OnReload;

    [SerializeField] private GameObject _gunModel;
    [SerializeField] private string _name;
    [SerializeField] private float _damage;
    [SerializeField] private float _shootRate;
    [SerializeField] private float _ammunation;
    [SerializeField] private float _clipSize;
    [SerializeField] private float _reloadTime;
    [SerializeField] private bool _hasScope;

    [Header("Audio")]
    [SerializeField] private AudioClip _shootSound;

    private float _ammunationClip;

    public GunElement(string name, float damage, float shootRate, float ammunation, float reloadTime)
    {
        _name = name;
        _damage = damage;
        _shootRate = shootRate;
        _ammunation = ammunation;
        _reloadTime = reloadTime;
    }

    public void Initialize()
    {
        _ammunationClip = _clipSize;
    }

    public bool UseAmmunation()
    {
        Debug.Log(_ammunationClip);

        if (_ammunationClip <= 0)
        {
            if (_ammunation > 0)
            {
                OnReload.Invoke();
            }

            return false;
        }

        _ammunationClip--;

        return true;
    }

    public void Reload()
    {
        if (_ammunation <= 0)
            return;

        float ammunationToReload = _clipSize - _ammunationClip;

        if (ammunationToReload <= 0)
            return;

        if (_ammunation < ammunationToReload)
        {
            ammunationToReload = _ammunation;
        }

        _ammunationClip += ammunationToReload;
        _ammunation -= ammunationToReload;
    }

    public string Name { get => _name; }

    public float Damage { get => _damage; }

    public float ShootRate { get => _shootRate; }

    public float Ammunation { get => _ammunation; }

    public float ReloadTime { get => _reloadTime; }

    public bool HasScope { get => _hasScope; }

    public GameObject GunModel { get => _gunModel; }

    public AudioClip ShootSound { get => _shootSound; }
}