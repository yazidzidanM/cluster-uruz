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
    public int HighScore;

    [Header("Pengaturan Batang")]
    [Tooltip("Offset Y awal jika ingin posisi awal sedikit lebih naik/turun dari player")]
    public float playerYOffset = 0f;

    [Tooltip("Panjang/Tinggi vertikal 1 prefab batang (Misal: 10 unit)")]
    public float jarakAntarBatang = 10f;

    [Tooltip("Seberapa tinggi batang harus disiapkan di atas posisi tertinggi player")]
    public float bufferDistanceAhead = 20f;

    [Tooltip("Jarak di bawah player untuk menghancurkan batang tua")]
    public float destroyDistance = 15f;

    private float highestPlayerY;
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

        // Track posisi Y player pertama kali
        if (player != null)
        {
            highestPlayerY = player.position.y;
        }

        // Spawn batang pertama tepat di posisi player
        float startY = highestPlayerY + playerYOffset;
        SpawnBatang(startY);

        // Panggil pemicu awal agar batang langsung di-generate beberapa meter ke atas!
        CheckSpawnBatang();
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

        // Selalu catat posisi Y tertinggi yang pernah dicapai player
        if (player.position.y > highestPlayerY)
        {
            highestPlayerY = player.position.y;
        }
    }

    private void CheckSpawnBatang()
    {
        if (player == null || batangAktif.Count == 0)
            return;

        // Gunakan WHILE loop:
        // Selama ujung batang teratas belum melebih jarak buffer (misal: 20 unit di atas player),
        // terus generate batang baru di atasnya!
        while (true)
        {
            GameObject batangTeratas = batangAktif[batangAktif.Count - 1];
            float ujungBatangTeratasY = batangTeratas.transform.position.y + jarakAntarBatang;

            // Jika tinggi batang teratas sudah melebihi (highestPlayerY + bufferDistanceAhead), stop loop
            if (ujungBatangTeratasY >= highestPlayerY + bufferDistanceAhead)
            {
                break;
            }

            // Spawn batang berikutnya persis di ujung atas batang teratas
            SpawnBatang(ujungBatangTeratasY);
        }
    }

    private void SpawnBatang(float worldY)
    {
        float spawnX = (treeSystem != null) ? treeSystem.position.x : 0f;
        Vector3 spawnWorldPos = new Vector3(spawnX, worldY, 0f);

        GameObject newBatang = Instantiate(batangPrefab, spawnWorldPos, Quaternion.identity);

        if (treeSystem != null)
        {
            newBatang.transform.SetParent(treeSystem, true);
        }

        batangAktif.Add(newBatang);

        // Generate cabang-cabang otomatis di batang baru
        BranchSpawner branchSpawner = newBatang.GetComponent<BranchSpawner>();
        if (branchSpawner != null)
        {
            branchSpawner.GenerateBranches();
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

            // Hapus jika ujung atas batang sudah tertinggal di bawah player
            if (batang.transform.position.y + jarakAntarBatang < highestPlayerY - destroyDistance)
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

    public static GameManager Instance; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}