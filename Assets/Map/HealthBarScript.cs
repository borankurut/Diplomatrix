using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Image = UnityEngine.UI.Image;


public class HealthBarScript : MonoBehaviour
{
    [SerializeField]
    private ArmyMember armyMember;

    [SerializeField]

    private Image image;

    private int initialHealth;

    private int currentHealth;

    private float slideAmount = 1;

    void Start(){
        initialHealth = armyMember.Health;
        ArmyScript myArmy = GetComponentInParent<ArmyScript>();
        if(myArmy == null){
            image.color = UnityEngine.Color.black;
        }

        else if(myArmy.GetArmyType() == ArmyScript.ArmyType.playerArmy){
            image.color = UnityEngine.Color.green;
        }

        else if(myArmy.GetArmyType() == ArmyScript.ArmyType.NPCArmy){
            image.color = UnityEngine.Color.red;
        }

    }

    void Update()
    {
        Vector3 lookDirection = Camera.main.transform.position - transform.position;
        lookDirection.y = 0;

        if (lookDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = rotation;
        }

        currentHealth = armyMember.Health;

        slideAmount = (float) currentHealth / initialHealth;

        image.fillAmount = slideAmount;
    }
}
