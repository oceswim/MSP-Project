using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject LEAPobject;
    public GameObject theCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TeleportClass()
    {
        Vector3 classPosition = new Vector3(-2.5f, 5.8f, -8.6f);
        float xRot =0;
        float yRot = 90f;
        float zRot = 0;
        StartCoroutine(TeleportProcess(classPosition,xRot,yRot,zRot));
    }
    public void TeleportPractice()
    {
        Vector3 deskPosition = new Vector3(1.7f, 6.2f, -10.7f);
        float xRot = 20.2f;
        float yRot = 182.4f;
        float zRot = .9f;
        StartCoroutine(TeleportProcess(deskPosition, xRot, yRot, zRot));
    }
    private IEnumerator TeleportProcess(Vector3 newPosition, float rotX, float rotY,float rotZ)
    {
        yield return new WaitForSeconds(.5f);
        LEAPobject.transform.position = newPosition;
        LEAPobject.transform.rotation = Quaternion.identity;
        LEAPobject.transform.Rotate(rotX,rotY ,rotZ);
        yield return new WaitForSeconds(.5f);
       
    }
}
