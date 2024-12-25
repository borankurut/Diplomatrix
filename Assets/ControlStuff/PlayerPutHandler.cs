using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPutHandler : MonoBehaviour
{
    [SerializeField]
    private Terrain terrain;

    [SerializeField]
    private FreeCameraController cameraController;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !Input.GetMouseButton(1))
        {
            LogClickLocation();
        }
    }

    void LogClickLocation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == terrain.gameObject)
            {
                Vector3 clickPosition = hit.point;
                Debug.Log("Clicked terrain position: " + clickPosition);
            }
        }
    }
}
