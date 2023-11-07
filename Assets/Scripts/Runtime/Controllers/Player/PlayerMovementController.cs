using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    private new Rigidbody rigidbody;
    //Normalde PlayerMeshController objeyi büyüteceðiz ve collider deðiþecek bu senaryoda buraya da onun o boyutunu aktarabilmek için oluþturuyoruz
    //ama burda o þekilde yapmayacaðýmýz için kullanmayacaðýz.
    //[SerializeField]
    //private new Collider collider;

    private PlayerMovementData _data;
    private bool _isReadyToMove;
    private bool _isReadyToPlay;
    //Player saða sola hareketindeki x deðeri parmaðýmýzýn x eksenindeki hareketi
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
            //Ýleri yönlü ve yatay yönlü olarak player duracak.
            StopPlayer();
            return;
        }

        if (_isReadyToMove)
        {
            //Yatay yönlü hareket edebilecek player
            MovePlayer();
        }
        else
        {
            //Yatay yönlü hareketi durduracaðýz.
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
        //Yatay eksende hareket hýzýný 0'lýyoruz y gravity deðerini alýyor, z de ileri yönlü hýz ne ise ona devam ediyor.
        rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, _data.ForwardSpeed);
        rigidbody.angularVelocity = Vector3.zero;
    }

    private void MovePlayer()
    {
        //Burada deðiþkene almamýzýn sebebi drawcallý önlemek için her seferinde rigidbody komponentine eriþmek yerine referanslýyoruz.
        Vector3 velocity = rigidbody.velocity;
        //Burada ise Playerýn x ve z eksenindeki hýzýný belirliyoruz
        velocity = new Vector3(_xValue * _data.SidewaySpeed, velocity.y, _data.ForwardSpeed);
        rigidbody.velocity = velocity;

        Vector3 position1 = rigidbody.position;
        Vector3 position;
        //Playerin x ve y sýnýr pozisyonlarýný belirliyoruz, x ekseninde -3,3 arsýnda hareket edebiliyor, y ve z de o anki pozisyonu ne ise o geliyor
        position = new Vector3(Math.Clamp(position1.x, _clampValues.x, _clampValues.y), (position = rigidbody.position).y, position.z);
        rigidbody.position = position;

        //11.23te kaldýn
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
        //Minigamede _isReadyToPlay true olacak belli bir mesafe gidip yavaþlayarak duracak.
    }
}
