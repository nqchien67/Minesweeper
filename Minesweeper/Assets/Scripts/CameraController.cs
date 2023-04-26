using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomSpeed = 5f;
    public float zoomMin = 1f;
    public float zoomMax = 3f;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        ZoomHandler();
    }

    private void ZoomHandler()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - scroll * zoomSpeed, zoomMin, zoomMax);
    }
}
