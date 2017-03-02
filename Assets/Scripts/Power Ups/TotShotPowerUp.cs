using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotShotPowerUp : MonoBehaviour
{

    private AudioClip powerUpGet;

    void Start()
    {
        powerUpGet = Resources.Load<AudioClip>("Sounds/KartEffects/Item_Get_Sound");
    }


    void Update()
    {
    }

    void FixedUpdate()
    {
        transform.Rotate(Vector3.up, 2.0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            other.gameObject.GetComponent<Kart>().Boost = 100;
            gameObject.SetActive(false);
        }
    }
}
