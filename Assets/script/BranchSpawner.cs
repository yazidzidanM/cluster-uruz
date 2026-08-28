using UnityEngine;

public class BranchSpawner : MonoBehaviour
{
    [Header("Aset Ranting")]
    public GameObject branchPrefab;

    [Header("Pengaturan Posisi")]
    public float trunkRadius = 1.5f;

    public int minBranches = 2;
    public int maxBranches = 4;

    public void GenerateBranches()
    {
        if (branchPrefab == null)
        {
            Debug.LogWarning(
                "Branch Prefab belum diisi pada " + gameObject.name
            );
            return;
        }

        int branchCount = Random.Range(minBranches, maxBranches + 1);

        float minY = 0.5f;
        float maxY = 9.5f;
        float segmentHeight = (maxY - minY) / branchCount;

        for (int i = 0; i < branchCount; i++)
        {
            float segmentStart = minY + (segmentHeight * i);
            float segmentEnd = segmentStart + segmentHeight;

            float randomY = Random.Range(segmentStart, segmentEnd);

            bool spawnOnRight = Random.value > 0.5f;

            float posX = spawnOnRight ? trunkRadius : -trunkRadius;

            Vector3 localPosition = new Vector3(posX, randomY, 0f);

            GameObject newBranch = Instantiate(branchPrefab, transform);

            newBranch.transform.localPosition = localPosition;
            newBranch.transform.localRotation = Quaternion.identity;

            newBranch.transform.localScale =
                spawnOnRight
                    ?
                    Vector3.one
                    :
                    new Vector3(-1f, 1f, 1f);
        }
    }
}
