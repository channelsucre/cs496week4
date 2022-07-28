using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lid : MonoBehaviour
{
    bool closed;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().color = new Color(1f, 0.1f, 0.1f, 0f);
        closed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Blink()
    {
        GetComponent<Image>().color = new Color(1f, 0.1f, 0.1f, closed ? 0f : 0.5f);
        closed = !closed;
        Debug.LogFormat("Blink() in Lid.cs - closed {0}", closed);
    }
}
