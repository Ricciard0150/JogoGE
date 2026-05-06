using UnityEngine;

public class EnemyStatus : MonoBehaviour, IShootable
{
    [SerializeField] private GameObject _bloodEffect;
    [SerializeField] float _lifeMax = 2;
    private float _currentLife;

    private bool isColliding;
    public void Hitted(float damage, Vector3 shootPoint)
    {
        _currentLife -= damage;

        GameObject blood = Instantiate(_bloodEffect, shootPoint, Quaternion.LookRotation(shootPoint - transform.position));
        blood.transform.SetParent(transform);
        if (_currentLife > 0)
            return;

        Destroy(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentLife = _lifeMax;
       
    }
    public void OnCollisionEnter(Collision collision)
    {
       if(collision.gameObject.TryGetComponent(out IDamageable dameageble))
        {
            dameageble.Damage(20);
            print("collided");
        }
    }
}
