using UnityEngine;
using System.Collections;

public class Skill_StunRaySphere : MonoBehaviour
{

    void OnTriggerEnter(Collider hit)
    {
        if (hit.transform.name == "Robot")
            hit.transform.parent.GetComponent<ThievingRobot>().StunRobot();
    }
}
