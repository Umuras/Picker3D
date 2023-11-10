using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : Component
{
    //Kullan�m �ekli �u �ekilde singleton olacak scripte gidip miras alacak k�sma MonoSingleton<Script ad�> yazd���n�zda otomatik olarak
    //singleton yap�s� kullanabilir hale geliyor. Ama proje �st�nde fazladan olan ayn� gameobjecti silemiyorsunuz bu yap�da dikkatli olmak laz�m.
    private static T _instance;

    public static T Instance
    {
        get 
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                //Burada birden fazla instance olu�tu�unda o olu�an Gameobjectleri yok etmek i�in b�yle bir kod yazd�k
                //Normalde yok. ��e yaram�yor ��nk� oyunun �al��ma an�nda fazla olan instance gameobjectleri bulamay�p silemiyor.
                //T[] instances = FindObjectsOfType<T>();
                //for (int i = 0; i < instances.Length - 1; i++)
                //{
                //    Destroy(instances[i]);
                //}

                //_instance = instances[0];

                //Burada da e�er instance� bulamazsa o t�r�n o isminde bir gameobject olu�turuyor ve o gameobjecte o scripti
                //komponent olarak at�yor.
                if (_instance == null)
                {
                    GameObject newGameObject = new GameObject(typeof(T).Name);
                    _instance = newGameObject.AddComponent<T>();
                }
            }

            return _instance;
        }
    }


    protected void Awake()
    {
        _instance = this as T;
    }
}
