using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        Vector3 forcePos = new Vector3(position1.x, position1.y + 1f, position1.z + 1f);

        Collider[] colliders = Physics.OverlapSphere(forcePos, 1.35f);

        List<Collider> collectableColliderList = colliders.Where(col => col.CompareTag("Collectable")).ToList();

        foreach (Collider col in collectableColliderList)
        {
            if (col.GetComponent<Rigidbody>() == null)
            {
                continue;
            }
            Rigidbody rb = col.GetComponent<Rigidbody>();
            rb.AddForce(new Vector3(0, _forceData.ForceParameters.y, _forceData.ForceParameters.z), ForceMode.Impulse);
        }

        collectableColliderList.Clear();
    }
}
