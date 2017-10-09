using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingShooter : MonoBehaviour {
    public GameObject Joueur;
    public GameObject grappling;
    private bool isShooting = false;
    private bool hasShoot = false;
    private Vector3 direction = new Vector3(1.0f, 2.0f, 1.0f);
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            shoot();
        else
            isShooting = false;
        if (hasShoot)
            grappling.transform.Translate(direction.x * 10.0f, direction.y, direction.z);
    }

    private void shoot()
    {
        isShooting = true;
        hasShoot = true;

        grappling.transform.position = transform.position;
        Instantiate(grappling);
        Debug.Log("Je viens de tirer");

    }
}
