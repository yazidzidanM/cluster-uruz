using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool GameIsOver;

    [Header("References")]
    public GameObject batangPrefab;
    public Transform player;
    public Transform treeSystem;
    public GameObject loseUi;

    [Header("Pengaturan Batang")]
    [Tooltip("Posisi Y tempat batang pertama muncul")]
    public float firstBatangY = 0f;

    [Tooltip("Jarak vertikal antar batang")]
    public float jarakAntarBatang = 10f;

    [Tooltip("Berapa unit sebelum ujung batang berikutnya, batang baru dibuat")]
    public float spawnTriggerDistance = 8f;

    [Tooltip("Batang yang lebih rendah dari player sejauh ini akan dihancurkan")]
    public float destroyDistance = 15f;

    private float nextBatangY;
    private float highestPlayerY;

    private float batangX;

    private List<GameObject> batangAktif = new List<GameObject>();

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioSource bgm;
    public AudioClip loseSound;

    private void Start()
    {
        Time.timeScale = 1f;
        GameIsOver = false;

        if (loseUi != null)
            loseUi.SetActive(false);

        if (player != null)
            highestPlayerY = player.position.y;

        SpawnBatang(firstBatangY);

        nextBatangY = firstBatangY + jarakAntarBatang;
    }

    private void Update()
    {
        if (GameIsOver)
            return;

        UpdateHighestPlayerY();

        CheckSpawnBatang();

        CheckDestroyBatang();
    }

    private void UpdateHighestPlayerY()
    {
        if (player == null)
            return;

        if (player.position.y > highestPlayerY)
        {
            highestPlayerY = player.position.y;
        }
    }

    private void CheckSpawnBatang()
    {
        if (player == null)
            return;

        if (highestPlayerY + spawnTriggerDistance >= nextBatangY)
        {
            SpawnBatang(nextBatangY);

            nextBatangY += jarakAntarBatang;
        }
    }

    private void SpawnBatang(float y)
    {
        Vector3 spawnPosition = new Vector3(
            batangX,
            y,
            0f
        );

        GameObject newBatang = Instantiate(
            batangPrefab,
            spawnPosition,
            Quaternion.identity,
            treeSystem
        );

        batangAktif.Add(newBatang);

        BranchSpawner branchSpawner =
            newBatang.GetComponent<BranchSpawner>();

        if (branchSpawner != null)
        {
            branchSpawner.GenerateBranches();
        }
        else
        {
            Debug.LogWarning(
                "Batang tidak memiliki component BranchSpawner!"
            );
        }

        if (batangAktif.Count == 1)
        {
            batangX = newBatang.transform.position.x;
        }
    }

    private void CheckDestroyBatang()
    {
        if (batangAktif.Count == 0)
            return;

        for (int i = batangAktif.Count - 1; i >= 0; i--)
        {
            GameObject batang = batangAktif[i];

            if (batang == null)
            {
                batangAktif.RemoveAt(i);
                continue;
            }

            if (batang.transform.position.y <
                highestPlayerY - destroyDistance)
            {
                Destroy(batang);
                batangAktif.RemoveAt(i);
            }
        }
    }

    public void Die()
    {
        GameIsOver = true;

        if (loseUi != null)
            loseUi.SetActive(true);

        if (bgm != null)
            bgm.Pause();

        if (audioSource != null && loseSound != null)
            audioSource.PlayOneShot(loseSound);

        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        GameIsOver = false;

        SceneManager.LoadScene("GamePlay");
    }

    public void BackToMainMenuButton()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(0);
    }
}
