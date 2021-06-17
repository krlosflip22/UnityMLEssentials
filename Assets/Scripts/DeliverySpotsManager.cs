using UnityEngine;

public class DeliverySpotsManager : MonoBehaviour
{
    [SerializeField] private float goalReward = 0.1f;

    public DeliveryPoint[] Milestones;

    public DeliveryPoint Goal;

    public DeliveryCarAgent agent;

    int currentPoint = 0;

    void Awake()
    {
        for(int i = 0; i < Milestones.Length; i++)
        {
            Milestones[i].Initialize(this, i);
        }

        Goal.Initialize(this, Milestones.Length);
    }

    public void Setup()
    {
        currentPoint = 0;
        foreach(DeliveryPoint dp in Milestones)
        {
            dp.gameObject.SetActive(true);
        }
        Goal.gameObject.SetActive(false);
    }

    public void SetReward(int _id, bool isLast)
    {
        if(currentPoint == _id)
        {
            agent.GivePoints(goalReward, isLast);
            if(isLast) return;
            if(currentPoint == Milestones.Length - 1) Goal.gameObject.SetActive(true);

            currentPoint++;
        }
        else
        {
            agent.TakeAwayPoints();
        }
    }



}