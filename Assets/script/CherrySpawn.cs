using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherrySpawn : MonoBehaviour
{
    public GameObject cherryPrefab;
    public GameObject warningPrefab;

    [Header("Timing")]
    public float warningTime = 1.5f;
    public float spawnInterval = 15f;

    [Header("Offsets dari CameraFollow")]
    public float warningOffsetY = 1f;
    public float cherryOffsetY = 3f;

    [Header("Reference")]
    public Transform cameraFollow;

    [Header("Buff & Background Settings")]
    public float buffDuration = 5f;
    public int scoreMultiplier = 2;
    public SpriteRenderer backgroundRenderer;
    public Sprite redBackgroundSprite;

    private Camera cam;
    private CameraFollow scoreManager;
    private Sprite originalBg;
    private Coroutine buffRoutine;
    public bool IsBuffActive { get; private set; }

    void Start()
    {
        cam = Camera.main;
        if (cameraFollow == null)
            cameraFollow = Camera.main != null ? Camera.main.transform : null;

        scoreManager = FindObjectOfType<CameraFollow>();
        if (backgroundRenderer != null)
            originalBg = backgroundRenderer.sprite;

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

        StartCoroutine(WarningThenCherry(randomX));
    }

    IEnumerator WarningThenCherry(float xPos)
    {
        float z = warningPrefab != null ? warningPrefab.transform.position.z : 0f;
        if (cherryPrefab != null) z = cherryPrefab.transform.position.z;

        float camZ = Mathf.Abs(cam.transform.position.z);

        Vector3 topWorld = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, camZ));
        float topY = topWorld.y;

        Vector3 warningPos = new Vector3(xPos, topY - warningOffsetY, z);
        GameObject warning = Instantiate(warningPrefab, warningPos, Quaternion.identity);

        float t = 0f;
        while (t < warningTime)
        {
            float loopTopY = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, camZ)).y;
            warning.transform.position = new Vector3(xPos, loopTopY - warningOffsetY, warning.transform.position.z);

            t += Time.deltaTime;
            yield return null;
        }

        Destroy(warning);

        float cherryY = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, camZ)).y;
        Vector3 cherryPos = new Vector3(xPos, cherryY + cherryOffsetY, z);
        Instantiate(cherryPrefab, cherryPos, Quaternion.identity);
    }

    public void OnCherryEaten()
    {
        Debug.Log("ceri eaten");
        if (buffRoutine != null)
        {
            StopCoroutine(buffRoutine);
        }
        buffRoutine = StartCoroutine(ActivateBuff());
    }

    IEnumerator ActivateBuff()
    {
        IsBuffActive = true;

        if (backgroundRenderer != null && redBackgroundSprite != null)
        {
            backgroundRenderer.sprite = redBackgroundSprite;
        }

        if (scoreManager != null)
        {
            scoreManager.scoreMultiplier = scoreMultiplier;
        }

        yield return new WaitForSeconds(buffDuration);

        if (backgroundRenderer != null && originalBg != null)
            backgroundRenderer.sprite = originalBg;

        if (scoreManager != null)
            scoreManager.scoreMultiplier = 1;

        IsBuffActive = false;
        buffRoutine = null;
    }
}