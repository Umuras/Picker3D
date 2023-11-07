using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class OnTouchingContinuesCommand
{
    public delegate bool IsPointerOverUIElement();
    public IsPointerOverUIElement _isPointerOverUIElement;

    public OnTouchingContinuesCommand(IsPointerOverUIElement isPointerOverUIElement)
    {
        _isPointerOverUIElement = isPointerOverUIElement;
    }

    public void Execute(bool isTouching, Vector2? mousePosition, InputData _data, float3 moveVector, float currentVelocity)
    {
        if (Input.GetMouseButton(0) && !_isPointerOverUIElement.Invoke())
        {
            if (isTouching)
            {
                if (mousePosition != null)
                {
                    Vector2 mouseDeltaPos = (Vector2)Input.mousePosition - mousePosition.Value;
                    if (mouseDeltaPos.x > _data.HorizontalInputSpeed)
                    {
                        moveVector.x = _data.HorizontalInputSpeed / 10f * mouseDeltaPos.x;
                    }
                    else if (mouseDeltaPos.x < _data.HorizontalInputSpeed)
                    {
                        moveVector.x = -_data.HorizontalInputSpeed / 10f * mouseDeltaPos.x;
                    }
                    else
                    {
                        moveVector.x = Mathf.SmoothDamp(-moveVector.x, 0f, ref currentVelocity,
                            _data.ClampSpeed);
                    }

                    mousePosition = Input.mousePosition;

                    InputSignals.Instance.onInputDragged?.Invoke(new HorizontalInputParams()
                    {
                        HorizontalValue = moveVector.x,
                        ClampValues = _data.ClampValues
                    });
                    Debug.LogWarning("Executed---> OnInputDragged");
                }
            }
        }
    }
}
