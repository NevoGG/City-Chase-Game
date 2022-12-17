using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


//RigidBody2D carRigidBody;
public class carController : PoliceCar
{
    [SerializeField] private float CAR_SPEED = 0.03f;
    [SerializeField] private float CAR_ROTATION_SPEED = 0.3f;

    [SerializeField] private float SIREN_ROTATION = 5f;

    //   [SerializeField] private float SPEAKR_RADIUS = 0.1f;
    [SerializeField] private float maxSpeed = 10;
    [SerializeField] private float accelerationFactor = 1;
    
    [SerializeField] private int _carLayer;
    [SerializeField] private GameObject _car;
    [SerializeField] private GameObject _siren;
    [SerializeField] private GameObject _moneySound;

    private Rigidbody2D carRigidbody2D;
    private const int FORWARD = 1;
    private const int BACK = -1;
    private const int RIGHT = -1;
    private const int LEFT = 1;
    private const int STOP = 0;
    private Vector3 _carOriginalPosition;
    private float curVelocity;
    private float rotationAngle = 0;
    private int accelerationInput = STOP;
    private int steeringInput = STOP;


    //[SerializeField] private GameObject _speaker;
    //   private bool _isSpeakerOn;
    //  private float _speakerTimeCounter;

    void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        carRigidbody2D = GetComponent<Rigidbody2D>();
        _car.GetComponent<SpriteRenderer> ().sortingOrder = _carLayer;  
        _siren.GetComponent<SpriteRenderer> ().sortingOrder = _carLayer + 1;
    }

    // Update is called once per frame
    void Update()
    {
        _siren.transform.eulerAngles += Vector3.forward * (SIREN_ROTATION * Time.deltaTime);
        ApplySteering();
        ApplyEngineForce();
        //if (input.getkeydown(keycode.space))
        //{
        //    if (!_isspeakeron)
        //    {
        //        _flower.transform.localposition = _floweroriginalposition;
        //        _cannondirection = _tanktop.transform.up;
        //        _flower.transform.setparent(null);
        //        // start the fire
        //        _flower.setactive(true);

        //        _isinfire = true;

        //        _firetimecounter = fire_time;
        //    }
    }

        void ApplySteering()
        {
            if (Input.GetKey(KeyCode.RightArrow)) steeringInput = RIGHT;
            else if (Input.GetKey(KeyCode.LeftArrow)) steeringInput = LEFT;
            else steeringInput = STOP;
            //Limit the cars ability to turn when moving slowly
            float minSpeedBeforeAllowTurningFactor = (carRigidbody2D.velocity.magnitude / 2);
            minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);

            //Update the rotation angle based on input
            if (steeringInput == RIGHT) rotationAngle -= CAR_ROTATION_SPEED * minSpeedBeforeAllowTurningFactor;
            else if (steeringInput == LEFT) rotationAngle += CAR_ROTATION_SPEED * minSpeedBeforeAllowTurningFactor;

            //Apply steering by rotating the car object
            carRigidbody2D.MoveRotation(rotationAngle);
        }

        void ApplyEngineForce()
        {
            if (Input.GetKey(KeyCode.UpArrow)) accelerationInput = FORWARD;
            else if (Input.GetKey(KeyCode.DownArrow)) accelerationInput = BACK;
            else accelerationInput = STOP;

            //Apply drag if there is no accelerationInput so the car stops when the player lets go of the accelerator
            if (accelerationInput == STOP)
                carRigidbody2D.drag = Mathf.Lerp(carRigidbody2D.drag, 3.0f, Time.fixedDeltaTime * 3);
            else carRigidbody2D.drag = 0;

            //Caculate how much "forward" we are going in terms of the direction of our velocity
            curVelocity = Vector2.Dot(transform.up, carRigidbody2D.velocity);

            //Limit so we cannot go faster than the max speed in the "forward" direction
            if (curVelocity > maxSpeed && accelerationInput == FORWARD)
                return;

            //Limit so we cannot go faster in any direction while accelerating
            if (carRigidbody2D.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput != STOP)
                return;

            //Create a force for the engine
            Vector2 engineForceVector = transform.up * (accelerationInput * accelerationFactor);

            //Apply force and pushes the car forward
            carRigidbody2D.AddForce(engineForceVector);
        }
        
}
