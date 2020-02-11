using UnityEngine;

public class DestructionZone : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        meshRenderer.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        var gameObj = other.GetComponentInParent<IPool>();
        if (gameObj != null) gameObj.ObjectShutdown();
        else Destroy(other.gameObject);
    }
}