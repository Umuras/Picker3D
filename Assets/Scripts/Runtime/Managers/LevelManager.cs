using Assets.Scripts.Data.ValueObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Managerin alt�nda m�mk�n mertebe i� yapm�yoruz g�revlilere yani Commandlere da��t�yoruz.
public class LevelManager : MonoBehaviour
{
    //Burada levelHolder gameObjecti alt�nda levellar�m�z� olu�turaca��z o y�zden bu de�i�keni
    //olu�turduk.
    [SerializeField]
    private Transform _levelHolder;

    //Oyundaki toplam seviyeyi referans etmek amac�yla kullan�yoruz.
    [SerializeField]
    private byte totalLevelCount;

    //�ok oynanabilme durumundan dolay� _currentLevel� bytetan shorta �evirdik.
    //Burada �ok oynamadan kas�t currentLevel de�erinin 255'i ge�me durumu olabilir.
    //shortun aral��� daha fazla byte'a g�re.
    private short _currentLevel;
    //Seviye �ekmek i�in kullanaca��z. O anki seviyemin datas�n� alabiliriz veya b�t�n�n� tuta-
    //biliriz.
    private LevelData _levelData;

    private OnLevelLoaderCommand _levelLoaderCommand;
    private OnLevelDestroyerCommand _levelDestroyerCommand;


    private void Awake()
    {
        //Burada LevelDatalar�n� �ekiyoruz.
       _levelData = GetLevelData();
        //Burada Aktif Level bilgisini al�yoruz.
       _currentLevel = GetActiveLevel();

        Init();
    }

    private void Init()
    {
        //BURADA INITIALIZE ��LEM� YAPIYORUZ. ��nk� bu Commandleri olu�turmas� laz�m. Kendine
        //Bind etmesi yani ba�lamas� gerekiyor. Dependency Injection temeli gibi d���nebiliriz.
        //LevelHolder gameobjecti i�erisine yeni leveli olu�turaca��z.
        _levelLoaderCommand = new OnLevelLoaderCommand(_levelHolder);
        //LevelHolder gameobjecti i�erisindeki leveli yok edece�iz.
        _levelDestroyerCommand = new OnLevelDestroyerCommand(_levelHolder);
    }

    //[Button] Hoca Odin paketini projeye entegre ederek Button Attribute'i sayesinde
    //Inspector panelindeki script �zerinden orada buton olu�turup o butona t�klan�ld���nda
    //bu fonksiyonun �al��mas� sa�lan�yor. Kodun �al��abilmesi i�in oyunun �al��mas� laz�m.
    //�uanki kodun yaz�m �eklinde �t�r�.
    //public void LevelLoader(byte levelIndex)
    //{
    //    _levelLoaderCommand.Execute(levelIndex);
    //}

    ////[Button] Yukar�dakinin ayn�s�.
    //public void LevelDestroyer()
    //{
    //    _levelDestroyerCommand.Execute();
    //}

    private LevelData GetLevelData()
    {
        //Burada CD_Level ScriptableObjectinde CD_Level scriptindeki Levels de�i�kenine eri�iyoruz.
        //Levelsda bir liste oldu�u i�in indexer �zerinden �uanki seviyemiz ne ise onu yazarak
        //o seviye i�in belirlenmi� PoolDatalara eri�iyoruz.
        return Resources.Load<CD_Level>("Data/CD_Level").Levels[_currentLevel];
    }

    //�ki tane Aktif level ald���m�z yer var, bu GetActiveLevelda class�n �zerinden _currentLevel'a
    //eri�iyoruz. OnGetLevelValuede ise d��ar�dan eri�iyoruz, Observer �zerinden eri�iyoruz.
    private byte GetActiveLevel()
    {
        return (byte)_currentLevel;
    }


    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        //CoreGameSignalsda tan�ml� onLevelInitialize UnityAction� i�ine _levelLoaderCommand.Execute
        //fonksiyonu y�kleniyor ve bu UnityAction Invoke edildi�i zaman bu fonksiyon �al��acakt�r.
        //1.k�s�m CoreGameSignals.Instance(Singleton Pattern) 2.k�s�m onLevelInitialize(Observer Pattern)
        //3.k�s�m _levelLoaderCommand.Execute(Command Pattern)
        //E�er observer pattern yap�yorsun o fonksiyona ve evente "On" eki getirmek zorundas�n!!!
        //Fonksiyonlar�n Observer Pattern yolu ile tetiklendi�ini bize g�steriyor ayr�ca.
        //Observer Patternin en kuvvetli birden fazla i�lemi tetikliyor olabiliyor olmak.
        // += dedi�inde o evente veya delegate birden fazla i�lem yetkisi veriyorsun.
        CoreGameSignals.Instance.onLevelInitialize += _levelLoaderCommand.Execute;
        CoreGameSignals.Instance.onClearActiveLevel += _levelDestroyerCommand.Execute;
        CoreGameSignals.Instance.onGetLevelValue += OnGetLevelValue;
        CoreGameSignals.Instance.onNextLevel += OnNextLevel;
        CoreGameSignals.Instance.onRestartLevel += OnRestartLevel;
    }
    //Observer Pattern i�in yazd���n fonksiyonlar� SubscribeEvents ve UnSubscribeEvents aras�na
    //yazarsan hat�rlamas� kolay olur.

    private byte OnGetLevelValue()
    {
        //PoolControllerde GetPoolData derken o anki seviyemize ihtiya� duyuyoruz ve bu fonksiyonu kullan�yoruz
        //Bu i�lemizi yapmam�z�n Startdaki a��klaman�n ayn�s� y�z�nden currentLevel s�rekli artarak devam ediyor ama bizim yapt���m�z
        //seviye ondan az olunca hata olu�aca�� i�in bu �ekilde mod al�yoruz.
        return (byte)((byte)_currentLevel % totalLevelCount);
    }

    private void OnNextLevel()
    {
        //Burada _currentLeveli bir artt�rarak bir sonraki seviyeye ge�isini sa�l�yoruz.
        //OnClearActiveLevelde LevelHolder i�inde bulunan eski leveli siliyoruz.
        //OnResette Resetleme i�lemi yap�yoruz.
        //onLevelInitializedada o leveli LevelHolder gameobjecti alt�na Instantiate ediyoruz.
        _currentLevel++;
        CoreGameSignals.Instance.onClearActiveLevel?.Invoke();
        CoreGameSignals.Instance.onReset?.Invoke();
        CoreGameSignals.Instance.onLevelInitialize?.Invoke((byte)(_currentLevel % totalLevelCount));
    }

    private void OnRestartLevel()
    {
        //Burada da ayn� level� tekrar y�kl�yoruz.
        CoreGameSignals.Instance.onClearActiveLevel?.Invoke();
        CoreGameSignals.Instance.onReset?.Invoke();
        CoreGameSignals.Instance.onLevelInitialize?.Invoke((byte)(_currentLevel % totalLevelCount));
    }

    private void UnSubscribeEvents()
    {
        //CoreGameSignalsda tan�ml� onLevelInitialize UnityAction� i�ine _levelLoaderCommand.Execute
        //fonksiyonu y�kleniyor ve bu UnityAction Invoke edildi�i zaman bu fonksiyon �al��acakt�r.
        //1.k�s�m CoreGameSignals.Instance(Singleton Pattern) 2.k�s�m onLevelInitialize(Observer Pattern)
        //3.k�s�m _levelLoaderCommand.Execute(Command Pattern)
        //E�er observer pattern yap�yorsun o fonksiyona ve evente "On" eki getirmek zorundas�n!!!
        //Fonksiyonlar�n Observer Pattern yolu ile tetiklendi�ini bize g�steriyor ayr�ca.
        CoreGameSignals.Instance.onLevelInitialize -= _levelLoaderCommand.Execute;
        CoreGameSignals.Instance.onClearActiveLevel -= _levelDestroyerCommand.Execute;
        CoreGameSignals.Instance.onGetLevelValue -= OnGetLevelValue;
        CoreGameSignals.Instance.onNextLevel -= OnNextLevel;
        CoreGameSignals.Instance.onRestartLevel -= OnRestartLevel;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    private void Start()
    {
        //Seviyelerimizin olu�ma s�recini bizim Level Manager�m�z�n y�netmesi laz�m.
        //Burada bunu yapmam�z�n sebebi diyelim biz sadece 20 seviye yapt�k ama oyunda oyuncu 20.seviyi ge�ti
        //Oyunun bitmemesi i�in ��nk� e�er 21'e gelip sadece currentLevel� g�nderseydik oyunda 21.seviye
        //olmad��� i�in hata verecekti ama totalLevelCounta g�re modu al�nd��� zaman ka��nc� seviyede olursa
        //olsun 20 den yukar� bir sonu� ��kmayacak. �rnek 21 % 20 = 1 1.seviye oyuna gelecek
        //39 % 20 = 19 uncu seviye oyuna gelecek �eklinde.
        CoreGameSignals.Instance.onLevelInitialize?.Invoke((byte)(_currentLevel % totalLevelCount));
        CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Start, 1);
    }

    
}
