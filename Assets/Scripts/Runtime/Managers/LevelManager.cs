using Assets.Scripts.Data.ValueObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Managerin altýnda mümkün mertebe iþ yapmýyoruz görevlilere yani Commandlere daðýtýyoruz.
public class LevelManager : MonoBehaviour
{
    //Burada levelHolder gameObjecti altýnda levellarýmýzý oluþturacaðýz o yüzden bu deðiþkeni
    //oluþturduk.
    [SerializeField]
    private Transform _levelHolder;

    //Oyundaki toplam seviyeyi referans etmek amacýyla kullanýyoruz.
    [SerializeField]
    private byte totalLevelCount;

    //Çok oynanabilme durumundan dolayý _currentLevelý bytetan shorta çevirdik.
    //Burada çok oynamadan kasýt currentLevel deðerinin 255'i geçme durumu olabilir.
    //shortun aralýðý daha fazla byte'a göre.
    private short _currentLevel;
    //Seviye çekmek için kullanacaðýz. O anki seviyemin datasýný alabiliriz veya bütününü tuta-
    //biliriz.
    private LevelData _levelData;

    private OnLevelLoaderCommand _levelLoaderCommand;
    private OnLevelDestroyerCommand _levelDestroyerCommand;


    private void Awake()
    {
        //Burada LevelDatalarýný çekiyoruz.
       _levelData = GetLevelData();
        //Burada Aktif Level bilgisini alýyoruz.
       _currentLevel = GetActiveLevel();

        Init();
    }

    private void Init()
    {
        //BURADA INITIALIZE ÝÞLEMÝ YAPIYORUZ. Çünkü bu Commandleri oluþturmasý lazým. Kendine
        //Bind etmesi yani baðlamasý gerekiyor. Dependency Injection temeli gibi düþünebiliriz.
        //LevelHolder gameobjecti içerisine yeni leveli oluþturacaðýz.
        _levelLoaderCommand = new OnLevelLoaderCommand(_levelHolder);
        //LevelHolder gameobjecti içerisindeki leveli yok edeceðiz.
        _levelDestroyerCommand = new OnLevelDestroyerCommand(_levelHolder);
    }

    //[Button] Hoca Odin paketini projeye entegre ederek Button Attribute'i sayesinde
    //Inspector panelindeki script üzerinden orada buton oluþturup o butona týklanýldýðýnda
    //bu fonksiyonun çalýþmasý saðlanýyor. Kodun çalýþabilmesi için oyunun çalýþmasý lazým.
    //Þuanki kodun yazým þeklinde ötürü.
    //public void LevelLoader(byte levelIndex)
    //{
    //    _levelLoaderCommand.Execute(levelIndex);
    //}

    ////[Button] Yukarýdakinin aynýsý.
    //public void LevelDestroyer()
    //{
    //    _levelDestroyerCommand.Execute();
    //}

    private LevelData GetLevelData()
    {
        //Burada CD_Level ScriptableObjectinde CD_Level scriptindeki Levels deðiþkenine eriþiyoruz.
        //Levelsda bir liste olduðu için indexer üzerinden þuanki seviyemiz ne ise onu yazarak
        //o seviye için belirlenmiþ PoolDatalara eriþiyoruz.
        return Resources.Load<CD_Level>("Data/CD_Level").Levels[_currentLevel];
    }

    //Ýki tane Aktif level aldýðýmýz yer var, bu GetActiveLevelda classýn üzerinden _currentLevel'a
    //eriþiyoruz. OnGetLevelValuede ise dýþarýdan eriþiyoruz, Observer üzerinden eriþiyoruz.
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
        //CoreGameSignalsda tanýmlý onLevelInitialize UnityActioný içine _levelLoaderCommand.Execute
        //fonksiyonu yükleniyor ve bu UnityAction Invoke edildiði zaman bu fonksiyon çalýþacaktýr.
        //1.kýsým CoreGameSignals.Instance(Singleton Pattern) 2.kýsým onLevelInitialize(Observer Pattern)
        //3.kýsým _levelLoaderCommand.Execute(Command Pattern)
        //Eðer observer pattern yapýyorsun o fonksiyona ve evente "On" eki getirmek zorundasýn!!!
        //Fonksiyonlarýn Observer Pattern yolu ile tetiklendiðini bize gösteriyor ayrýca.
        //Observer Patternin en kuvvetli birden fazla iþlemi tetikliyor olabiliyor olmak.
        // += dediðinde o evente veya delegate birden fazla iþlem yetkisi veriyorsun.
        CoreGameSignals.Instance.onLevelInitialize += _levelLoaderCommand.Execute;
        CoreGameSignals.Instance.onClearActiveLevel += _levelDestroyerCommand.Execute;
        CoreGameSignals.Instance.onGetLevelValue += OnGetLevelValue;
        CoreGameSignals.Instance.onNextLevel += OnNextLevel;
        CoreGameSignals.Instance.onRestartLevel += OnRestartLevel;
    }
    //Observer Pattern için yazdýðýn fonksiyonlarý SubscribeEvents ve UnSubscribeEvents arasýna
    //yazarsan hatýrlamasý kolay olur.

    private byte OnGetLevelValue()
    {
        //PoolControllerde GetPoolData derken o anki seviyemize ihtiyaç duyuyoruz ve bu fonksiyonu kullanýyoruz
        //Bu iþlemizi yapmamýzýn Startdaki açýklamanýn aynýsý yüzünden currentLevel sürekli artarak devam ediyor ama bizim yaptýðýmýz
        //seviye ondan az olunca hata oluþacaðý için bu þekilde mod alýyoruz.
        return (byte)((byte)_currentLevel % totalLevelCount);
    }

    private void OnNextLevel()
    {
        //Burada _currentLeveli bir arttýrarak bir sonraki seviyeye geçisini saðlýyoruz.
        //OnClearActiveLevelde LevelHolder içinde bulunan eski leveli siliyoruz.
        //OnResette Resetleme iþlemi yapýyoruz.
        //onLevelInitializedada o leveli LevelHolder gameobjecti altýna Instantiate ediyoruz.
        _currentLevel++;
        CoreGameSignals.Instance.onClearActiveLevel?.Invoke();
        CoreGameSignals.Instance.onReset?.Invoke();
        CoreGameSignals.Instance.onLevelInitialize?.Invoke((byte)(_currentLevel % totalLevelCount));
    }

    private void OnRestartLevel()
    {
        //Burada da ayný levelý tekrar yüklüyoruz.
        CoreGameSignals.Instance.onClearActiveLevel?.Invoke();
        CoreGameSignals.Instance.onReset?.Invoke();
        CoreGameSignals.Instance.onLevelInitialize?.Invoke((byte)(_currentLevel % totalLevelCount));
    }

    private void UnSubscribeEvents()
    {
        //CoreGameSignalsda tanýmlý onLevelInitialize UnityActioný içine _levelLoaderCommand.Execute
        //fonksiyonu yükleniyor ve bu UnityAction Invoke edildiði zaman bu fonksiyon çalýþacaktýr.
        //1.kýsým CoreGameSignals.Instance(Singleton Pattern) 2.kýsým onLevelInitialize(Observer Pattern)
        //3.kýsým _levelLoaderCommand.Execute(Command Pattern)
        //Eðer observer pattern yapýyorsun o fonksiyona ve evente "On" eki getirmek zorundasýn!!!
        //Fonksiyonlarýn Observer Pattern yolu ile tetiklendiðini bize gösteriyor ayrýca.
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
        //Seviyelerimizin oluþma sürecini bizim Level Managerýmýzýn yönetmesi lazým.
        //Burada bunu yapmamýzýn sebebi diyelim biz sadece 20 seviye yaptýk ama oyunda oyuncu 20.seviyi geçti
        //Oyunun bitmemesi için çünkü eðer 21'e gelip sadece currentLevelý gönderseydik oyunda 21.seviye
        //olmadýðý için hata verecekti ama totalLevelCounta göre modu alýndýðý zaman kaçýncý seviyede olursa
        //olsun 20 den yukarý bir sonuç çýkmayacak. Örnek 21 % 20 = 1 1.seviye oyuna gelecek
        //39 % 20 = 19 uncu seviye oyuna gelecek þeklinde.
        CoreGameSignals.Instance.onLevelInitialize?.Invoke((byte)(_currentLevel % totalLevelCount));
        CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Start, 1);
    }

    
}
