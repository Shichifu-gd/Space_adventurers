using System.Collections;
using UnityEngine;

public class EnemyMovements : MonoBehaviour
{
    [SerializeField]
    private DirectionMovement directionMovement = new DirectionMovement();

    [SerializeField] private float Tilt = .5f;
    [SerializeField] private float Dodge = .2f;
    [SerializeField]
    private float Smoothing = .1f;
    private float RotationOnZ;
    private float TargetManeuver;

    [SerializeField] private Vector2 StartWait = new Vector2();
    [SerializeField] private Vector2 ManeuverTime = new Vector2();
    [SerializeField] private Vector2 ManeuverWait = new Vector2();

    private Boundary boundary = new Boundary();
    private BoundarySettings boundarySettings = new BoundarySettings();

    private new Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        boundarySettings.SetBoundary(boundary, Resolution.R_16_9);
        SetRotationOnZ();
        StartCoroutine(Evade());
    }

    private void SetRotationOnZ()
    {
        if (directionMovement == DirectionMovement.North) RotationOnZ = 0f;
        if (directionMovement == DirectionMovement.South) RotationOnZ = 180.0f;
    }

    private IEnumerator Evade()
    {
        yield return new WaitForSeconds(Random.Range(StartWait.x, StartWait.y));
        while (true)
        {
            TargetManeuver = Random.Range(1, Dodge) * -Mathf.Sign(transform.position.x);
            yield return new WaitForSeconds(Random.Range(ManeuverTime.x, ManeuverTime.y));
            TargetManeuver = 0;
            yield return new WaitForSeconds(Random.Range(ManeuverWait.x, ManeuverWait.y));
        }
    }

    private void FixedUpdate()
    {
        Move();
        Tilts();
    }

    private void Move()
    {
        float newManeuver = Mathf.MoveTowards(rigidbody.velocity.x, TargetManeuver, Time.deltaTime * Smoothing);
        rigidbody.velocity = new Vector3(newManeuver, 0.0f, -15);
        rigidbody.position = new Vector3
        (
            Mathf.Clamp(rigidbody.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rigidbody.position.z, boundary.zMin - 100, boundary.zMax + 100)
        );
    }

    private void Tilts()
    {
        rigidbody.rotation = Quaternion.Euler(0.0f, RotationOnZ, rigidbody.velocity.x * -Tilt);
    }
}