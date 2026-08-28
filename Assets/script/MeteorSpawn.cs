using System.Collections;
using UnityEngine;

public class MeteorSpawn : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject meteorPrefab;
    public GameObject warningPrefab;
    public GameObject LoseUi;

    [Header("Camera")]
    public Transform cameraFollow;

    [Header("Spawn Settings")]
    public float spawnInterval = 6f;
    public float spawnOffsetY = 3f;
    public float spawnOffsetOutsideFrame = 2f;

    [Header("Patrol Settings")]
    public float moveSpeed = 5f;
    public float patrolDistance = 10f;

    private Camera cam;

    void Start()
    {
        if (LoseUi != null)
            LoseUi.SetActive(false);

        cam = Camera.main;

        if (cameraFollow == null && cam != null)
            cameraFollow = cam.transform;

        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnWalkingObject();

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnWalkingObject()
    {
        if (meteorPrefab == null || cam == null)
            return;

        // Ukuran kamera
        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        // ==============================
        // POSISI X
        // ==============================

        // Spawn dari kanan, sedikit di luar layar
        float rightX = cam.transform.position.x + halfWidth;
        float spawnX = rightX + spawnOffsetOutsideFrame;


        // ==============================
        // POSISI Y
        // ==============================

        // Batas ATAS kamera
        float topY = cam.transform.position.y + halfHeight;

        // Spawn beberapa unit DI ATAS kamera
        float spawnY = topY + spawnOffsetY;


        Vector3 spawnPosition = new Vector3(
            spawnX,
            spawnY,
            0f
        );


        // ==============================
        // SPAWN OBJECT
        // ==============================

        GameObject newObj = Instantiate(
            meteorPrefab,
            spawnPosition,
            Quaternion.identity
        );


        // ==============================
        // MATIKAN GRAVITY
        // ==============================

        Rigidbody2D rb = newObj.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;

            // Sangat disarankan kalau gerakan dikontrol script
            rb.bodyType = RigidbodyType2D.Kinematic;
        }


        // ==============================
        // MOVING PATROL
        // ==============================

        MovingPatrol patrol = newObj.GetComponent<MovingPatrol>();

        if (patrol == null)
            patrol = newObj.AddComponent<MovingPatrol>();


        // Mulai bergerak ke kiri
        patrol.Setup(
            moveSpeed,
            patrolDistance,
            true
        );
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (LoseUi != null)
                LoseUi.SetActive(true);

            Time.timeScale = 0f;
        }
    }
}

