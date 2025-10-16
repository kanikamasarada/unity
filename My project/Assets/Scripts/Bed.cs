using UnityEngine;

public class Bed : MonoBehaviour
{
    public SleepUI sleepUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sleepUI.Show();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sleepUI.Hide();
        }
    }
}
