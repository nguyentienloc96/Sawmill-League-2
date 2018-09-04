using UnityEngine;

public class FellingGame : MonoBehaviour
{

    public Animator anim;
    public Animator trunk;
    public Transform maskFelling;

    public void FellingTree()
    {
        anim.SetBool("isFelling", true);
    }

    public void StopFelling()
    {
        anim.SetBool("isFelling", false);
        maskFelling.localScale += new Vector3(0.5f, 0f, 0f);
        if(maskFelling.localScale.x >= 2.5f)
        {
            trunk.SetBool("isFall", true);
            Invoke("ResetTree", 0.5f);
        }
    }

    public void ResetTree()
    {
        GameManager.Instance.lsLocation[GameManager.Instance.IDLocation].FellingComplete();
        if(GameManager.Instance.lsLocation[GameManager.Instance.IDLocation]._Forest._Tree > 0)
        {
            maskFelling.localScale = new Vector3(0f, 2f, 1f);
            trunk.SetBool("isFall", false);
        }
    }
}
