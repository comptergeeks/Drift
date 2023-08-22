using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class WheelController : MonoBehaviour
{
    [SerializeField] WheelCollider frontRight; 
    [SerializeField] WheelCollider frontLeft; 
    [SerializeField] WheelCollider backRight; 
    [SerializeField] WheelCollider backLeft; 

    [SerializeField] Transform frontRightTire; 
    [SerializeField] Transform frontLeftTire; 
    [SerializeField] Transform backRightTires;
    [SerializeField] Transform backLeftTires; 
    [SerializeField] public TMPro.TextMeshProUGUI rpmCount;  
    [SerializeField] public  TMPro.TextMeshProUGUI torqueCount; 
    [SerializeField] public  TMPro.TextMeshProUGUI speedCount;  
    [SerializeField] public  TMPro.TextMeshProUGUI isDriftingDisplay; 
    [SerializeField] public  TMPro.TextMeshProUGUI driftAngleDisplay; 
    [SerializeField] ParticleSystem[] TireSmokes; 
    ParticleSystem.EmissionModule tireSmokeEmission;
    [SerializeField] Rigidbody car; 
    [SerializeField] GameObject centerOfMass;

    public float acceleration; 
    public float brakingForce;
    public float maxTurnAngle;   

    private float currentAcceleration = 0f;
    private float currentBreakForce = 0f; 
    private float currentTurnAngle = 0f; 
    private float rpmCounter;
    private string display;
    private float torqueOut; 
    private string displayTorque; 
    private string speedo; 

    public MyButton gasPedal; 

    public MyButton brakePedal;

    public MyButton right; 
    public MyButton left; 
    public MyButton handBrake; 

    private float gas = 0f; 
    public float isTurn = 0f; 

    private float driftValue; 
    private float driftAngle; 
    private float whatIsDrift = -0.95f; 
    private float whatIsNoDrift = -0.15f; 

    public float addDownForce;

    


    
   private void Start() {
     car.centerOfMass = centerOfMass.transform.localPosition; 
    }
    private void FixedUpdate() {
        applyDownforce(); 
        calculateDrift(); 
        applyAccel();
        currentAcceleration = -acceleration * gas;
        backRight.motorTorque = currentAcceleration;  
        backLeft.motorTorque = currentAcceleration; 
        //frontRight.motorTorque = currentAcceleration; 
        //frontLeft.motorTorque = currentAcceleration; 
        //frontRight.brakeTorque = currentBreakForce;
        //frontLeft.brakeTorque = currentBreakForce; 
        backRight.brakeTorque = currentBreakForce; 
        backLeft.brakeTorque = currentBreakForce; 

        currentTurnAngle = maxTurnAngle * isTurn;

        frontLeft.steerAngle = currentTurnAngle; 
        frontRight.steerAngle = currentTurnAngle;  

        UpdateWheel(frontLeft, frontLeftTire); 
        UpdateWheel(frontRight, frontRightTire); 
        UpdateWheel(backLeft, backLeftTires);
        UpdateWheel(backRight, backRightTires);
    }
    void UpdateWheel(WheelCollider col, Transform tire) {
        rpmCounter = Mathf.Abs(col.rpm);
        rpmCounter = Mathf.RoundToInt(rpmCounter); 
        display = rpmCounter.ToString() + " RPM";
        rpmCount.text = display; 

        torqueOut = Mathf.Abs(col.motorTorque);
        torqueOut = Mathf.RoundToInt(torqueOut); 
        displayTorque = torqueOut.ToString() + " Torque";
        torqueCount.text = displayTorque; 
        speedo = (Mathf.RoundToInt((float)(car.velocity.magnitude * 2.237f))).ToString(); 

        speedCount.text = speedo + "MPH"; 

        isDriftingDisplay.text = "is Drift " + driftValue.ToString();
        driftAngleDisplay.text = "Drift Angle"+ driftAngle.ToString();  

        
        Vector3 position; 
        Quaternion rotation; 

        col.GetWorldPose(out position, out rotation);

        tire.rotation = rotation; 
    }

    void applyAccel() {
        if(gasPedal.isPressed) {
            gas = 1f; 
        } else if(brakePedal.isPressed) {
            gas = -1f; 
        } else {
            gas = 0f; 
        }
        if(right.isPressed) {
            isTurn = 1f; 
        } else if(left.isPressed) {
            isTurn = -1f; 
        } else {
            isTurn = 0f; 
        }
    }
    void calculateDrift() {
        driftValue = Vector3.Dot(car.velocity.normalized, car.transform.forward.normalized); 
        driftAngle = Mathf.Acos(driftValue) * Mathf.Rad2Deg; 
        createSmoke(); 
        

    } 
    void createSmoke() {
        for(int i = 0; i < TireSmokes.Length; i++) {
        tireSmokeEmission = TireSmokes[i].emission;
         if(driftValue > whatIsDrift && driftValue < whatIsNoDrift) {
            tireSmokeEmission.enabled = true;
         } else {
            tireSmokeEmission.enabled = false; 

         }
        }
       
    }
    void applyDownforce() {
        car.AddForce(-transform.up * addDownForce * car.velocity.magnitude); 
    }
}
