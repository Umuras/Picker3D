using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    private new Rigidbody rigidbody;
    //Normalde PlayerMeshController objeyi b�y�tece�iz ve collider de�i�ecek bu senaryoda buraya da onun o boyutunu aktarabilmek i�in olu�turuyoruz
    //ama burda o �ekilde yapmayaca��m�z i�in kullanmayaca��z.
    //[SerializeField]
    //private new Collider collider;

    private PlayerMovementData _data;
    private bool _isReadyToMove;
    private bool _isReadyToPlay;
    //Player sa�a sola hareketindeki x de�eri parma��m�z�n x eksenindeki hareketi
    private float _xValue;

    private float2 _clampValues;


    internal void SetData(PlayerMovementData data)
    {
        _data = data;
    }

    private void FixedUpdate()
    {
        if (!_isReadyToPlay)
        {
            //�leri y�nl� ve yatay y�nl� olarak player duracak.
            StopPlayer();
            return;
        }

        if (_isReadyToMove)
        {
            //Yatay y�nl� hareket edebilecek player
            MovePlayer();
        }
        else
        {
            //Yatay y�nl� hareketi durduraca��z.
            StopPlayerHorizontally();
        }
    }

    private void StopPlayer()
    {
        //Hareket
        rigidbody.velocity = Vector3.zero;
        //Rotasyon
        rigidbody.angularVelocity = Vector3.zero;
    }
    private void StopPlayerHorizontally()
    {
        //Yatay eksende hareket h�z�n� 0'l�yoruz y gravity de�erini al�yor, z de ileri y�nl� h�z ne ise ona devam ediyor.
        rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, _data.ForwardSpeed);
        rigidbody.angularVelocity = Vector3.zero;
    }

    private void MovePlayer()
    {
        //Burada de�i�kene almam�z�n sebebi drawcall� �nlemek i�in her seferinde rigidbody komponentine eri�mek yerine referansl�yoruz.
        Vector3 velocity = rigidbody.velocity;
        //Burada ise Player�n x ve z eksenindeki h�z�n� belirliyoruz
        velocity = new Vector3(_xValue * _data.SidewaySpeed, velocity.y, _data.ForwardSpeed);
        rigidbody.velocity = velocity;

        Vector3 position1 = rigidbody.position;
        Vector3 position;
        //Playerin x ve y s�n�r pozisyonlar�n� belirliyoruz, x ekseninde -3,3 ars�nda hareket edebiliyor, y ve z de o anki pozisyonu ne ise o geliyor
        position = new Vector3(Math.Clamp(position1.x, _clampValues.x, _clampValues.y), (position = rigidbody.position).y, position.z);
        rigidbody.position = position;

        //11.23te kald�n
    }

    internal void IsReadyToPlay(bool condition)
    {
        _isReadyToPlay = condition;
    }

    internal void IsReadyToMove(bool condition) 
    {
        _isReadyToMove = condition;
    }

    internal void UpdateInputParams(HorizontalInputParams inputParams)
    {
        _xValue = inputParams.HorizontalValue;
        _clampValues = inputParams.ClampValues;
    }

    internal void OnReset()
    {
        StopPlayer();
        _isReadyToMove = false;
        _isReadyToPlay = false;
        //Minigamede _isReadyToPlay true olacak belli bir mesafe gidip yava�layarak duracak.
    }
}
