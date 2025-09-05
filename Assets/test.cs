using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public string str = "さんぷる";
    int cnt;
    void Start()
    {
        cnt = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(cnt + str + " ");
        str = str + "!";
        cnt++;
        if(cnt % 2 == 0)
        Debug.Log("偶数");
    
    }
}
