using UnityEngine;
using TMPro;

public class CarControl : MonoBehaviour
{
    
    // Motor
    public float motorTorque = 2000;
    public float brakeTorque = 2000;
    public float maxSpeed = 20;
    public float steeringRange = 30;
    public float steeringRangeAtMaxSpeed = 10;
    public float centreOfGravityOffset = -1f;

    private bool caurisCamOn = false;
    public TextMeshProUGUI caurisCamStatusText;

    private string caurisCamOnlineString = "CaurisCam ONLINE";
    private string caurisCamOfflineString = "CaurisCam OFFLINE";

    // Audio
    public float audioPitch = 0.75f;
    public float minPitch = 0.5f;
    public float pitchDenominator = 50f;

    private AudioSource accelAudioSource;
    private AudioSource brakeAudioSource;
    private AudioSource idleAudioSource;
    private AudioSource warningAudioSource;

    public AudioClip carAcceleration;
    public AudioClip carIdle;
    public AudioClip carBrake;

    public AudioClip warning;

    WheelControl[] wheels;
    Rigidbody rigidBody;

    /*
    // Fog
    public ParticleSystem fogParticleSystem;
    public float fogSpeedDenominator = 5;
    */

    // Rain
    public ParticleSystem rainParticleSystem;
    public float rainSpeedDenominator = 2;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        // Adjust center of mass vertically, to help prevent the car from rolling
        rigidBody.centerOfMass += Vector3.up * centreOfGravityOffset;

        // Find all child GameObjects that have the WheelControl script attached
        wheels = GetComponentsInChildren<WheelControl>();

        idleAudioSource = gameObject.AddComponent<AudioSource>();
        brakeAudioSource = gameObject.AddComponent<AudioSource>();
        
        warningAudioSource = gameObject.AddComponent<AudioSource>();
        warningAudioSource.clip = warning;

        idleAudioSource.clip = carIdle;
        idleAudioSource.loop = true;
        idleAudioSource.Play();

        caurisCamStatusText.text = caurisCamOfflineString;
    }

    public void PlayWarning(){
        if(caurisCamOn){
            warningAudioSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.P)){
            // Toggle cauriscam
            if(caurisCamOn){
                // Toggle off
                caurisCamOn = false;
                caurisCamStatusText.text = caurisCamOfflineString;
            } else {
                // Toggle off
                caurisCamOn = true;
                caurisCamStatusText.text = caurisCamOnlineString;
            }
        }

        // Get keyboard input
        float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");

        // Get Logitech G29 input
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            var state = LogitechGSDK.LogiGetStateUnity(0);

            // Steering
            hInput = (state.lX / 32768f)/1.2f; // Normalize to -1 to 1

            // Pedals (accelerator and brake)
            float accelerator = (32768f - state.lY) / 32768f; // Normalize to 0 to 1
            float brake = (32768f - state.lRz) / 32768f; // Normalize to 0 to 1

            // Move the car
            vInput = accelerator - brake; // Combine accelerator and brake inputs

        }
        // Calculate current speed in relation to the forward direction of the car
        // (this returns a negative number when traveling backwards)
        float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.velocity);

        /*
        // fog speed handling: faster car -> fog should also move faster
        float fogSpeed = forwardSpeed / fogSpeedDenominator;
        var mainFog = fogParticleSystem.main;
        mainFog.simulationSpeed = fogSpeed;
        */

        // increase rain speed similarly, simulation speed increases both emission and particle speed
        float rainSpeed = forwardSpeed / rainSpeedDenominator;
        var mainRain = rainParticleSystem.main;
        mainRain.simulationSpeed = Mathf.Max(rainSpeed, 1); // keep the rain going also if going backwards or sitting still

        // audio pitch determined by speed
        idleAudioSource.pitch = Mathf.Max(minPitch, Mathf.Abs(forwardSpeed / pitchDenominator));

        // Calculate how close the car is to top speed
        // as a number from zero to one
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);

        // Use that to calculate how much torque is available 
        // (zero torque at top speed)
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);

        // …and to calculate how much to steer 
        // (the car steers more gently at top speed)
        float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        // Check whether the user input is in the same direction 
        // as the car's velocity
        bool isAccelerating = Mathf.Sign(vInput) == Mathf.Sign(forwardSpeed);

        if(!isAccelerating && (vInput != 0)) {
            // Brake
            brakeAudioSource.PlayOneShot(carBrake, idleAudioSource.pitch/6);
        }

        foreach (var wheel in wheels)
        {
            // Apply steering to Wheel colliders that have "Steerable" enabled
            if (wheel.steerable)
            {
                wheel.WheelCollider.steerAngle = hInput * currentSteerRange;
            }
            
            if (isAccelerating)
            {
                // Apply torque to Wheel colliders that have "Motorized" enabled
                if (wheel.motorized)
                {
                    wheel.WheelCollider.motorTorque = vInput * currentMotorTorque;
                }
                wheel.WheelCollider.brakeTorque = 0;
            }
            else
            {
                // If the user is trying to go in the opposite direction
                // apply brakes to all wheels
                wheel.WheelCollider.brakeTorque = Mathf.Abs(vInput) * brakeTorque;
                wheel.WheelCollider.motorTorque = 0;
            }
        }
    }
}