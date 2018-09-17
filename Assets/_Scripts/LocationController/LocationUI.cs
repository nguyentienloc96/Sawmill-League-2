using UnityEngine;
using UnityEngine.UI;

public class LocationUI : MonoBehaviour {

    public int id;
    public string nameLocationUI;

    public void OnclickLocation()
    {
        UIManager.Instance.scene = TypeScene.LOCATION;
        AudioManager.Instance.Play("Click");
        GameManager.Instance.IDLocation = id;
        GameManager.Instance.LoadLocation();
        UIManager.Instance.worldManager.transform.SetAsFirstSibling();
        if (!UIManager.Instance.locationManager.gameObject.activeInHierarchy)
            UIManager.Instance.locationManager.SetActive(true);
    }
}
