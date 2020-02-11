using UnityEngine;

public class RandomRotator : MonoBehaviour
{
    [SerializeField]
    private float Tumble = .1f;

    private void Start()
    {
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * Tumble;
    }
}