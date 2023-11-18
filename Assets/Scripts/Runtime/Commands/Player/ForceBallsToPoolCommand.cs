using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ForceBallsToPoolCommand
{
    private PlayerManager _playerManager;
    private PlayerForceData _forceData;

    public ForceBallsToPoolCommand(PlayerManager playerManager, PlayerForceData forceData)
    {
        _playerManager = playerManager;
        _forceData = forceData;
    }

    internal void Execute()
    {
        Transform transform1 = _playerManager.transform;
        Vector3 position1 = transform1.position;
        //Her seferinde _playerManager.transform.position.x yapmamak i�in ��nk� drawcall olu�turuyor.
        //�nce tranformu bir de�i�kende tutuyoruz. Sonra Vector3 tipinde position1 diye de�i�ken olu�turup position'a eri�ip
        //onun �zerinden PlayerManager Gameobjectinin x,y,z pozisyonlar�na eri�iyoruz.
        //Burada forcePos'da yukar� ve ileri y�nl� bir kuvvet olu�turuyoruz.
        Vector3 forcePos = new Vector3(position1.x, position1.y - 1f, position1.z + 0.9f);

        //Burada forcePos konumunda ve 1.35f �ap�nda bir k�re olu�turup,
        //K�reye dokunan veya k�renin i�indeki �arp��t�r�c�lar� hesaplar ve saklar.
        Collider[] colliders = Physics.OverlapSphere(forcePos, 1.7f);

        //Burada Player�n temas etti�i bir tek collectiblelar yok zeminde var, ayr�ca ba�ka �eyler de olabilir, bundan dolay�
        //Sadece Collectable tagine sahip objeleri listede topluyoruz.
        //colliders.Where(col => col.CompareTag("Collectable")).ToList(); burada yapt��� b�t�n colliderlar aras�nda Collectable tagine
        //sahip olanlardan bir liste olu�turup o listeyi d�n�yor.
        //col parametre alan k�s�m oluyor colliders dizisi i�indeki herhangi bir collider� temsil ediyor sonra lambda expression sayesinde
        //parametre olan col �zerinden tag kontrol� yap�p listeye ekliyoruz.
        List<Collider> collectableColliderList = colliders.Where(col => col.CompareTag("Collectable")).ToList();

        //Burada Collectable tagine sahip collider�n Rigidbody komponentine sahip de�il mi o kontrol edilyor ard�ndan
        //rb.AddForce diyerek sadece y ve z ekseninde 
        foreach (Collider col in collectableColliderList)
        {
            if (col.GetComponent<Rigidbody>() == null)
            {
                continue;
            }
            Rigidbody rb = col.GetComponent<Rigidbody>();
            //Burada y ve z ekseninde ForceParameters �zerindeki de�erler ne ise o kadar g�� uyguluyoruz.
            //ForceMode A��klamalar�: ForceMode.Acceleration: H�zlanan demek ama kuvvet uygulanacak objenin mass de�eri hesaba kat�lm�yor.
            //ForceMode.Force: S�rekli artan kuvvet, mass de�eri hesaba kat�l�yor.
            //ForceMode.Impulse: Tek seferli ve mass de�eri hesaba kat�l�yor.
            //ForceMode.VelocityChange: Tek seferli ve mass de�eri hesaba katmaz.
            rb.AddForce(new Vector3(0, _forceData.ForceParameters.y, _forceData.ForceParameters.z), ForceMode.Impulse);
        }
        collectableColliderList.Clear();
    }

    //[DrawGizmo(GizmoType.Selected | GizmoType.Active)]
    //public static void OnDrawGizmos(PlayerManager manager, GizmoType gizmoType)
    //{
    //    Gizmos.color = Color.yellow;
    //    Transform transform1 = manager.transform;
    //    Vector3 position1 = transform1.position;

    //    Gizmos.DrawSphere(new Vector3(position1.x, position1.y - 1f, position1.z + 0.9f), 1.7f);
    //}

   
        
}
