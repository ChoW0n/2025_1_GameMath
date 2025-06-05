using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ShotLineGuide : MonoBehaviour
{
    [SerializeField] LayerMask tableLayer;
    [SerializeField] Material lineMat;
    [SerializeField] float maxLen = 5f;

    LineRenderer lr;
    Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        lr = new GameObject("GuideLine").AddComponent<LineRenderer>();
        lr.transform.parent = null;
        lr.positionCount = 2;
        lr.widthMultiplier = 0.04f;
        lr.material = lineMat != null
            ? lineMat
            : new Material(Shader.Find("Unlit/Color")) { color = Color.yellow };
        lr.enabled = false;
    }

    void Update()
    {
        var gm = BilliardGameManager.Instance;
        if (gm == null || gm.HasShot) { lr.enabled = false; return; }

        Rigidbody cue = gm.CurrentCue;
        if (cue == null) { lr.enabled = false; return; }

        // 마우스 히트포인트 계산
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, 100f, tableLayer))
        {
            lr.enabled = false; return;
        }

        Vector3 dir = (hit.point - cue.position);
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.01f) { lr.enabled = false; return; }

        lr.enabled = true;
        lr.SetPosition(0, cue.position + Vector3.up * 0.02f);
        lr.SetPosition(1, cue.position + dir.normalized * maxLen + Vector3.up * 0.02f);
    }
}
