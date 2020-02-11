using UnityEngine;

public class UIActive : MonoBehaviour
{
    public void OnUI()
    {
        gameObject.SetActive(true);
    }

    public void OffUI()
    {
        gameObject.SetActive(false);
    }
}