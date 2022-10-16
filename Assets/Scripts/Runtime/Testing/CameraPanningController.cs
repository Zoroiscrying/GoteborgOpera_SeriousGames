using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanningController : MonoBehaviour
{
    [SerializeField] private bool inputReversed;
    [SerializeField] private float sensitivity = 1.0f;

    private bool _releasedButton = false;
    private Vector2 _deltaPosition;
    private Vector2 _currentMousePosition;
    private Vector2 _lastFrameMousePosition;

    // Update is called once per frame
    void Update()
    {
        // if right mouse or middle mouse
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            _currentMousePosition = Input.mousePosition;
            
            // this is the first time the mouse button get down, reset cur and last position
            if (_releasedButton)
            {
                _lastFrameMousePosition = _currentMousePosition;
            }
            _releasedButton = false;

            _deltaPosition = (_currentMousePosition -_lastFrameMousePosition) * sensitivity;
            if (inputReversed)
            {
                _deltaPosition *= -1;
            }
            
            // move camera
            this.transform.position += new Vector3(_deltaPosition.x, _deltaPosition.y, 0.0f);
            
            // record last frame position
            _lastFrameMousePosition = _currentMousePosition;
        }
        else
        {
            // the mouse just released, disable movement now.
            if (!_releasedButton)
            {
                _deltaPosition = Vector2.zero;
            }
            _releasedButton = true;
        }
    }
}
