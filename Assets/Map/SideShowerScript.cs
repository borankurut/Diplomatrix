using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideShowerScript : MonoBehaviour
{
    [SerializeField]
    GameObject plane;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver()
    {
        plane.SetActive(true);
    }

    void OnMouseExit()
    {
        plane.SetActive(false);
    }
}
