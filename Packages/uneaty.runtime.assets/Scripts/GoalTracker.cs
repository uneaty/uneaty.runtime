using System.Collections.Generic;
using UnityEngine;

public class GoalTracker : MonoBehaviour
{
    public float TimeInGoal;
    public float TimesFound;
    public List<GoalZone> Entered = new List<GoalZone>();

    public void Update()
    {
//        TODO: Figure out how to do this more cleanly.  Memory leak potentially.
        if (Entered.Count > 0)
            Entered = new List<GoalZone>();
    }

    public void OnTriggerStay(Collider other)
    {
        GoalZone zone = other.gameObject.GetComponent<GoalZone>();
        if (zone != null && Entered.IndexOf(zone) == -1)
        {
            TimeInGoal += Time.deltaTime;
            TimesFound++;
            Entered.Add(zone);
        }
    }
}