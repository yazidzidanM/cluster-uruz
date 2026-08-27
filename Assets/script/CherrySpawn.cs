using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherrySpawn : MonoBehaviour
{
    public GameObject cherryPrefab;
    public GameObject cherryHintPrefab;
    public GameObject BoostedUi;

    [Header("Timing")]
    public float hintTime = 1.5f;
    public float spawnInterval = 30f;

    [Header("Offsets dari CameraFollow")]
    public float hintOffsetY = 1f;
    public float CherryOffsetY = 3f;

    [Header("Reference")]
    public Transform cameraFollow;

    private Camera cam;

    void Start()
    {
        BoostedUi.SetActive(false);
        cam = Camera.main;
        if (cameraFollow == null)
            cameraFollow = Camera.main != null ? Camera.main.transform : null;

        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnCherrySequence();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnCherrySequence()
    {
        float halfWidth = cam.orthographicSize * cam.aspect;

        float randomX = Random.Range(cam.transform.position.x - halfWidth, cam.transform.position.x + halfWidth);

        StartCoroutine(HintThenCherry(randomX));
    }

    IEnumerator HintThenCherry(float xPos)
    {
        float z = cherryHintPrefab != null ? cherryHintPrefab.transform.position.z : 0f;
        if (cherryPrefab != null) z = cherryPrefab.transform.position.z;

        float camZ = Mathf.Abs(cam.transform.position.z);

        Vector3 topWorld = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, camZ));
        float topY = topWorld.y;


        Vector3 warningPos = new Vector3(xPos, topY - hintOffsetY, z);
        GameObject warning = Instantiate(cherryHintPrefab, warningPos, Quaternion.identity);

        float t = 0f;
        while (t < hintTime)
        {
            float loopTopY = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, camZ)).y;
            warning.transform.position = new Vector3(xPos, loopTopY - hintOffsetY, warning.transform.position.z);

            t += Time.deltaTime;
            yield return null;
        }

        Destroy(warning);

        float cherryY = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, camZ)).y;
        Vector3 cherryPos = new Vector3(xPos, cherryY + CherryOffsetY, z);
        Instantiate(cherryPrefab, cherryPos, Quaternion.identity);


    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            BoostedUi.SetActive(true);
        }
    }
}
