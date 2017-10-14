using System;
using UnityEngine;

public class TempControl : MonoBehaviour {

    public float movementSpeed;
    public float rotationSpeed;

    private Animator animator;
    private bool isWalking;
    private int currentSpeed = 0; // x10000

    private float currentSpeedPercent {
        get { return (((float)currentSpeed) / 10000)/ movementSpeed; }
    }

    // Use this for initialization
    void Start() {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update() {
        float timeFactor = Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Return)) {
            animator.SetTrigger("emote5");
        } else {
            walkCycle(timeFactor);
            applyMovement(timeFactor);
        }
    }

    void walkStart() {
        animator.SetTrigger("walkStart");
        isWalking = true;
    }

    void walkStop() {
        animator.SetTrigger("walkStop");
        isWalking = false;
    }

    void walkCycle(float deltaT) {
        float v = Input.GetAxisRaw("Vertical");
        int hInt = (int)(v * movementSpeed * deltaT * 10000);
        if(isWalking) { 
            if(hInt == 0) { // stop walking
                walkStop();
                increaseCurrentSpeed(-(int)(deltaT * movementSpeed * 10000));
            } else { // continue walking
                increaseCurrentSpeed(hInt);
            }
        } else {
            if (hInt == 0) { // continue not walking
                increaseCurrentSpeed(-(int)(deltaT * movementSpeed * 20000));
            } else { // start walking
                walkStart();
                increaseCurrentSpeed(hInt);
            }
        }
        animator.SetFloat("walkSpeed", currentSpeedPercent);
    }

    void increaseCurrentSpeed(int inputSpeed) {
        currentSpeed += inputSpeed;
        if (currentSpeedPercent < 0)
            currentSpeed = 0;
        if (currentSpeedPercent > 1)
            currentSpeed = (int)(movementSpeed * 10000);
    }

    void applyMovement(float deltaT) {
        float h = Input.GetAxisRaw("Horizontal");
        Vector3 angle = transform.localEulerAngles;
        angle.y = angle.y + h * rotationSpeed * deltaT;
        Vector3 pos = transform.localPosition;
        pos.x = pos.x + (float)Math.Sin(angle.y * (Math.PI / 180)) * currentSpeedPercent * movementSpeed * deltaT;
        pos.z = pos.z + (float)Math.Cos(angle.y * (Math.PI / 180)) * currentSpeedPercent * movementSpeed * deltaT;
        transform.localEulerAngles = angle;
        transform.localPosition = pos;
    }
}
