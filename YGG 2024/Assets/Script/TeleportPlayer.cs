using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public GameObject TeleportPoint;
    private GameObject player;
    private Camera cam;            
    public Vector3 cameraOffset = new Vector3(0f, 0f, -10f);

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        cam = Camera.main;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Teleport();
            Debug.Log("Detected " + collision.gameObject);
        }
    }
    public void Teleport()
    {
        if (TeleportPoint == null || player == null || cam == null)
        {
            Debug.LogError("TeleportPoint, player, or camera is not assigned.");
            return;
        }
        player.transform.position = TeleportPoint.transform.position;
        cam.transform.position = TeleportPoint.transform.position + cameraOffset;
        Debug.Log("Player and camera teleported successfully!");
    }
}
