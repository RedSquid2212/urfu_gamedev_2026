using UnityEngine;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour
{
    private Rigidbody rb;
    private DiceValues diceValues;
    private Vector3 startPosition;

    public bool IsRolling { get; private set; }
    public int LastValue { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        diceValues = GetComponent<DiceValues>();
        startPosition = transform.position;
    }
    
    private void FixedUpdate()
    {
        if (!IsRolling) return;
        if (!rb.IsSleeping()) return;

        IsRolling = false;
        LastValue = diceValues.GetTopValue();
    }

    public void Throw(Vector3 force, Vector3 torque)
    {
        IsRolling = true;
        LastValue = 0;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.AddForce(force, ForceMode.Impulse);
        rb.AddTorque(torque, ForceMode.Impulse);
    }

    public void ResetToStart()
    {
        IsRolling = false;
        LastValue = 0;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.SetPositionAndRotation(startPosition, Random.rotation);
    }
}
