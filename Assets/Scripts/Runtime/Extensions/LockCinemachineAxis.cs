using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CinemachineLockAxis
{
    X,
    Y,
    Z
}


[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")]

public class LockCinemachineAxis : CinemachineExtension
{
    public CinemachineLockAxis axis;

    [Tooltip("Lock the Cinemachine Virtual Camera's X Axis position with this specific value")]
    public float XClampValue = 0;

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        switch (axis)
        {
            case CinemachineLockAxis.X:
                if (stage == CinemachineCore.Stage.Body)
                {
                    Vector3 pos = state.RawPosition;
                    pos.x = XClampValue;
                    state.RawPosition = pos;
                }
                break;
            case CinemachineLockAxis.Y:
                if (stage == CinemachineCore.Stage.Body)
                {
                    Vector3 pos = state.RawPosition;
                    pos.y = XClampValue;
                    state.RawPosition = pos;
                }
                break;
            case CinemachineLockAxis.Z:
                if (stage == CinemachineCore.Stage.Body)
                {
                    Vector3 pos = state.RawPosition;
                    pos.z = XClampValue;
                    state.RawPosition = pos;
                }
                break;
            default:
                break;
        }

    }
}
