using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideScript : MonoBehaviour
{
    public List<GameObject> activeSideShowerPlanes;

    void Start()
    {
        activeSideShowerPlanes = new List<GameObject>();
    }

    public void deactivateAllActivePlanes(){
        foreach (GameObject plane in activeSideShowerPlanes){
            plane.SetActive(false);
        }
    }
}
