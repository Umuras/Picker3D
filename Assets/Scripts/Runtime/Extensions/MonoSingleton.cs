using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//T evrensel anlam�na geliyor. var gibi d���nebilirsin. : MonoBehaviour where T : Component yaparak T yi bir Component yapt�k.
//B�t�n classlar Component tipinde. ��nk� �o�u class�m�z Monobehaviourdan t�r�yor. MonoBehivour bir Behaviour Behaivourda bir Component
// public class MonoSingleton<T> : MonoBehaviour where T : Component burada MonoSingleton<T> MonoBehaviourdan t�r�yor ve T s�n�f�da Component s�n�f�ndan t�remi� bir s�n�f olmal�d�r
//anlam�na geliyor.
public class MonoSingleton<T> : MonoBehaviour where T : Component
{
    //Kullan�m �ekli �u �ekilde singleton olacak scripte gidip miras alacak k�sma MonoSingleton<Script ad�> yazd���n�zda otomatik olarak
    //singleton yap�s� kullanabilir hale geliyor. Ama proje �st�nde fazladan olan ayn� gameobjecti silemiyorsunuz bu yap�da dikkatli olmak laz�m.
    private static T _instance;

    //Burada Construction s�reci gibi yap�yoruz di�er region t�r�ne g�re
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
                    //Gameobjecti Miras olarak eklendi�i script ne ise onun isminde �retiyor �rnek InputSignals : MonoSingleton<InputSignals>
                    //Bu �ekilde InputSignals ismini baz al�yor ve component olarak da InputSignals scriptini �zerine ekliyor.
                    GameObject newGameObject = new GameObject(typeof(T).Name);
                    _instance = newGameObject.AddComponent<T>();
                }
            }

            return _instance;
        }
    }

    //Awakein tetiklenmesinin sebebi InputSignalsde bu s�n�ftan t�retti�imiz i�in ve oyundada InputSignals bir Gameobject �zerinde bulundu�u i�in oluyor.
    protected void Awake()
    {
        //InputSignals : MonoSingleton<InputSignals> bu �ekilde bir script miras olarak ald���nda buran�n Awakeinde T tip ne ise instance'a y�kleniyor.
        //MonoSingleton<T> s�n�f�n� InputSignals'a d�n��t�r�yor, cast ediyor. this = MonoSingleton<T> s�n�f� T de InputSignalsd�r.
        _instance = this as T;
    }
}
