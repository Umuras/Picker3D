using System.Collections;
using System.Collections.Generic;
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

    }
}
