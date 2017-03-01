using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotShotAI : MonoBehaviour {

    private const float MINSPEED = 175f;
    private const float NORMALMAXSPEED = 250f;
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

    void Start () {

        tot = GameObject.Find("Tot");

        boost = 100.0f;

        physics = new KartPhysics(gameObject, MINSPEED, NORMALMAXSPEED, BOOSTSPEED);
        kartAudio = new KartAudio(gameObject, physics, NORMALMAXSPEED, MINSPEED);

    }
	
	void FixedUpdate () {
        steerTowardsTot();
        handleMovement();
        handleWheelAnimation();
    }

    private void steerTowardsTot()
    {
        Vector3 targetXZPosition = new Vector3(tot.transform.position.x, tot.transform.position.y, tot.transform.position.z);
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

}
