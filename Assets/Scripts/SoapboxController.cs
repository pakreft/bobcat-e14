using UnityEngine;

public class SoapboxController : MonoBehaviour
{
    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteeringAngle;

    [SerializeField] private WheelCollider frontLeftCollider;
    [SerializeField] private WheelCollider frontRightCollider;
    [SerializeField] private WheelCollider rearLeftCollider;
    [SerializeField] private WheelCollider rearRightCollider;
    
    [SerializeField] private Transform frontLeftWheel;
    [SerializeField] private Transform frontRightWheel;
    [SerializeField] private Transform rearLeftWheel;
    [SerializeField] private Transform rearRightWheel;
    
    private float _horizontalInput;
    private float _verticalInput;
    private bool _isBreaking;
    
    private void FixedUpdate()
    {
        GetInput();
        ApplyForces();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        _isBreaking = Input.GetKey(KeyCode.Space);
    }
    
    private void ApplyForces()
    {
        // Just front drive
        frontLeftCollider.motorTorque = _verticalInput * motorForce;
        frontRightCollider.motorTorque = _verticalInput * motorForce;

        // Braking
        float currentBrakeForce = _isBreaking ? breakForce : 0.0f;
        frontLeftCollider.brakeTorque = currentBrakeForce;
        frontRightCollider.brakeTorque = currentBrakeForce;
        rearLeftCollider.brakeTorque = currentBrakeForce;
        rearRightCollider.brakeTorque = currentBrakeForce;
    }

    private void HandleSteering()
    {
        frontLeftCollider.steerAngle = _horizontalInput * maxSteeringAngle;
        frontRightCollider.steerAngle = _horizontalInput * maxSteeringAngle;
    }

    private void UpdateWheels()
    {
        UpdateWheel(frontLeftCollider, frontLeftWheel);
        UpdateWheel(frontRightCollider, frontRightWheel);
        UpdateWheel(rearLeftCollider, rearLeftWheel);
        UpdateWheel(rearRightCollider, rearRightWheel);
    }

    private void UpdateWheel(WheelCollider collider, Transform wheel)
    {
        Vector3 pos;
        Quaternion rot;
        collider.GetWorldPose(out pos, out rot);
        wheel.position = pos;
        wheel.rotation = rot;
    }
}
