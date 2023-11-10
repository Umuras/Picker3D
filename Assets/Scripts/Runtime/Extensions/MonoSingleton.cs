using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : Component
{
    //Kullaným þekli þu þekilde singleton olacak scripte gidip miras alacak kýsma MonoSingleton<Script adý> yazdýðýnýzda otomatik olarak
    //singleton yapýsý kullanabilir hale geliyor. Ama proje üstünde fazladan olan ayný gameobjecti silemiyorsunuz bu yapýda dikkatli olmak lazým.
    private static T _instance;

    public static T Instance
    {
        get 
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                //Burada birden fazla instance oluþtuðunda o oluþan Gameobjectleri yok etmek için böyle bir kod yazdýk
                //Normalde yok. Ýþe yaramýyor çünkü oyunun çalýþma anýnda fazla olan instance gameobjectleri bulamayýp silemiyor.
                //T[] instances = FindObjectsOfType<T>();
                //for (int i = 0; i < instances.Length - 1; i++)
                //{
                //    Destroy(instances[i]);
                //}

                //_instance = instances[0];

                //Burada da eðer instanceý bulamazsa o türün o isminde bir gameobject oluþturuyor ve o gameobjecte o scripti
                //komponent olarak atýyor.
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
