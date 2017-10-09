using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOn : MonoBehaviour {

    public Transform target;

    public float distance; //the distance between the camera and the player
    public float startToFade = 1.5f; //when the player start to fade
    public float maxFade = 0.2f; //when you want an alpha equals to 0

    private Color currentColor;
    private Color baseColor;
    private bool isFading = false;
    private SkinnedMeshRenderer meshRenderer;


    private void Start() {
        meshRenderer = target.GetComponentInChildren<SkinnedMeshRenderer>();
        baseColor = meshRenderer.material.color;
        currentColor = baseColor;
    }

    private float alphaBlendFactor = 10;

    void LateUpdate() {
        //joueur.opacity(joueur.transform.position - camera.transform.position);
        distance = Vector3.Distance(target.position, transform.position);
        if (isFading) {
            float fadeOffset = startToFade * maxFade;
            if (distance <= startToFade) {
                float fadeProgression = (distance - fadeOffset) / (startToFade - fadeOffset);
                if (fadeProgression < 0) {
                    meshRenderer.enabled = false;
                } else {
                    if (!meshRenderer.enabled)
                        meshRenderer.enabled = true;
                    currentColor = Color.Lerp(currentColor, new Color(baseColor.r, baseColor.g, baseColor.b, fadeProgression), Time.deltaTime * alphaBlendFactor);
                }
            } else {
                isFading = false;
            }
        } else {
            if (distance <= startToFade) {
                isFading = true;
            } else if (currentColor.a < 1) {
                if (1 - currentColor.a < 0.001f)
                    currentColor = baseColor;
                else
                    currentColor = Color.Lerp(currentColor, baseColor, Time.deltaTime * alphaBlendFactor * 3);
            }
        }
        meshRenderer.material.color = currentColor;
    }


    private void OnGUI() {
        GUI.Label(new Rect(0, 0, 200, 20), "distance : " + distance);
        if (!meshRenderer.enabled) {
            GUI.Label(new Rect(0, 20, 200, 20), "full transparent");
        } else {
            if (currentColor.a < 1)
                GUI.Label(new Rect(0, 20, 200, 20), "current alpha : " + currentColor.a);
            else
                GUI.Label(new Rect(0, 20, 200, 20), "full opaque");
        }
    }

}
