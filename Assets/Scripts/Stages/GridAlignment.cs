using UnityEngine;
using System.Collections;

public class GridAlignment : MonoBehaviour {

    void Awake()
    {    
        RaycastHit hitInfo;

        if (Physics.SphereCast(transform.position, 3, transform.position, out hitInfo, Mathf.Infinity))
            if (hitInfo.transform.GetComponent<CrystalsUnit>() || hitInfo.transform.GetComponent<CrystalsUnit_Bridge>())
            {
                var dist = Vector3.Distance(hitInfo.transform.position, transform.position);

                if(dist < 4)
                    hitInfo.transform.position = this.transform.position;

                Destroy(gameObject);

            }

    }
}
