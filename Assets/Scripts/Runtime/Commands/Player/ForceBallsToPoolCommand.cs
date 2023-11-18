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
        //Her seferinde _playerManager.transform.position.x yapmamak için çünkü drawcall oluþturuyor.
        //Önce tranformu bir deðiþkende tutuyoruz. Sonra Vector3 tipinde position1 diye deðiþken oluþturup position'a eriþip
        //onun üzerinden PlayerManager Gameobjectinin x,y,z pozisyonlarýna eriþiyoruz.
        //Burada forcePos'da yukarý ve ileri yönlü bir kuvvet oluþturuyoruz.
        Vector3 forcePos = new Vector3(position1.x, position1.y - 1f, position1.z + 0.9f);

        //Burada forcePos konumunda ve 1.35f çapýnda bir küre oluþturup,
        //Küreye dokunan veya kürenin içindeki çarpýþtýrýcýlarý hesaplar ve saklar.
        Collider[] colliders = Physics.OverlapSphere(forcePos, 1.7f);

        //Burada Playerýn temas ettiði bir tek collectiblelar yok zeminde var, ayrýca baþka þeyler de olabilir, bundan dolayý
        //Sadece Collectable tagine sahip objeleri listede topluyoruz.
        //colliders.Where(col => col.CompareTag("Collectable")).ToList(); burada yaptýðý bütün colliderlar arasýnda Collectable tagine
        //sahip olanlardan bir liste oluþturup o listeyi dönüyor.
        //col parametre alan kýsým oluyor colliders dizisi içindeki herhangi bir colliderý temsil ediyor sonra lambda expression sayesinde
        //parametre olan col üzerinden tag kontrolü yapýp listeye ekliyoruz.
        List<Collider> collectableColliderList = colliders.Where(col => col.CompareTag("Collectable")).ToList();

        //Burada Collectable tagine sahip colliderýn Rigidbody komponentine sahip deðil mi o kontrol edilyor ardýndan
        //rb.AddForce diyerek sadece y ve z ekseninde 
        foreach (Collider col in collectableColliderList)
        {
            if (col.GetComponent<Rigidbody>() == null)
            {
                continue;
            }
            Rigidbody rb = col.GetComponent<Rigidbody>();
            //Burada y ve z ekseninde ForceParameters üzerindeki deðerler ne ise o kadar güç uyguluyoruz.
            //ForceMode Açýklamalarý: ForceMode.Acceleration: Hýzlanan demek ama kuvvet uygulanacak objenin mass deðeri hesaba katýlmýyor.
            //ForceMode.Force: Sürekli artan kuvvet, mass deðeri hesaba katýlýyor.
            //ForceMode.Impulse: Tek seferli ve mass deðeri hesaba katýlýyor.
            //ForceMode.VelocityChange: Tek seferli ve mass deðeri hesaba katmaz.
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
