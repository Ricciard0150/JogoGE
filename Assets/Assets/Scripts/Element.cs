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
    [SerializeField] private float _ammunation;//Munição total da arma para referência pro jogo
    [SerializeField] private float _clipSize;//Quantidade de balas que o pente suporta
    [SerializeField] private float _reloadTime;//Tempo que leva para recarregar a arma
    [SerializeField] private bool _hasScope;
    private float _ammunationClip;//Pente atual sendo utilizado até ter que puxar mais

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
        return true;//Retorna true se a bala foi utilizada com sucesso
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
    public bool HasScope { get => _hasScope;}
    public GameObject GunModel { get => _gunModel;}
}
