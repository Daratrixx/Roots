using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpZone : MonoBehaviour {

    public Color color;
    public Transform warpTarget;
    public bool resetSpeed;
    
	void Start () {
        GetComponent<MeshRenderer>().material.color = color;
	}

    private void OnTriggerEnter(Collider collider) {
        Debug.Log("Warp zone triggered ! " + color.ToString());
        GameObject o = collider.gameObject;
        Player p = o.GetComponent<Player>();
        if (p != null) {
            if(warpTarget != null)
                o.transform.position = warpTarget.position;
            if(resetSpeed) {
                o.transform.rotation = Quaternion.identity;
                o.GetComponent<Rigidbody>().ResetInertiaTensor();
            }
        }
    }
}
