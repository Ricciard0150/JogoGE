using UnityEngine;

public abstract class Item : MonoBehaviour, ICollectable
{
    public abstract Element Collect();

    //MÉTODOS ABSTRATOS
    //Força os filhos a implementarem
    //Usado quando todos os filhos usam, mas com comportamentos diferentes
    //Não declara corpo, apenas a assinatura
    protected abstract void Teste1();

    //MÉTODOS VIRTUAIS
    //Permite que os filhos sobrescrevam, mas não obriga
    //Quando apenas alguns dos filhos tem comportamento diferente
    protected virtual void Teste2()
    {
        //Corpo do método
    }

    //MÉTODOS NORMAIS
    //Quando todos os filhos tem o mesmo comportamento
    protected void Teste3()
    {

    }
}
