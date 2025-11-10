using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 会話対象（オブジェクト）に付けるコンポーネント。
/// - DialogueSelector によって選ばれたときのみ E キーで会話を開始する。
/// - 大きな群れに置いても、カメラ中心に近いものが選ばれる。
/// </summary>
[RequireComponent(typeof(Collider))]
public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue")]
    public List<DialogueLine> dialogueLines = new List<DialogueLine>()
    {
        new DialogueLine("？？？", "……ここはどこだ？"),
        new DialogueLine("？？？", "辺りは静まり返っている……。"),
        new DialogueLine("主人公", "……何かの気配がする。")
    };

    [Header("設定")]
    [Tooltip("会話可能かどうかを一時的に無効化したいときに使う")]
    public bool ignore = false;

    [Tooltip("自分の会話が進行中かどうか（内部フラグ）")]
    public bool isTalking = false;

    // Eキー入力は DialogueSelector.CurrentTarget を使う
    void Update()
    {
        // 選定されたターゲットがこのオブジェクトで、かつ E キーが押されたら会話開始
        if (!ignore && DialogueSelector.CurrentTarget == this && Input.GetKeyDown(KeyCode.E) && !isTalking)
        {
            StartDialogue();
        }
    }

    public void StartDialogue()
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
        // エディタで見やすくするための表示
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}
