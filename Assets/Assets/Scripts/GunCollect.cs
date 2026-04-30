using UnityEngine;

public class GunCollect : Item
{
    [SerializeField] private GunElement _attributes;
    public override Element Collect()
    {
        Destroy(gameObject);
        return _attributes;
    }

    protected override void Teste1()
    {
        throw new System.NotImplementedException();
    }

    //Se eu sobrescrevo o método virtual do pai
    //ao chamar no filho, o método do filho é executado
    protected override void Teste2()
    {
        Debug.Log("Teste2");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
