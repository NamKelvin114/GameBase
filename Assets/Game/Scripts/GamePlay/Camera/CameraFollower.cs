using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraFollower : MonoBehaviour
{
    private GameObject _target;
    private Quaternion _originalPosition;
    [SerializeField] private float duration;
    [SerializeField] private float magnitude;
    private bool _isShaking;

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag(Constant.Player);
        _originalPosition = transform.rotation;
    }
    private void OnEnable()
    {
        Observer.CameraShake += ShakeCamera;
    }
    private void OnDisable()
    {
        Observer.CameraShake -= ShakeCamera;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(_target.transform.position.x, transform.position.y, _target.transform.position.z);
    }

    public void ShakeCamera()
    {
        _isShaking = true;
        StartCoroutine(Shake(duration, magnitude));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        if (_isShaking)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float offset = Random.Range(-1f, 1.1f) * magnitude;
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, offset, transform.eulerAngles.z);
                elapsed += Time.deltaTime;
                yield return null;
            }
            transform.rotation = _originalPosition;
            _isShaking = false;
        }

    }

    // private bool IsShaking()
    // {
    //     return transform.position != originalPosition;
    // }
}
