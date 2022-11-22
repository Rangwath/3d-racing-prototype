using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    // Configuration
    [SerializeField] float maxSpeed = 30f;
    [SerializeField] float accelerationFactor = 8f;
    [SerializeField] float brakingFactor = 4f;
    [SerializeField] float turnFactor = 180f;

    // Game Objects
    [SerializeField] Rigidbody myRigidBody;

    // Local variables
    float accelerationInput = 0f;
    float turnInput = 0f;

    // Constants
    const float FactorMultiplier = 1000f;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() 
    {
        // Moving the car object the same as the sphere's RigidBody
        transform.position = myRigidBody.position;

        ApplyAccelerationForce();
        ApplyTurning();
    }

    void ApplyAccelerationForce()
    {
        myRigidBody.AddForce(transform.forward * accelerationInput);

        if (myRigidBody.velocity.magnitude > maxSpeed)
        {
            myRigidBody.velocity = myRigidBody.velocity.normalized * maxSpeed;
        }

        Debug.Log(myRigidBody.velocity.magnitude);
    }
    
    void ApplyTurning()
    {
        Vector3 rotationVector = new Vector3(0f, turnInput * turnFactor * Time.deltaTime * Mathf.Sign(accelerationInput) * (myRigidBody.velocity.magnitude / maxSpeed), 0f);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rotationVector);
    }

    void OnMove(InputValue value)
    {
        // Get the movement input and save it to local variables
        Vector2 moveInput = value.Get<Vector2>();
        if (moveInput.y > 0)
        {
            accelerationInput = moveInput.y * accelerationFactor * FactorMultiplier;
        }
        else if (moveInput.y < 0)
        {
            accelerationInput = moveInput.y * brakingFactor * FactorMultiplier;
        }
        else
        {
            accelerationInput = 0;
        }

        turnInput = moveInput.x;
    }
}
