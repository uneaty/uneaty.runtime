using UnityEngine;

public class FourLeggedWalker : SensorBasedAI
{
    public Vector3 Starting;

    public void Start()
    {
        Starting = transform.position;
    }

    public override float GetFitness()
    {
        return !float.IsNaN(Starting.z)
            ? (Mathf.Abs(transform.position.x - Starting.x) + Mathf.Abs(transform.position.z - Starting.z))
            : 0;
    }
}