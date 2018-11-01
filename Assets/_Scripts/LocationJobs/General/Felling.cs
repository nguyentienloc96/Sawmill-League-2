using UnityEngine;
using UnityEngine.UI;

public class Felling : MonoBehaviour
{

    public Animator anim;
    public GameObject notification;

    public GameObject tutorialHand;
    private bool isWaiting;

    public void OnEnable()
    {
        int ID = GameManager.Instance.IDLocation;
        if (GameManager.Instance.lsLocation[ID].forest.tree > 0)
        {
            tutorialHand.SetActive(true);
            notification.SetActive(false);
        }
        else
        {
            notification.SetActive(true);
        }
    }

    public void FellingTree()
    {
        int ID = GameManager.Instance.IDLocation;
        if (!isWaiting)
        {
            if (GameManager.Instance.lsLocation[ID].forest.tree > 0)
            {
                tutorialHand.SetActive(false);
                AudioManager.Instance.PlayOneShot("Felling");
                anim.SetBool("isFelling", true);
                isWaiting = true;
            }
        }
    }

    public void StopFelling()
    {
        if (isWaiting)
        {
            int ID = GameManager.Instance.IDLocation;
            GameManager.Instance.lsLocation[GameManager.Instance.IDLocation].FellingComplete();
            if (GameManager.Instance.lsLocation[ID].forest.tree <= 0)
            {
                notification.SetActive(true);
            }
            anim.SetBool("isFelling", false);
            isWaiting = false;
        }
    }

    public void Help()
    {
        tutorialHand.SetActive(true);
    }

    private void Update()
    {
        int ID = GameManager.Instance.IDLocation;
        if (GameManager.Instance.lsLocation[ID].forest.tree > 0)
        {
            notification.SetActive(false);
        }
        else
        {
            notification.SetActive(true);
        }
    }
}
