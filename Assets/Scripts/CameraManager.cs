using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour {
    #region Variables

    private Vector2 _delta;

    private bool _isMoving;
    private bool _isRotating;
    private bool _isBusy;

    private float _xRotation;

    [SerializeField] private float movementSpeed = 10.0f;
    [SerializeField] private float rotationSpeed = 0.5f;

    [SerializeField] private List<GameObject> _walls;

    #endregion

    private void Awake() {
        _xRotation = transform.rotation.eulerAngles.x;
    }

    public void OnLook(InputAction.CallbackContext context) {
        _delta = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context) {
        _isMoving = context.started || context.performed;
    }

    public void OnRotate(InputAction.CallbackContext context) {
        if (_isBusy) return;

        _isRotating = context.started || context.performed;

        if (context.canceled) {
            _isBusy = true;
            SnapRotation();
        }
    }

    private void LateUpdate() {
        if (_isMoving) {
            var position = transform.right * (_delta.x * -movementSpeed);
            transform.position += position * Time.deltaTime;
        }

        if (_isRotating) {
            transform.Rotate(new Vector3(_xRotation, -_delta.x * rotationSpeed, 0.0f));
            transform.rotation = Quaternion.Euler(_xRotation, transform.rotation.eulerAngles.y, 0.0f);
        }


        if (Mathf.Approximately(transform.rotation.eulerAngles.y, 45.0f)) {
            _walls[0].GetComponentInChildren<Renderer>().enabled = false;
            _walls[1].GetComponentInChildren<Renderer>().enabled = false;
            _walls[2].GetComponentInChildren<Renderer>().enabled = true;
            _walls[3].GetComponentInChildren<Renderer>().enabled = true;

        } else if (Mathf.Approximately(transform.rotation.eulerAngles.y, 315.0f)) {
            _walls[0].GetComponentInChildren<Renderer>().enabled = true;
            _walls[1].GetComponentInChildren<Renderer>().enabled = false;
            _walls[2].GetComponentInChildren<Renderer>().enabled = false;
            _walls[3].GetComponentInChildren<Renderer>().enabled = true;

        } else if (Mathf.Approximately(transform.rotation.eulerAngles.y, 225.0f)) {
            _walls[0].GetComponentInChildren<Renderer>().enabled = true;
            _walls[1].GetComponentInChildren<Renderer>().enabled = true;
            _walls[2].GetComponentInChildren<Renderer>().enabled = false;
            _walls[3].GetComponentInChildren<Renderer>().enabled = false;

        } else if (Mathf.Approximately(transform.rotation.eulerAngles.y, 135.0f)) {
            _walls[0].GetComponentInChildren<Renderer>().enabled = false;
            _walls[1].GetComponentInChildren<Renderer>().enabled = true;
            _walls[2].GetComponentInChildren<Renderer>().enabled = true;
            _walls[3].GetComponentInChildren<Renderer>().enabled = false;
        }
    }

    private void SnapRotation() {
        transform.DORotate(SnappedVector(), 0.5f)
            .SetEase(Ease.OutBounce)
            .OnComplete(() => {
                _isBusy = false;
            });
    }

    private Vector3 SnappedVector() {
        var endValue = 0.0f;
        var currentY = Mathf.Ceil(transform.rotation.eulerAngles.y);

        endValue = currentY switch {
            >= 0 and <= 90 => 45.0f,
            >= 91 and <= 180 => 135.0f,
            >= 181 and <= 270 => 225.0f,
            _ => 315.0f
        };

        return new Vector3(_xRotation, endValue, 0.0f);
    }
}
