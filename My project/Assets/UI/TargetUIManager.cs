using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class TargetUIManager : MonoBehaviour
{
    [Header("カメラ設定")]
    [SerializeField] private Camera mainCamera;

    [Header("UI設定")]
    [SerializeField] private Canvas worldCanvas;
    [SerializeField] private Transform uiRoot;
    [SerializeField] private TextMeshProUGUI nameText_TMP;
    [SerializeField] private TextMeshProUGUI hintText_TMP;
    [SerializeField] private Text nameText_Legacy;
    [SerializeField] private Text hintText_Legacy;
    [SerializeField] private float displayDistance = 2f;
    [SerializeField] private Vector3 offset = new Vector3(-0.5f, 1.2f, 0);

    [Header("対象設定")]
    [SerializeField] private List<string> targetTags = new List<string> { "NPC" };

    [Header("他処理中は非表示")]
    public bool isSystemBusy = false;

    private GameObject currentTarget;
    private Transform playerCam;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        playerCam = mainCamera.transform;

        SetVisible(false);
    }

    private void Update()
    {
        if (isSystemBusy)
        {
            SetVisible(false);
            return;
        }

        currentTarget = FindClosestTarget();

        if (currentTarget != null)
        {
            float distance = Vector3.Distance(playerCam.position, currentTarget.transform.position);

            if (distance <= displayDistance)
            {
                UpdateUIPosition();
                SetVisible(true);
                UpdateUIText(currentTarget);
            }
            else
            {
                SetVisible(false);
            }
        }
        else
        {
            SetVisible(false);
        }
    }

    private void UpdateUIText(GameObject target)
    {
        string displayName = target.name;
        string hint = "";

        // TargetInfoコンポーネントがある場合はそちらを優先
        var info = target.GetComponent<TargetInfo>();
        if (info != null)
        {
            displayName = info.displayName;
            hint = info.hintText;
        }

        if (nameText_TMP != null) nameText_TMP.text = displayName;
        if (nameText_Legacy != null) nameText_Legacy.text = displayName;

        if (hintText_TMP != null) hintText_TMP.text = hint;
        if (hintText_Legacy != null) hintText_Legacy.text = hint;
    }

    // 対象の左側にUIを配置
    private void UpdateUIPosition()
    {
        if (uiRoot == null || currentTarget == null || playerCam == null)
            return;

        Vector3 leftOffset = -playerCam.right * offset.x + Vector3.up * offset.y + playerCam.forward * offset.z;
        Vector3 targetPos = currentTarget.transform.position + leftOffset;

        uiRoot.position = targetPos;
        uiRoot.rotation = Quaternion.LookRotation(uiRoot.position - playerCam.position);
    }

    // 指定タグの中で最も近い対象を取得
    private GameObject FindClosestTarget()
    {
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (string tag in targetTags)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
            foreach (var obj in objs)
            {
                float dist = Vector3.Distance(playerCam.position, obj.transform.position);
                Vector3 dir = (obj.transform.position - playerCam.position).normalized;

                if (dist < minDistance && Vector3.Dot(playerCam.forward, dir) > 0.7f)
                {
                    minDistance = dist;
                    closest = obj;
                }
            }
        }
        return closest;
    }

    private void SetVisible(bool visible)
    {
        if (nameText_TMP != null)
            nameText_TMP.gameObject.SetActive(visible);
        if (nameText_Legacy != null)
            nameText_Legacy.gameObject.SetActive(visible);
        if (hintText_TMP != null)
            hintText_TMP.gameObject.SetActive(visible);
        if (hintText_Legacy != null)
            hintText_Legacy.gameObject.SetActive(visible);
    }
}
