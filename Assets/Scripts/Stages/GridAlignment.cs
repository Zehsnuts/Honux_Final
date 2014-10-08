using UnityEngine;
using System.Collections;

public class GridAlignment : MonoBehaviour {

    void Awake()
    {
        Ray myRay = new Ray(transform.position, new Vector3(transform.position.x, transform.localPosition.y + 1, transform.position.z));
        RaycastHit hitInfo;

<<<<<<< HEAD
        if (Physics.SphereCast(transform.position, 2F, transform.position, out hitInfo, Mathf.Infinity))
            if (hitInfo.transform.GetComponent<CrystalsUnit>())
=======
        if (Physics.SphereCast(transform.position, 2, transform.position, out hitInfo, Mathf.Infinity))
            if (hitInfo.transform.GetComponent<CrystalsUnit>() || hitInfo.transform.GetComponent<CrystalsUnit_Bridge>())
>>>>>>> new_branch
            {
                hitInfo.transform.position = transform.position;
                Debug.Log(hitInfo.transform.name);
            }

        Destroy(gameObject);
    }
}
