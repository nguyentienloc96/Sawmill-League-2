using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMouse : MonoBehaviour {

    public int indexType;

    public void Update()
    {
        if (UIManager.Instance.scene == TypeScene.MINIGAME)
        {
            if (indexType == GameManager.Instance.lsLocation[GameManager.Instance.IDLocation].indexType)
            {
                Vector3 posMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                posMouse.z = 0;
                transform.position = posMouse;
            }
        }
    }

}
