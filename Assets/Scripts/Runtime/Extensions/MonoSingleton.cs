using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//T evrensel anlamýna geliyor. var gibi düþünebilirsin. : MonoBehaviour where T : Component yaparak T yi bir Component yaptýk.
//Bütün classlar Component tipinde. Çünkü çoðu classýmýz Monobehaviourdan türüyor. MonoBehivour bir Behaviour Behaivourda bir Component
// public class MonoSingleton<T> : MonoBehaviour where T : Component burada MonoSingleton<T> MonoBehaviourdan türüyor ve T sýnýfýda Component sýnýfýndan türemiþ bir sýnýf olmalýdýr
//anlamýna geliyor.
public class MonoSingleton<T> : MonoBehaviour where T : Component
{
    //Kullaným þekli þu þekilde singleton olacak scripte gidip miras alacak kýsma MonoSingleton<Script adý> yazdýðýnýzda otomatik olarak
    //singleton yapýsý kullanabilir hale geliyor. Ama proje üstünde fazladan olan ayný gameobjecti silemiyorsunuz bu yapýda dikkatli olmak lazým.
    private static T _instance;

    //Burada Construction süreci gibi yapýyoruz diðer region türüne göre
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
                    //Gameobjecti Miras olarak eklendiði script ne ise onun isminde üretiyor örnek InputSignals : MonoSingleton<InputSignals>
                    //Bu þekilde InputSignals ismini baz alýyor ve component olarak da InputSignals scriptini üzerine ekliyor.
                    GameObject newGameObject = new GameObject(typeof(T).Name);
                    _instance = newGameObject.AddComponent<T>();
                }
            }

            return _instance;
        }
    }

    //Awakein tetiklenmesinin sebebi InputSignalsde bu sýnýftan türettiðimiz için ve oyundada InputSignals bir Gameobject üzerinde bulunduðu için oluyor.
    protected void Awake()
    {
        //InputSignals : MonoSingleton<InputSignals> bu þekilde bir script miras olarak aldýðýnda buranýn Awakeinde T tip ne ise instance'a yükleniyor.
        //MonoSingleton<T> sýnýfýný InputSignals'a dönüþtürüyor, cast ediyor. this = MonoSingleton<T> sýnýfý T de InputSignalsdýr.
        _instance = this as T;
    }
}
