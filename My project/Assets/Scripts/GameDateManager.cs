using UnityEngine;
using System;

public class GameDateManager : MonoBehaviour
{
    public static GameDateManager Instance;

    public int day = 1; // 現在の日付
    public event Action OnDateChanged;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 日付を1日進める
    public void NextDay()
    {
        day++;
        Debug.Log($"現在日付: Day {day}");
        Debug.Log("OnDateChanged イベントを発火します");
        OnDateChanged?.Invoke();
    }
}
