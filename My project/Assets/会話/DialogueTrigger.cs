using UnityEngine;
using System.Collections.Generic;

public class DialogueTrigger : MonoBehaviour
{
    public bool isTalking = false; // 会話中かどうか

    public List<DialogueLine> dialogueLines = new List<DialogueLine>()
    {
        new DialogueLine("？？？", "……ここはどこだ？"),
        new DialogueLine("？？？", "辺りは静まり返っている……。"),
        new DialogueLine("主人公", "……何かの気配がする。")
    };

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            var player = GameObject.FindWithTag("Player");
            if (player == null) return;

            float distance = Vector3.Distance(player.transform.position, transform.position);

            // 会話可能距離内 & 会話中でないときのみ開始
            if (distance < 3f && !isTalking)
            {
                StartDialogue();
            }
        }
    }

    void StartDialogue()
    {
        isTalking = true;

        // 会話を開始
        DialogueManager.instance.ShowDialogue(dialogueLines, OnDialogueEnd);
    }

    // === 会話終了時に呼ばれる ===
    void OnDialogueEnd()
    {
        isTalking = false;
    }
}
