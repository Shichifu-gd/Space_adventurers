using UnityEngine;

public class MovementObjects : MonoBehaviour
{
    [SerializeField]
    private DirectionMovement directionMovement = new DirectionMovement();

    private new Rigidbody rigidbody;

    [SerializeField]
    private float Speed = 15;

    private Vector3 Movement = new Vector3();

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        Vector3 curentVector = gameObject.transform.position;
        if (directionMovement == DirectionMovement.North) Movement = new Vector3(curentVector.x, curentVector.y, 1f);
        if (directionMovement == DirectionMovement.South) Movement = new Vector3(curentVector.x, curentVector.y, -1f);
    }

    public void SetSpeed(float speed)
    {
        Speed = speed;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {

        rigidbody.velocity = Movement * Speed;
    }
}