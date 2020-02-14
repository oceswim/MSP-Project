using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpAndDown : MonoBehaviour
{
    private bool again;
    private void OnEnable()
    {
        again = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (again)
        {
            again = false;
            StartCoroutine(MoveUpDown());
        }
    }
    private IEnumerator MoveUpDown()
    {
        transform.Translate(0, 0, .05f);
        yield return new WaitForSeconds(.25f);
        transform.Translate(0, 0, -.05f);
        yield return new WaitForSeconds(.25f);
        again = true;
    }
}

