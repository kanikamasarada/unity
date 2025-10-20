using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    [Header("設定")]
    public float typeSpeed = 0.06f;
    public float autoCloseDelay = 3f;

    private bool isShowing = false;
#pragma warning disable CS0414
    private bool isTyping = false;
#pragma warning restore CS0414

    private Queue<DialogueLine> dialogueQueue = new Queue<DialogueLine>();

    // ★追加：会話終了時に呼ばれるコールバック
    private System.Action onDialogueEnd;

    void Awake()
    {
        instance = this;
        dialoguePanel.SetActive(false);
    }

    // === 通常の呼び出し ===
    public void ShowDialogue(IEnumerable<DialogueLine> lines)
    {
        ShowDialogue(lines, null);
    }

    // === ★コールバック対応版 ===
    public void ShowDialogue(IEnumerable<DialogueLine> lines, System.Action onEnd)
    {
        if (isShowing)
        {
            StopAllCoroutines();
            dialogueQueue.Clear();
        }

        foreach (var line in lines)
        {
            dialogueQueue.Enqueue(line);
        }

        onDialogueEnd = onEnd; // コールバック登録

        dialoguePanel.SetActive(true);
        dialogueText.text = "";
        nameText.text = "";
        StartCoroutine(RunDialogue());
    }

    private IEnumerator RunDialogue()
    {
        isShowing = true;
        FreezePlayer(true);

        while (dialogueQueue.Count > 0)
        {
            DialogueLine line = dialogueQueue.Dequeue();

            nameText.text = line.speakerName;
            yield return StartCoroutine(TypeText(line.text));

            // Enterキー待ち
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        }

        yield return new WaitForSeconds(autoCloseDelay);
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        nameText.text = "";
        isShowing = false;
        FreezePlayer(false);

        // ★追加：会話終了後にコールバックを実行
        onDialogueEnd?.Invoke();
        onDialogueEnd = null;
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
    }

    private void FreezePlayer(bool freeze)
    {
        var playerMove = FindFirstObjectByType<PlayerMovement>();
        var mouseLook = FindFirstObjectByType<MouseLook>();

        if (playerMove != null) playerMove.enabled = !freeze;
        if (mouseLook != null) mouseLook.enabled = !freeze;

        Cursor.lockState = freeze ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = freeze;
    }
}

[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea(2, 5)]
    public string text;

    public DialogueLine(string name, string text)
    {
        this.speakerName = name;
        this.text = text;
    }
}
