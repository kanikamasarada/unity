using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class DialogueTrigger : MonoBehaviour
{
    public bool isTalking = false; // 会話中フラグ
    public float talkDistance = 3f; // 会話可能距離
    public List<DialogueLine> dialogueLines = new List<DialogueLine>()
    {
        new DialogueLine("？？？", "……ここはどこだ？"),
        new DialogueLine("？？？", "辺りは静まり返っている……。"),
        new DialogueLine("主人公", "……何かの気配がする。")
    };

    private static DialogueTrigger currentTarget; // 現在見ている会話対象

    void Update()
    {
        DetectPlayerLooking();

        if (Input.GetKeyDown(KeyCode.E) && currentTarget == this && !isTalking)
        {
            StartDialogue();
        }
    }

    /// <summary>
    /// カメラ中心からレイを飛ばして「見ている対象」を取得
    /// </summary>
    void DetectPlayerLooking()
    {
        var player = GameObject.FindWithTag("Player");
        if (player == null) return;

        Camera cam = Camera.main;
        if (cam == null) return;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, talkDistance))
        {
            DialogueTrigger target = hit.collider.GetComponent<DialogueTrigger>();

            if (target != null)
            {
                currentTarget = target;
                return;
            }
        }

        // 何も見ていない場合リセット
        if (currentTarget == this)
            currentTarget = null;
    }

    void StartDialogue()
    {
        isTalking = true;
        DialogueManager.instance.ShowDialogue(dialogueLines, OnDialogueEnd);
        Debug.Log($"Started dialogue with {gameObject.name}");
    }

    void OnDialogueEnd()
    {
        isTalking = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, talkDistance);
    }
}
