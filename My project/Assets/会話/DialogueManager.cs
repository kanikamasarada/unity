using TMPro;
using UnityEngine;
using UnityEngine.UI; // ← Legacy Text用
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("UI (どちらかを使う)")]
    public GameObject dialoguePanel;

    // TMP
    public TextMeshProUGUI nameTextTMP;
    public TextMeshProUGUI dialogueTextTMP;

    // Legacy
    public Text nameTextLegacy;
    public Text dialogueTextLegacy;

    [Header("設定")]
    public float typeSpeed = 0.06f;
    public float autoCloseDelay = 3f;

    private bool isShowing = false;
    [System.NonSerialized] private bool isTyping;

    private Queue<DialogueLine> dialogueQueue = new Queue<DialogueLine>();

    // ★追加：会話終了時に呼ばれるコールバック
    private System.Action onDialogueEnd;

    void Awake()
    {
        instance = this;
        if (dialoguePanel != null)
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

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        ClearText();
        StartCoroutine(RunDialogue());
    }

    private IEnumerator RunDialogue()
    {
        isShowing = true;
        FreezePlayer(true);

        while (dialogueQueue.Count > 0)
        {
            DialogueLine line = dialogueQueue.Dequeue();

            SetNameText(line.speakerName);
            yield return StartCoroutine(TypeText(line.text));

            // Enterキー待ち
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        }

        yield return new WaitForSeconds(autoCloseDelay);
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        ClearText();
        isShowing = false;
        FreezePlayer(false);

        onDialogueEnd?.Invoke();
        onDialogueEnd = null;
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        SetDialogueText("");

        foreach (char c in text)
        {
            AppendDialogueText(c.ToString());
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

    // ======== ここから共通処理 ========

    private void SetNameText(string text)
    {
        if (nameTextTMP != null)
            nameTextTMP.text = text;
        if (nameTextLegacy != null)
            nameTextLegacy.text = text;
    }

    private void SetDialogueText(string text)
    {
        if (dialogueTextTMP != null)
            dialogueTextTMP.text = text;
        if (dialogueTextLegacy != null)
            dialogueTextLegacy.text = text;
    }

    private void AppendDialogueText(string text)
    {
        if (dialogueTextTMP != null)
            dialogueTextTMP.text += text;
        if (dialogueTextLegacy != null)
            dialogueTextLegacy.text += text;
    }

    private void ClearText()
    {
        SetNameText("");
        SetDialogueText("");
    }
}

// ===========================================
// 会話1行のデータ構造
// ===========================================
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
