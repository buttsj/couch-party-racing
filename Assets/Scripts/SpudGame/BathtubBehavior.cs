using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BathtubBehavior : MonoBehaviour {

    public float TubTimer { get; set; }
    private bool raising;
    private bool lowering;
    public Text waterRaisingText;
    public GameObject potato;
    float maxTubTimer;
    public AudioClip tornadoSiren;
    public AudioSource tubAudio;

    // Use this for initialization
    void Start () {
        TubTimer = 0;
        raising = false;
        tornadoSiren = Resources.Load<AudioClip>("Sounds/Tornado_Siren-SoundBible");
        tubAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {

        if (potato.GetComponent<SpudScript>().RoundCount > 1)
        {
            switch (potato.GetComponent<SpudScript>().RoundCount) {
                case 2:
                    maxTubTimer = 40;
                    break;
                case 3:
                    maxTubTimer = 25;
                    break;
            }

            TubTimer += Time.deltaTime;
            if (TubTimer > maxTubTimer - 5 && TubTimer < maxTubTimer)
            {
                waterRaisingText.text = "Water rising ! Get to high ground !";
                tubAudio.PlayOneShot(tornadoSiren);
            }

            if (TubTimer > maxTubTimer)
            {
                if (!lowering)
                {
                    raising = true;
                }
                waterRaisingText.text = "";
            }

            if (raising)
            {
                RaiseWater();
            }
            else
            {
                LowerWater();
            }
        }
	}
    
    void RaiseWater() {
        if (potato.GetComponent<SpudScript>().RoundCount == 2)
        {
            if (transform.position.y < 59)
            {
                transform.position += 4 * Time.deltaTime * Vector3.up;
            }
            else
            {
                raising = false;
                lowering = true;
            }
        }
        else {
            if (transform.position.y < 109)
            {
                transform.position += 8 * Time.deltaTime * Vector3.up;
            }
            else
            {
                raising = false;
                lowering = true;
            }
        }
    }

    void LowerWater() {
        if (potato.GetComponent<SpudScript>().RoundCount == 2)
        {
            if (transform.position.y > -3)
            {
                transform.position -= 4 * Time.deltaTime * Vector3.up;
            }
            else if (TubTimer > maxTubTimer)
            {
                TubTimer = 0;
                lowering = false;
            }
        }
        else {
            if (transform.position.y > -3)
            {
                transform.position -= 8 * Time.deltaTime * Vector3.up;
            }
            else if (TubTimer > maxTubTimer)
            {
                TubTimer = 0;
                lowering = false;
            }
        }
    }

}
