using UnityEngine;

public class Track : MonoBehaviour
{
    private float Speed;

    private bool CanMove;

    private Vector3 Direction = new Vector3();

    public Transform Point;

    public void TrackSettings(float speed, Vector3 direction, bool canMove = true)
    {
        Speed = speed;
        Direction = direction;
        CanMove = canMove;
    }

    private void FixedUpdate()
    {
        if (CanMove) Move();
    }

    private void Move()
    {
        transform.Translate(Direction.normalized * Speed);
    }

    public Vector3 GetPoint()
    {
        return Point.position;
    }

    public void NewVector(Vector3 vector3)
    {
        transform.position = vector3;
    }
}