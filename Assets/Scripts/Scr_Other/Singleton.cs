using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    var singlton = new GameObject("[Singleton] " + typeof(T));
                    _instance = singlton.AddComponent<T>();
                    DontDestroyOnLoad(singlton);
                    singlton.tag = "Singleton";
                }
            }
            return _instance;
        }
    }
}