using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOn : MonoBehaviour {

    public GameObject joueur;
    public GameObject mesh; //the mesh of the player
    public GameObject joueurCamera;

    public float distance; //the distance between the camera and the player
    public float startToFade = 1.5f; //when the player start to fade
    public float maxFade = 0.2f; //when you want an alpha equals to 0

    private Color color = Color.white;
    private bool isFading = false;

    void LateUpdate(){
        //joueur.opacity(joueur.transform.position - camera.transform.position);
        distance = Vector3.Distance(joueur.transform.position, joueurCamera.transform.position);
        if (isFading){
            if (distance <= startToFade){
                float fadeProgression = (distance - (maxFade/ startToFade)) / startToFade;

                if (fadeProgression < maxFade)
                    color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                else
                    color = new Color(1.0f, 1.0f, 1.0f, fadeProgression);
            }
            else{
                isFading = false;
                color = Color.white;
            }
        }
        else{
            if (distance <= startToFade){
                isFading = true;
            }
        }
        mesh.GetComponent<SkinnedMeshRenderer>().material.color = color;

        Debug.Log((distance * 1) / 3);
    }


    private void OnGUI()
    {
        GUI.Label(new Rect(0,0,200,50), "" + color.a);
    }

}
