using UnityEngine;
using DG.Tweening;

public class DebarkingGame : MonoBehaviour
{
    public bool isMaterial;
    public Transform clamp;
    public Transform tree;

    private bool isRun;
    private Vector3 posDown;

    private void OnEnable()
    {
        ResetMaterial();
    }

    public void Update()
    {
        if (isRun)
        {
            if (Input.mousePosition.y > posDown.y)
            {
                float dis = Input.mousePosition.y - posDown.y;
                clamp.position += new Vector3(0f, dis * 0.01f * Time.deltaTime, 0f);
            }
        }
    }

    public void TapDown()
    {
        posDown = Input.mousePosition;
        isRun = true;
    }

    public void TapUp()
    {
        isRun = false;
    }

    public void ResetMaterial()
    {
        clamp.localPosition = Vector3.zero;
        tree.localPosition = new Vector3(11f, 0f, 0f);
        tree.DOLocalMove(new Vector3(1f, 0f, 0f), 2f).OnComplete(() => isMaterial = true);
    }
}
