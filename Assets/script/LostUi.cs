using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LostUi : MonoBehaviour
{

    public CameraFollow CameraFollowScript;
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI HighScoreText;

    public int score;
    public int HighScore;

    void Start()
    {
        if (currentScoreText != null)
        {
            string teksMentah = currentScoreText.text; 
            Debug.Log("Isi teks lengkap: " + teksMentah); 
        }
        if (currentScoreText != null)
        {
            string teksLengkap = currentScoreText.text;
            Debug.Log("Teks Lengkap: " + teksLengkap); // Output: "Current Score   :   10"

            // 2. Mengambil angka "10" saja dan mengubahnya menjadi tipe data int (angka)
            int nilaiAngka = AmbilAngkaSaja(teksLengkap);
            Debug.Log("Angka Skor yang Didapat: " + nilaiAngka); // Output: 10
        }
    }


    public void Update()
    {
        if(CameraFollowScript != null)
        {
            score = CameraFollowScript.score;
            HighScore = CameraFollowScript.score;
        }
        if(currentScoreText != null){
            PerbaruiCurrentScoreUI(score);

        }
        if(HighScoreText != null)
        {
            if(score > HighScore){
                PerbaruiHighScoreUI(HighScore);
            }else{
                PerbaruiHighScoreUI(score);
            }
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

    private int AmbilAngkaSaja(string inputTeks)
    {
        string angkaSaja = System.Text.RegularExpressions.Regex.Match(inputTeks, @"\d+").Value;
        
        if (int.TryParse(angkaSaja, out int hasil))
        {
            return hasil;
        }
        return 0;
    }
}
