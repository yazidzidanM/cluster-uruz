
using UnityEngine;

public class MovingPatrol : MonoBehaviour
{
    private float speed;
    private float distance;
    private float startX;
    private int direction = -1;

    public void Setup(
        float speedSetting,
        float distanceSetting,
        bool startMovingLeft
    )
    {
        speed = speedSetting;
        distance = distanceSetting;

        startX = transform.position.x;

        direction = startMovingLeft ? -1 : 1;

        UpdateSpriteFacing();
    }

    void Update()
    {
        transform.position +=
            Vector3.right * direction * speed * Time.deltaTime;

        float distanceMoved =
            Mathf.Abs(transform.position.x - startX);

        if (distanceMoved >= distance)
        {
            direction *= -1;

            startX = transform.position.x;

            UpdateSpriteFacing();
        }
    }

    void UpdateSpriteFacing()
    {
        if (direction == 1)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}
