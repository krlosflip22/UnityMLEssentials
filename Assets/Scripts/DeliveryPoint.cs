using UnityEngine;

public class DeliveryPoint : MonoBehaviour
{
    DeliverySpotsManager manager;

    [SerializeField] bool isLast;

    [SerializeField] int id;

    public void Initialize(DeliverySpotsManager _manager, int _id)
    {
        manager = _manager;
        id = _id;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag.ToLower() == "player")
        {
            manager.SetReward(id, isLast);
            gameObject.SetActive(false);
        }
    }

}