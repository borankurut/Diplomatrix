using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideShowerScript : MonoBehaviour
{
    [SerializeField]
    GameObject plane;
    // Start is called before the first frame update

    private SideScript sideScript;
    void Start()
    {
        sideScript = GetComponentInParent<SideScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver()
    {
        sideScript.activeSideShowerPlanes.Add(plane);
        plane.SetActive(true);
    }

    void OnMouseExit()
    {
        sideScript.activeSideShowerPlanes.Remove(plane);
        plane.SetActive(false);
    }
}
