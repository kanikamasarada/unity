using UnityEngine;

public class DateObjectBehaviour : MonoBehaviour
{
    public Renderer targetRenderer;
    public DayChangeData[] changes;

    void Start()
    {
        if (GameDateManager.Instance == null)
        {
            Debug.LogWarning("GameDateManager.Instance is NULL in " + gameObject.name);
            return;
        }

        GameDateManager.Instance.OnDateChanged += CheckDate;
        CheckDate();
    }

    void OnEnable()
    {
        if (GameDateManager.Instance != null)
            GameDateManager.Instance.OnDateChanged += CheckDate;
    }

    void OnDisable()
    {
        if (GameDateManager.Instance != null)
            GameDateManager.Instance.OnDateChanged -= CheckDate;
    }

    void CheckDate()
    {
        Debug.Log($"[{gameObject.name}] CheckDate called. Current day: {GameDateManager.Instance.day}");

        foreach (var change in changes)
        {
            Debug.Log($"Comparing: change.day={change.day}, current day={GameDateManager.Instance.day}");

            if (change.day == GameDateManager.Instance.day)
            {
                Debug.Log("Applying change for day " + change.day + " on object " + gameObject.name);

                transform.position = change.newPosition;
                gameObject.SetActive(change.active);

                if (targetRenderer != null && change.newTexture != null)
                {
                    targetRenderer.material.mainTexture = change.newTexture;
                    Debug.Log("Texture changed on object " + gameObject.name);
                }
            }
        }
    }
}
