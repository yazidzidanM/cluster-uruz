using System.IO;
using UnityEngine;
using TMPro;

public class LostUi : MonoBehaviour
{
    public CameraFollow CameraFollowScript;

    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI HighScoreText;

    public int score;
    public int HighScore;

    private string savePath;

    [System.Serializable]
    public class SaveData
    {
        public int highScore;
    }

    void Start()
    {
        // Lokasi file JSON
        savePath = Application.persistentDataPath + "/saveData.json";

        // Ambil high score lama
        LoadHighScore();

        // Update tampilan awal
        PerbaruiHighScoreUI(HighScore);
    }

    void Update()
    {
        if (CameraFollowScript != null)
        {
            score = CameraFollowScript.score;
        }

        // Current Score
        if (currentScoreText != null)
        {
            PerbaruiCurrentScoreUI(score);
        }

        // Kalau score sekarang lebih tinggi dari highscore
        if (score > HighScore)
        {
            HighScore = score;

            PerbaruiHighScoreUI(HighScore);

            // Simpan ke JSON
            SaveHighScore();
        }
        else
        {
            PerbaruiHighScoreUI(HighScore);
        }
    }

    public void PerbaruiCurrentScoreUI(int skorBaru)
    {
        if (currentScoreText != null)
        {
            currentScoreText.text = "Current Score  :   " + skorBaru;
        }
    }

    public void PerbaruiHighScoreUI(int skorBaru)
    {
        if (HighScoreText != null)
        {
            HighScoreText.text = "High Score  :   " + skorBaru;
        }
    }

    public void SaveHighScore()
    {
        SaveData data = new SaveData();

        data.highScore = HighScore;

        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(savePath, json);

        Debug.Log("High Score disimpan: " + HighScore);
        Debug.Log("Lokasi Save: " + savePath);
    }

    public void LoadHighScore()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);

            SaveData data = JsonUtility.FromJson<SaveData>(json);

            HighScore = data.highScore;

            Debug.Log("High Score dimuat: " + HighScore);
        }
        else
        {
            HighScore = 0;

            // Bikin file save pertama kali
            SaveHighScore();

            Debug.Log("Belum ada save. Membuat save baru.");
        }
    }
}