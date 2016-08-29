using SharpNeat.Phenomes;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FourLeggedWalkerController : UnitController
{
    public float SensorRange = 10f;
    public float Damper = 100f;
    public float HeightDamper = 1f;
    public float DistanceDamper = 1f;

    private Rigidbody _cachedRigidBody;
    private float _maximumHeight = float.MinValue;
    private float _maximumDistance = float.MinValue;

    void Start()
    {
        _cachedRigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!IsRunning)
        {
            return;
        }

        if (transform.position.y > _maximumHeight)
        {
            _maximumHeight = transform.position.y;
        }

        if (transform.position.x > _maximumDistance)
        {
            _maximumDistance = transform.position.x;
        }

        float rightSensor = 0;
        float leftSensor = 0;

        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.forward*1.1f,
            transform.TransformDirection(new Vector3(0, 0, 1).normalized), out hit, SensorRange))
        {
            rightSensor = 1 - hit.distance/SensorRange;
        }

        if (Physics.Raycast(transform.position + transform.forward*1.1f,
            transform.TransformDirection(new Vector3(0, 0, -1).normalized), out hit, SensorRange))
        {
            leftSensor = 1 - hit.distance/SensorRange;
        }

        ISignalArray inputArray = Box.InputSignalArray;
        inputArray[0] = rightSensor;
        inputArray[1] = leftSensor;

        Box.Activate();

        ISignalArray outputArray = Box.OutputSignalArray;
        Vector3 leftDirection = new Vector3((float) outputArray[0], (float) outputArray[1], (float) outputArray[2])*
                                (1/Damper);
        Vector3 rightDirection = new Vector3((float) outputArray[3], (float) outputArray[4], (float) outputArray[5])*
                                 (1/Damper);

        _cachedRigidBody.AddForceAtPosition(leftDirection, new Vector3(0f, 1f, .5f));
        _cachedRigidBody.AddForceAtPosition(rightDirection, new Vector3(1f, 1f, .5f));
    }

    public override float GetFitness()
    {
        return _maximumHeight * (1 / HeightDamper) + _maximumDistance * (1 / DistanceDamper);
    }
}