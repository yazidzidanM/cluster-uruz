using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : MonoBehaviour
{
  [Header("Cherry Settings")]
  public float fallSpeed = 2f;
  public Rigidbody2D rb;

  [Header("Cherry Audio")]
  public AudioSource audioSource;
  public AudioClip collideSound;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    rb.velocity = Vector2.down * fallSpeed;
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    Debug.Log("ceri triggered");
    if (other.CompareTag("void"))
    {
      Destroy(gameObject);
      return;
    }

    if (!other.CompareTag("Player"))
      return;

    if (audioSource != null && collideSound != null)
    {
      Debug.Log("sound");
    }
    CherrySpawn spawner = Object.FindFirstObjectByType<CherrySpawn>();
    if (spawner != null)
    {
      spawner.OnCherryEaten();
    }

    Destroy(gameObject);
  }
}