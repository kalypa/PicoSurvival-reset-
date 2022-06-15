using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    private Transform _cameraTransform = null;
    public GameObject _objTarget = null;
    private Transform _objTargetTransform = null;

    public float _distance = 6;

    public float _height = 1.75f;

    public float _heightDamp = 6.0f;
    public float _rotateDamp = 6.0f;
    private void LateUpdate()
    {
        if (_objTarget == null)
        {
            return;
        }

        if (_objTargetTransform == null)
        {
            _objTargetTransform = _objTarget.transform;
        }

        ThirdCamera();

    }

    void Start()
    {
        _cameraTransform = GetComponent<Transform>();

        if (_objTarget != null)
        {
            _objTargetTransform = _objTarget.GetComponent<Transform>();
        }
    }
    /// <summary>
    /// 3ÀÎÄª Ä«¸Þ¶ó
    /// </summary>
    void ThirdCamera()
    {
        float objTargetRotationAngle = _objTargetTransform.eulerAngles.y;
        float objHeight = _objTargetTransform.position.y + _height;

        float nowRotationAngle = _cameraTransform.eulerAngles.y;
        float nowHeight = _cameraTransform.position.y;

        nowRotationAngle = Mathf.LerpAngle(nowRotationAngle, objTargetRotationAngle, _rotateDamp * Time.deltaTime);
        nowHeight = Mathf.Lerp(nowHeight, objHeight, _heightDamp * Time.deltaTime);
        Quaternion nowRotation = Quaternion.Euler(0, nowRotationAngle, 0);

        _cameraTransform.position = _objTargetTransform.position;
        _cameraTransform.position -= nowRotation * Vector3.forward * _distance;

        _cameraTransform.position = new Vector3(_cameraTransform.position.x, nowHeight, _cameraTransform.position.z);
        _cameraTransform.LookAt(_objTargetTransform);
    }
}
