using UnityEngine;

public class BranchSpawner: MonoBehaviour
{
    [Header("Branch Prefab & Settings")]
    public GameObject branchPrefab;
    
    [Header("Height Limits")]
    public float minY = 0.5f;
    public float maxY = 9.5f;

    [Header("Spacing Settings")]
    [Tooltip("Jarak terdekat antar cabang (Y)")]
    public float minDistance = 1.0f; // Jarak minimal agar tidak bertumpukan
    
    [Tooltip("Jarak terjauh antar cabang (Y)")]
    public float maxDistance = 2.5f; // Jarak maksimal agar tidak terlalu sepi

    public void GenerateBranches()
    {
        if (branchPrefab == null)
        {
            Debug.LogWarning("Branch Prefab belum diisi pada " + gameObject.name);
            return;
        }

        // Mulai dari posisi minY
        float currentY = minY;

        // Loop terus selama posisi Y belum melewati batas maxY
        while (currentY <= maxY)
        {
            // Acak arah (Kanan / Kiri)
            bool spawnOnRight = Random.value > 0.5f;

            // Posisikan tepat di tengah (X = 0) pada tinggi Y saat ini
            Vector3 localPosition = new Vector3(0f, currentY, 0f);

            // Spawn cabang
            GameObject newBranch = Instantiate(branchPrefab, transform);
            newBranch.transform.localPosition = localPosition;
            newBranch.transform.localRotation = Quaternion.identity;

            // Flip scale berdasarkan arah
            newBranch.transform.localScale = spawnOnRight
                ? Vector3.one
                : new Vector3(-1f, 1f, 1f);

            // Tambahkan jarak acak untuk posisi cabang berikutnya
            float randomStep = Random.Range(minDistance, maxDistance);
            currentY += randomStep;
        }
    }
}