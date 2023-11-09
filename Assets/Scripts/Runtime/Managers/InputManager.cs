using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Managers
{
    public class InputManager : MonoBehaviour
    {
        private InputData _data;
        private bool _isAvailableForTouch;
        private bool _isFirstTimeTouchTaken;
        private bool _isTouching;

        private float _currentVelocity;
        private float3 _moveVector;
        //Sonundaki soru işareti ref type olduğunu belli ediyor. Geçici değişken demek.
        private Vector2? _mousePosition;

        private OnTouchingFinishedCommand _touchingFinishedCommand;
        private OnTouchingStartedCommand _touchingStartedCommand;
        private OnTouchingContinuesCommand _touchingContinuesCommand;


        private void Awake()
        {
            _data = GetInputData();
            Init();
        }

        private void Init()
        {
            _touchingFinishedCommand = new OnTouchingFinishedCommand(IsPointerOverUIElement);
            _touchingStartedCommand = new OnTouchingStartedCommand(IsPointerOverUIElement);
            _touchingContinuesCommand = new OnTouchingContinuesCommand(IsPointerOverUIElement);
        }

        private InputData GetInputData()
        {
            return Resources.Load<CD_Input>("Data/CD_Input").Data;
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            //OnReset tetiklenerek seviye sebebiyle değişikliğe uğrayan classlar var ise
            //bu değişiklikleri resetlemesi için kullanıyorum sinyali.
            CoreGameSignals.Instance.onReset += OnReset;
            InputSignals.Instance.onEnableInput += OnEnableInput;
            InputSignals.Instance.onDisableInput += OnDisableInput;
            InputSignals.Instance.onTouchingFinished += _touchingFinishedCommand.Execute;
            InputSignals.Instance.onTouchingStarted += _touchingStartedCommand.Execute;
            InputSignals.Instance.onTouchingContinues += _touchingContinuesCommand.Execute;
            //onEnableInput ve onDisableInput delegateini tek seferde kontrol eden delegate yapısı 
            //InputSignals.Instance.onInputStateChanged += OnInputStateChanged;
        }

        //Bu şekilde yazarak inputa true false vererek tek bir methodda yapabiliyoruz ancak
        //bu takip işimizi zorlaştıracağından bu yapıda şuanlık kullanmıyoruz hoca bilelim
        //diye gösterdi.
        //private void OnInputStateChanged(bool state)
        //{
        //    _isAvailableForTouch = state;
        //}

        private void OnDisableInput()
        {
            _isAvailableForTouch = false;
        }

        private void OnEnableInput()
        {
            _isAvailableForTouch = true;
        }

        private void OnReset()
        {
            _isAvailableForTouch = false;
            //Bizim oyunumuz açıldığından oyunumuzda kullandığımız bir durum var, 
            //Örnek olarak oyun başlangıcında tutorial olacak sağa sola kaydırarak pickerı 
            //hareket ettir diye o tarz durumlar için kullanacağız ama Resette gerek yok
            //bunu kullanmak için bir kere kullanılacak ve bir daha işimize yaramayacak.
            //_isFirstTimeTouchTaken = false;
            _isTouching = false;
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onReset -= OnReset;
            InputSignals.Instance.onEnableInput -= OnEnableInput;
            InputSignals.Instance.onDisableInput -= OnDisableInput;
            InputSignals.Instance.onTouchingFinished -= _touchingFinishedCommand.Execute;
            InputSignals.Instance.onTouchingStarted -= _touchingStartedCommand.Execute;
            InputSignals.Instance.onTouchingContinues -= _touchingContinuesCommand.Execute;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
        /*
                private void Update()
                {
                    //Oyuncudan dokunma girdisi alıyor muyuz onu kontrol ediyoruz, eğer alamıyorsak oyun başlamış değil,
                    //o yüzden return edip aşağıdaki kodların çalışmasını engelliyoruz. Aynı zamanda bu bir kod optimizasyonudur. Aşağıdaki
                    //kodlara girditmemen
                    if (!_isAvailableForTouch)
                    {
                        return;
                    }

                    //Burada Mouse sol tıkından basıp elimizi kaldırdığımızda ve eğer UI elemanına değmiyorsak çalışıyor.
                    //Input.GetMouseButtonUp(0) 0 yazıldığında birden fazla parmak ile ekrana basılsa bile ilk tetikleyen ilk basılan
                    //parmaktır. 1 2 3 4 aslında touchların nasıl ilerlediği ile alakalı, kaç tane parmağın değdiği ile alakalı.
                    //Neden Touchları değilde Input.GetMouseButtonları kullanıyoruz çünkü, Touchlar state yaklaşımında daha başarılı, çünkü
                    //Touchın direk phasei var, TouchPhase diye geçiyor. Touchlar editörde çalışmıyor. Sadece mobilde çalışıyor. Ya telefonu
                    //bağlayarak sürekli test edeceksiniz, ya da eski usül Input sistemi üzerinden ilerleyeceksiniz. Input.GetMouseButton ile
                    //Touch arasındada o kadar büyük bir fark yok. Mouselar mobil cihazın işletim sisteminde de mouse olarak alınmış, 
                    if (Input.GetMouseButtonUp(0) && !IsPointerOverUIElement())
                    {
                        _isTouching = false;
                        //? işareti amacı şudur, onInputReleased sinyalinde beni dinleyen biri var mı onun kontrolü yapılmaktadır, eğer var
                        //ise çalıştırılır.
                        InputSignals.Instance.onInputReleased?.Invoke();
                        Debug.LogWarning("Executed ---> OnInputReleased");
                    }

                    //Burada da oyuncu ekrana dokununca çalışacak
                    if (Input.GetMouseButtonDown(0) && !IsPointerOverUIElement())
                    {
                        _isTouching = true;
                        InputSignals.Instance.onInputTaken?.Invoke();
                        Debug.LogWarning("Executed ---> OnInputTaken");
                        //Burada oyuncu ilk dokunduğunda girecek ve çalışacak ondan sonra bir dahaki dokunmalarda çalışmayacak.
                        //Resette sıfırlamamızın sebebi bir daha buraya girmesini istememizden kaynaklı.
                        if (!_isFirstTimeTouchTaken)
                        {
                            _isFirstTimeTouchTaken = true;
                            InputSignals.Instance.onFirstTimeTouchTaken?.Invoke();
                            Debug.LogWarning("Executed ---> OnFirstTimeTouchTaken");
                        }

                        //Bunu yapmamızın sebebi, mouse tarafında deltaposition alma gibi bir şansımız yok, Unity böyle bir sistem yazmamış
                        //delta position'a erişemiyoruz. Ama editörde EventSystemin altında gözüküyor, kod tarafında erişilemiyor, o yüzden
                        //manuel kendimizin yazması lazım.
                        //delta position hesabını yapabilmek adına ilk tıkladığım pozisyonu yeni kordinat sistemindeki originim alacağım.
                        //Yani aslında yapılan şu, belki ekranda -100,200. pozisyona tıklıyorum, kordinat düzleminde, ama orası bizim için 
                        //delta position olmayacak, çünkü biz oradaki harekete göre hesaplayacaksak, sürekli karmaşık bir matematik alacaz
                        //ki onu alabilmemiz için bile onu referanslamamız lazım aslında, oradaki pozisyonu yeni originime alırsam eğer, yani
                        //buradaki değerimi -200,100 olmasına rağmen sanki 0,0 mış gibi kabul edersem, parmağımı sağa veya sola kaydırdığımda
                        //aradaki fark sebebiyle pozitif eksende mi hareket ettirmişim negatif eksende mi hareket ettirmişim anlayabilirim.
                        //Bu da bana deltapositionı verir zaten ve oradaki değerde kaç birim hareket ettirdiğimi, kordinat düzlemi bazında
                        //söyler, bu da aslında bizim ilk başlarda yapmış olduğumuz datanın kontrolü, denetlenmesi ve bütün cihaz gamında 
                        //dengeli bir şekilde işlem yapılması aşamasına getirir. Delta positionı hesaplayabilmek için tutuyoruz bu değeri.
                        _mousePosition = Input.mousePosition;

                    }

                    //Ekrana dokunuyorsak ve ui'a dokunmuyorsak çalışıyor.
                    if (Input.GetMouseButton(0) && !IsPointerOverUIElement())
                    {
                        //Ekrana dokunuyorsak giriyor,
                        if (_isTouching)
                        {
                            if (_mousePosition != null)
                            {
                                //Güncel dokunma konumumuzdan başlangıç dokunma konumumuzu çıkarınca bu bizim deltapositionımız oluyor.
                                //Negatif veya pozitif gelebilir.
                                Vector2 mouseDeltaPos = (Vector2)Input.mousePosition - _mousePosition.Value;
                                //mouseDeltaPos.x konumu benim limitlediğim değerden büyük ise 
                                if (mouseDeltaPos.x > _data.HorizontalInputSpeed)
                                {
                                    _moveVector.x = _data.HorizontalInputSpeed / 10f * mouseDeltaPos.x;
                                }
                                else if (mouseDeltaPos.x < _data.HorizontalInputSpeed)
                                {
                                    _moveVector.x = -_data.HorizontalInputSpeed / 10f * mouseDeltaPos.x;
                                }
                                else
                                {
                                    //Yavaşça 0'a doğru gitmesini sağlayan işlem yapılıyor.
                                    _moveVector.x = Mathf.SmoothDamp(-_moveVector.x, 0f, ref _currentVelocity,
                                        _data.ClampSpeed);
                                }

                                _mousePosition = Input.mousePosition;

                                InputSignals.Instance.onInputDragged?.Invoke(new HorizontalInputParams()
                                {
                                    HorizontalValue = _moveVector.x,
                                    //Yatay eksende limite edilen final değişken tipi ClampValues
                                    ClampValues = _data.ClampValues
                                });
                            }
                        }
                    }
                }
        */

        private void Update()
        {
            if (!_isAvailableForTouch)
            {
                return;
            }

            InputSignals.Instance.onTouchingFinished?.Invoke(_isTouching);
            InputSignals.Instance.onTouchingStarted?.Invoke(_isTouching, _isFirstTimeTouchTaken, _mousePosition);
            InputSignals.Instance.onTouchingContinues?.Invoke(_isTouching, _mousePosition, _data, _moveVector, _currentVelocity);
        }
        //IsPointerOverUIElementin yaptığı iş, Canvastan alacağımız için Inputu, çünkü Canvas ekranın en önünde bulunuyor,
        //Kameranın bile önünde bulunuyor, biz Inputu ancak oradan alabiliriz, EventSystem bu şekilde çalıştığından ötürü,
        //EventSystem Canvas tarafından Input alıyor, özellikle mobil tarafta dokumatik yüzey bazında, haliyle bizimde Canvas
        //üzerinden Input almamız lazım ama burada şu çok önem arz ediyor, UI'a değip değmediğinin kontrolünün yapılması lazım,
        //Eğer UI'a değiyorsak oyunu oynamıyoruz, demektir. Kısacası UI'a değip değmediğimizi kontrol ediyoruz, bu fonksiyonda.
        private bool IsPointerOverUIElement()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current)
            { 
                //Mevcut mouse pozisyonumuzu alıyoruz, UI'a değip değmediğimizi mouse pozisyonun ölçümleyebiliyoruz sadece.
                position = Input.mousePosition
            };
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }
    }
}
