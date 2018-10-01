using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TapDown : MonoBehaviour {

    public UnityEvent onMouseDown;

    public void OnMouseDown()
    {
        onMouseDown.Invoke();
    }

}
