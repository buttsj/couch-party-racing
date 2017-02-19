using UnityEngine;

public class KartAudio {
    private const string SPINOUT = "Sounds/KartEffects/cpuspin";

    private AudioSource engineAudioSource;
    private AudioSource oneOffAudioSource;

    private KartPhysics physics;
    private float maxSpeed;
    private float minSpeed;

    private AudioClip spinout;
    private bool spinOutPlayed;

    public bool SpinOutPlayed { set { spinOutPlayed = value; } }

    public KartAudio(GameObject kart, KartPhysics physics,float maxSpeed, float minSpeed)
    {
        engineAudioSource = kart.GetComponent<AudioSource>();
        oneOffAudioSource = kart.GetComponents<AudioSource>()[1];

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
                engineAudioSource.pitch = (currentTravelingSpeed / (currentMaxSpeed * (1.0f / 2.5f))) + 1.5f;
            }
            else
            {
                engineAudioSource.pitch = (currentTravelingSpeed / currentMaxSpeed) + 1.5f;
            }
        }
        else
        {
            float currentTravelingSpeed = physics.Speed - minSpeed;
            float currentTravelingMaxSpeed = maxSpeed - minSpeed;
            if (currentTravelingSpeed < (currentTravelingMaxSpeed * (1.0f / 2.5f)))
            {
                engineAudioSource.pitch = (currentTravelingSpeed / (currentTravelingMaxSpeed * (1.0f / 2.5f))) + 2.0f;
            }
            else
            {
                engineAudioSource.pitch = (currentTravelingSpeed / currentTravelingMaxSpeed) + 2.0f;
            }
        }
    }

    public void handleCoastingGearingSounds(bool isBoosting)
    {
        if (!isBoosting)
        {
            engineAudioSource.pitch = ((physics.Speed - minSpeed) / (maxSpeed - minSpeed)) + 1.5f;
        }
        else
        {
            engineAudioSource.pitch = ((physics.Speed - minSpeed) / (maxSpeed - minSpeed)) + 2.0f;
        }
    }

    public void handleDamageGearingSounds()
    {
        engineAudioSource.pitch = 1.5f;
    }

    public void spinOutSound()
    {
        if (!spinOutPlayed)
        {
            oneOffAudioSource.PlayOneShot(spinout, 1.0f);
            spinOutPlayed = true;
        }
    }

    public void playOneOff(AudioClip clip)
    {
        oneOffAudioSource.PlayOneShot(clip, 1.0f);
    }
}
