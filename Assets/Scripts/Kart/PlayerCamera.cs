using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCamera : MonoBehaviour {

    public Transform player;
    public float followDistance;
    private Vector3 originalOrientation;
    private Vector3 offset;
    float fov;
    public bool Spectate = false;
    private bool isHuman;

    public GameObject[] players;
    private int index = 0;

	void Start() {
        offset = new Vector3(0, 10, 0);
        transform.position = player.position - followDistance * player.forward + offset;
        transform.localEulerAngles = new Vector3(player.localEulerAngles.x + 10, player.localEulerAngles.y, 0);
        fov = gameObject.GetComponent<Camera>().fieldOfView;

        if(player.gameObject.GetComponent<Kart>() != null)
        {
            isHuman = true;
            if (player.gameObject.GetComponent<Kart>().IsTotShotGameState) {
                gameObject.GetComponent<Camera>().fieldOfView = 70;
            }
        }

        if (Spectate)
        {
            Text text = FindObjectsOfType<Canvas>()[0].gameObject.AddComponent<Text>();
            text.text = "Press Space  / Bump Kart to quit\nPress E  / Use Item to switch perspective";
            Font NFSFont = Resources.Load<Font>("Fonts/NFS_by_JLTV");
            text.font = NFSFont;
            text.material = NFSFont.material;
        }

    }

    void LateUpdate() {
        if (isHuman)
        {
            if (!player.gameObject.GetComponent<Kart>().IsDamaged && !player.gameObject.GetComponent<Kart>().PhysicsObject.IsFlipping)
            {
                Vector3 tempPosition = new Vector3(followDistance * player.forward.x, 0, followDistance * player.forward.z);
                transform.position = player.position - followDistance * tempPosition.normalized + offset;
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, player.localEulerAngles.y, 0);
                originalOrientation = tempPosition;
            }
            else
            {
                transform.position = player.position - followDistance * originalOrientation.normalized + offset;
            }

            if (player.gameObject.GetComponent<Kart>().IsBoosting)
            {
                if (gameObject.GetComponent<Camera>().fieldOfView < 80)
                {
                    gameObject.GetComponent<Camera>().fieldOfView++;
                }

            }
            else
            {
                int minFov;
                if (player.gameObject.GetComponent<Kart>().IsTotShotGameState)
                {
                    minFov = 70;
                }
                else {
                    minFov = 60;
                }

                if (gameObject.GetComponent<Camera>().fieldOfView > minFov)
                {
                        gameObject.GetComponent<Camera>().fieldOfView--;
                }
            }
        }
        else
        {
            if (!player.gameObject.GetComponent<WaypointAI>().IsDamaged && !player.gameObject.GetComponent<WaypointAI>().PhysicsObject.IsFlipping)
            {
                Vector3 tempPosition = new Vector3(followDistance * player.forward.x, 0, followDistance * player.forward.z);
                transform.position = player.position - followDistance * tempPosition.normalized + offset;
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, player.localEulerAngles.y, 0);
                originalOrientation = tempPosition;
            }
            else
            {
                transform.position = player.position - followDistance * originalOrientation.normalized + offset;
            }

            if (player.gameObject.GetComponent<WaypointAI>().IsBoosting)
            {
                if (gameObject.GetComponent<Camera>().fieldOfView < 80)
                {
                    gameObject.GetComponent<Camera>().fieldOfView++;
                }

            }
            else
            {
                if (gameObject.GetComponent<Camera>().fieldOfView > 60)
                {
                    gameObject.GetComponent<Camera>().fieldOfView--;
                }
            }
            if (Spectate)
            {
                if (SimpleInput.GetButtonDown("Use PowerUp"))
                {
                    index = index + 1;
                    if (index >= 4)
                        index = 0;
                    player = players[index].transform;
                }
                if (SimpleInput.GetButtonDown("Bump Kart"))
                {
                    SceneManager.LoadScene(0);
                }
            }
        }

    }
}
