using UnityEngine;

public class DeliveryObstacle : MonoBehaviour
{
    DeliveryCarAgent agent;

    void Awake()
    {
        // cache agent
        agent = transform.parent.parent.GetComponentInChildren<DeliveryCarAgent>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag.ToLower() == "player")
        {
            agent.TakeAwayPoints();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag.ToLower() == "player")
        {
            agent.TakeAwayPoints();
        }
    }

}