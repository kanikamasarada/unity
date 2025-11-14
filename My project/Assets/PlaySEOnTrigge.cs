using UnityEngine;

public class PlaySEOnTrigger : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // 他のオブジェクトが触れたとき
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // PlayerのTagが必要
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}