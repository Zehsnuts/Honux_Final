using UnityEngine;
using System.Collections;

public class GridAlignment : MonoBehaviour {

    void Awake()
    {
        Ray myRay = new Ray(transform.position, new Vector3(transform.position.x, transform.localPosition.y + 1, transform.position.z));
        RaycastHit hitInfo;

        if (Physics.SphereCast(transform.position, 2, transform.position, out hitInfo, Mathf.Infinity))
            if (hitInfo.transform.GetComponent<CrystalsUnit>() || hitInfo.transform.GetComponent<CrystalsUnit_Bridge>())
            {
                var dist = Vector3.Distance(hitInfo.transform.position, transform.position);

                if(dist < 4)
                    hitInfo.transform.position = this.transform.position;
            }

        Destroy(gameObject);
    }
}
