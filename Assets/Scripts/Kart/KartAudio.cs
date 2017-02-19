using UnityEngine;

public class KartAudio {
    private const string SPINOUT = "Sounds/KartEffects/cpuspin";

    private GameObject kart;
    private KartPhysics physics;
    private float maxSpeed;
    private float minSpeed;

    private AudioClip spinout;
    private bool spinOutPlayed;

    public bool SpinOutPlayed { set { spinOutPlayed = value; } }

    public KartAudio(GameObject kart, KartPhysics physics,float maxSpeed, float minSpeed)
    {
        this.kart = kart;
        this.physics = physics;
        this.maxSpeed = maxSpeed;
        this.minSpeed = minSpeed;

        spinout = (AudioClip)Resources.Load(SPINOUT);
        spinOutPlayed = false;
    }

    public void handleAccelerationGearingSounds(bool isBoosting)
    {
        if (!isBoosting)
        {
            float currentTravelingSpeed = physics.Speed - minSpeed;
            float currentMaxSpeed = maxSpeed - minSpeed;
            if (currentTravelingSpeed < (currentMaxSpeed * (1.0f / 2.5f)))
            {
                kart.GetComponent<AudioSource>().pitch = (currentTravelingSpeed / (currentMaxSpeed * (1.0f / 2.5f))) + 1.5f;
            }
            else
            {
                kart.GetComponent<AudioSource>().pitch = (currentTravelingSpeed / currentMaxSpeed) + 1.5f;
            }
        }
        else
        {
            float currentTravelingSpeed = physics.Speed - minSpeed;
            float currentTravelingMaxSpeed = maxSpeed - minSpeed;
            if (currentTravelingSpeed < (currentTravelingMaxSpeed * (1.0f / 2.5f)))
            {
                kart.GetComponent<AudioSource>().pitch = (currentTravelingSpeed / (currentTravelingMaxSpeed * (1.0f / 2.5f))) + 2.0f;
            }
            else
            {
                kart.GetComponent<AudioSource>().pitch = (currentTravelingSpeed / currentTravelingMaxSpeed) + 2.0f;
            }
        }
    }

    public void handleCoastingGearingSounds(bool isBoosting)
    {
        if (!isBoosting)
        {
            kart.GetComponent<AudioSource>().pitch = ((physics.Speed - minSpeed) / (maxSpeed - minSpeed)) + 1.5f;
        }
        else
        {
            kart.GetComponent<AudioSource>().pitch = ((physics.Speed - minSpeed) / (maxSpeed - minSpeed)) + 2.0f;
        }
    }

    public void handleDamageGearingSounds()
    {
        kart.GetComponent<AudioSource>().pitch = 1.5f;
    }

    public void spinOutSound()
    {
        if (!spinOutPlayed)
        {
            kart.GetComponents<AudioSource>()[1].PlayOneShot(spinout, 1.0f);
            spinOutPlayed = true;
        }
    }

}
