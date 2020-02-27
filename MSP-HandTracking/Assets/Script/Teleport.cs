using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teleport : MonoBehaviour
{
    public GameObject LEAPobject;
    public GameObject theCamera;
    public AudioSource welcomeTeacher,selectWordTeacher;
    public Button StartButton;
   
    public void TeleportClass(int index)
    {
        Vector3 classPosition = new Vector3(-2.5f, 5.8f, -8.6f);
        float xRot =0;
        float yRot = 90f;
        float zRot = 0;
        StartCoroutine(TeleportProcess(classPosition,xRot,yRot,zRot,index));
    }
    public void TeleportPractice()
    {
        Vector3 deskPosition = new Vector3(1.7f, 6.2f, -10.7f);
        float xRot = 20.2f;
        float yRot = 182.4f;
        float zRot = .9f;
        StartCoroutine(TeleportProcess(deskPosition, xRot, yRot, zRot,2));
    }
    public void TeleportTuto()
    {
        
        Vector3 deskPosition = new Vector3(-3, 6.2f, -25.987f);
        StartCoroutine(TeleportProcess(deskPosition, 0, 0, 0, 3));
    }
    private IEnumerator TeleportProcess(Vector3 newPosition, float rotX, float rotY, float rotZ, int index)
    {
        yield return new WaitForSeconds(.5f);
        LEAPobject.transform.position = newPosition;
        LEAPobject.transform.rotation = Quaternion.identity;
        LEAPobject.transform.Rotate(rotX, rotY, rotZ);
        yield return new WaitForSeconds(.5f);
        switch (index)
        {
          
            case 1://from hallway to class
                welcomeTeacher.Play();
                yield return new WaitForSeconds(3f);
                StartButton.interactable = true;
                break;
            case 2://from class to practice desk
                selectWordTeacher.Play();
                break;
    
        }
   

       
    }
}
