using UnityEngine;
using UnityEngine.UI;

public class Felling : MonoBehaviour
{

    public Animator anim;
    public Image imgTree;
    public GameObject notification;

    public GameObject tutorialHand;
    private bool isWaiting;
    private int indexFelling;

    public void OnEnable()
    {
        int ID = GameManager.Instance.IDLocation;
        if (GameManager.Instance.lsLocation[ID].forest.tree > 0)
        {
            indexFelling = 0;
            tutorialHand.SetActive(true);
            imgTree.sprite = UIManager.Instance.spTree[indexFelling];
            notification.SetActive(false);
        }
        else
        {
            indexFelling = 3;
            imgTree.sprite = UIManager.Instance.spTree[indexFelling];
            notification.SetActive(true);
        }
    }

    public void FellingTree()
    {
        int ID = GameManager.Instance.IDLocation;
        if (GameManager.Instance.lsLocation[ID].forest.tree > 0)
        {
            tutorialHand.SetActive(false);
            AudioManager.Instance.PlayOneShot("Felling");
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
                indexFelling++;
                anim.SetBool("isFelling", false);
                imgTree.sprite = UIManager.Instance.spTree[indexFelling];
                if (indexFelling == 3)
                {
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
            indexFelling = 0;
            imgTree.sprite = UIManager.Instance.spTree[indexFelling];
        }
        else
        {
            anim.SetBool("isFelling", false);
            indexFelling = 3;
            imgTree.sprite = UIManager.Instance.spTree[indexFelling];
            notification.SetActive(true);
        }
    }

    public void Help()
    {
        tutorialHand.SetActive(true);
    }
}
