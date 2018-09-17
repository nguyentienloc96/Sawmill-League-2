using UnityEngine;

public class Felling : MonoBehaviour
{

    public Animator anim;
    public Animator animTrunk;
    public GameObject objTrunk;
    public Transform maskFelling;
    public GameObject notification;

    private bool isWaiting;

    public void OnEnable()
    {
        int ID = GameManager.Instance.IDLocation;
        if (GameManager.Instance.lsLocation[ID].forest.tree > 0)
        {
            objTrunk.SetActive(true);
            notification.SetActive(false);
        }
        else
        {
            objTrunk.SetActive(false);
            notification.SetActive(true);
        }
    }

    public void FellingTree()
    {
        int ID = GameManager.Instance.IDLocation;
        if (GameManager.Instance.lsLocation[ID].forest.tree > 0)
        {
            AudioManager.Instance.Play("Felling");
            anim.SetBool("isFelling", true);
        }
    }

    public void StopFelling()
    {
        if (!isWaiting)
        {
            int ID = GameManager.Instance.IDLocation;
            if (GameManager.Instance.lsLocation[ID].forest.tree > 0)
            {
                anim.SetBool("isFelling", false);
                maskFelling.localScale += new Vector3(0.5f, 0f, 0f);
                if (maskFelling.localScale.x >= 2.5f)
                {
                    animTrunk.SetBool("isFall", true);
                    isWaiting = true;
                    Invoke("ResetTree", 0.5f);
                }
            }
        }
    }

    public void ResetTree()
    {
        GameManager.Instance.lsLocation[GameManager.Instance.IDLocation].FellingComplete();
        if(GameManager.Instance.lsLocation[GameManager.Instance.IDLocation].forest.tree > 0)
        {
            isWaiting = false;
            maskFelling.localScale = new Vector3(0f, 2f, 1f);
            animTrunk.SetBool("isFall", false);
        }
        else
        {
            anim.SetBool("isFelling", false);
            objTrunk.SetActive(false);
            maskFelling.localScale = new Vector3(0f, 2f, 1f);
            animTrunk.SetBool("isFall", false);
            notification.SetActive(true);
        }
    }
}
