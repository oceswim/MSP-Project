using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    private bool button1, button2, button3,stop;
    public GameObject nextButton;
    public AudioSource good;
    private void Start()
    {
        button1 = button2 = button3 =stop= false;
    }
    private void Update()
    {
        if(button1 && button2 && button3 && !stop)
        {
            stop = true;
            nextButton.SetActive(true);
        }
    }
    // Start is called before the first frame update
    public void Button1()
    {
        if(!button1)
        {
            good.Play();
            button1 = true;
        }
    }
    public void Button2()
    {
        if(!button2)
        {
            good.Play();
            button2 = true;
        }
    }
    public void Button3()
    {
        if(!button3)
        {
            good.Play();
            button3 = true;
        }
    }
}
