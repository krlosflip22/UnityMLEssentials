using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using UnityEngine;
using static CarController;

public class DeliveryCarAgent : BaseAgent
{
    private Vector3 originalPosition;

    private BehaviorParameters behaviorParameters;

    private RacingCarController deliveryCarController;

    private Rigidbody carControllerRigidBody;

    private DeliverySpotsManager deliverySpots;

    public override void Initialize()
    {
        originalPosition = transform.localPosition;
        behaviorParameters = GetComponent<BehaviorParameters>();
        carControllerRigidBody = GetComponent<Rigidbody>();
        deliveryCarController = GetComponent<RacingCarController>();
        deliverySpots = transform.parent.GetComponentInChildren<DeliverySpotsManager>();

        ResetParkingLotArea();
    }

    public override void OnEpisodeBegin()
    {
        ResetParkingLotArea();
    }

    private void ResetParkingLotArea()
    {
        // important to set car to automonous during default behavior
        deliveryCarController.IsAutonomous = behaviorParameters.BehaviorType == BehaviorType.Default;
        transform.localPosition = originalPosition;
        transform.localRotation = Quaternion.identity;
        carControllerRigidBody.velocity = Vector3.zero;
        carControllerRigidBody.angularVelocity = Vector3.zero;

        // reset which cars show or not show
        deliverySpots.Setup();
    }

    void Update()
    {
        if(transform.localPosition.y <= 0)
        {
            TakeAwayPoints();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.rotation);

        sensor.AddObservation(deliverySpots.Goal.transform.position);
        sensor.AddObservation(deliverySpots.Goal.transform.rotation);

        foreach(DeliveryPoint mlstn in deliverySpots.Milestones)
        {
            sensor.AddObservation(mlstn.transform.position);
            sensor.AddObservation(mlstn.transform.rotation);
        }

        sensor.AddObservation(carControllerRigidBody.velocity);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        deliveryCarController.VerticalInput = vectorAction[0] ;
        Debug.Log($"Vertical {vectorAction[0]}");
        deliveryCarController.HorizontalInput = vectorAction[1] ;
        Debug.Log($"Horizontal {vectorAction[1]}");
        deliveryCarController.IsBreaking = Mathf.FloorToInt(vectorAction[0]) == 1 ? true : false;

        AddReward(-1f / MaxStep);
    }

    public void GivePoints(float amount = 1.0f, bool isFinal = false)
    {
        AddReward(amount);

        if(isFinal)
        {
            StartCoroutine(SwapGroundMaterial(successMaterial, 0.5f));

            EndEpisode();
        }
    }

    public void TakeAwayPoints()
    {
        StartCoroutine(SwapGroundMaterial(failureMaterial, 0.5f));

        AddReward(-0.01f);

        EndEpisode();
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis ("Horizontal");
        actionsOut[1] = Input.GetAxis ("Vertical");
        actionsOut[2] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }
}
