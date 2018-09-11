using UnityEngine;
using UnityEngine.UI;

public class LocationUI : MonoBehaviour {

    public int _ID;
    public string _Name;

    public void OnclickLocation()
    {
        AudioManager.Instance.Play("Click");
        GameManager.Instance.IDLocation = _ID;
        GameManager.Instance.LoadLocation();
        UIManager.Instance.worldUI.transform.SetAsFirstSibling();
        if (!UIManager.Instance.locationUI.gameObject.activeInHierarchy)
            UIManager.Instance.locationUI.SetActive(true);
    }
}
