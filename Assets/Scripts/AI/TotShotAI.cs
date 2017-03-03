using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotShotAI : MonoBehaviour {

    private const float REDGOAL = -140.0f;
    private const float BLUEGOAL = 140.0f;

    private const float MINSPEED = 120f;
    private const float NORMALMAXSPEED = 200f;
    private const float BOOSTSPEED = 300f;

    private KartPhysics physics;
    private float boost;

    public GameObject fLeftModel;
    public GameObject fRightModel;
    public GameObject rLeftModel;
    public GameObject rRightModel;
    public GameObject steering_wheel;
    public GameObject fLParent;
    public GameObject fRParent;

    private GameObject tot;

    private KartAudio kartAudio;

    private float startDelayTimer;
    private float startDelayLimit; 

    private TotShotGameState gameState;

    public TotShotGameState GameState { set { gameState = value; } get { return gameState; } }

    void Start () {

        tot = GameObject.Find("Tot");

        boost = 100.0f;

        physics = new KartPhysics(gameObject, MINSPEED, NORMALMAXSPEED, BOOSTSPEED);
        kartAudio = new KartAudio(gameObject, physics, NORMALMAXSPEED, MINSPEED);

        startDelayTimer = 0.0f;
        startDelayLimit = Random.Range(0.0f, 1.1f);
    }
	
	void FixedUpdate () {

        if(startDelayTimer > startDelayLimit)
        {
            handleSteering();
            handleMovement();
            handleWheelAnimation();
        }
        else
        {
            startDelayTimer += Time.deltaTime;
        }

    }

    private void handleSteering()
    {
        Vector3 xzPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
        Vector3 xzTotPos = new Vector3(tot.transform.position.x, 0.0f, tot.transform.position.z);

        if (gameState.getTeam() == "red")
        {
            if (Vector3.Distance(xzPos, xzTotPos) < 4.0f)
            {
                if (IsGrounded())
                {
                    physics.TotJump1();
                }
                steerTowardsGoal(BLUEGOAL);
            }else
            {
                steerTowardsTot();
            }
        }
        else if (gameState.getTeam() == "blue")
        {
            if (Vector3.Distance(xzPos, xzTotPos) < 4.0f)
            {
                if (IsGrounded())
                {
                    physics.TotJump1();
                }
                steerTowardsGoal(REDGOAL);
            }
            else
            {
                steerTowardsTot();
            }
        }
    }

    private void steerTowardsTot()
    {
        Vector3 targetXZPosition = new Vector3(tot.transform.position.x, 0.0f, tot.transform.position.z);
        Vector3 aiXZPosition = new Vector3(transform.position.x, 0.0f, transform.position.z);

        Quaternion targetQuaternion = Quaternion.LookRotation((targetXZPosition - aiXZPosition).normalized);
        Quaternion slerpQuaternion = Quaternion.Slerp(transform.rotation, targetQuaternion, 0.2f);

        transform.rotation = new Quaternion(transform.rotation.x, slerpQuaternion.y, transform.rotation.z, slerpQuaternion.w);
    }

    private void steerTowardsGoal(float zTarget)
    {
        Vector3 targetXZPosition = new Vector3(0.0f, 0.0f, zTarget);
        Vector3 aiXZPosition = new Vector3(transform.position.x, 0.0f, transform.position.z);

        Quaternion targetQuaternion = Quaternion.LookRotation((targetXZPosition - aiXZPosition).normalized);
        Quaternion slerpQuaternion = Quaternion.Slerp(transform.rotation, targetQuaternion, 0.2f);

        transform.rotation = new Quaternion(transform.rotation.x, slerpQuaternion.y, transform.rotation.z, slerpQuaternion.w);
    }

    private void handleMovement()
    {
        physics.Accelerate();
        kartAudio.handleAccelerationGearingSounds(false);
        physics.ApplyForces();
    }

    private void handleWheelAnimation()
    {
        fLeftModel.transform.Rotate(Vector3.right * physics.Speed);
        fRightModel.transform.Rotate(Vector3.right * physics.Speed);
        rLeftModel.transform.Rotate(Vector3.right * physics.Speed);
        rRightModel.transform.Rotate(Vector3.right * physics.Speed);
    }

    private bool IsGrounded()
    {
        return Physics.SphereCast(new Ray(transform.position, -transform.up), 1f, 1);
    }

}
