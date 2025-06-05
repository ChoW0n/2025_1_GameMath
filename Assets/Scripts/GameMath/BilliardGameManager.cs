using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public enum BallOwner { None, Player1, Player2 }

public class BilliardGameManager : MonoBehaviour
{

    public static BilliardGameManager Instance { get; private set; }
    void Awake() => Instance = this;

    [Header("큐볼")]
    [SerializeField] Rigidbody cueBall1;          // Player1 (빨간)
    [SerializeField] Rigidbody cueBall2;          // Player2 (파란)

    [Header("샷 파라미터")]
    [SerializeField] float maxPower = 8f;       // 충전 100% 시 힘
    [SerializeField] float chargeTime = 1f;       // 100% 까지 걸리는 시간
    [SerializeField] LayerMask tableLayer;        // Plane 레이어

    [Header("정지 판정")]
    [SerializeField] float stillSpeed = 0.05f;
    [SerializeField] float stillTimeNeeded = 1f;

    public BallOwner CurrentTurn { get; private set; } = BallOwner.Player1;
    public Rigidbody CurrentCue => CurrentTurn == BallOwner.Player1 ? cueBall1 : cueBall2;
    public bool HasShot => shotDone;
    public int P1Score => p1Score;
    public int P2Score => p2Score;

    float holdTimer, stillTimer;
    bool shotDone;

    int p1Score, p2Score;

    readonly List<Rigidbody> allRBs = new();  // 정지 판정용
    readonly List<BilliardBall> targetBalls = new();  // 두 개 목적구
    readonly HashSet<BilliardBall> objectsHit = new();
    bool opponentCueHit;

    Camera cam;


    void Start()
    {
        cam = GetComponent<Camera>();
        opponentCueHit = false;

        foreach (var rb in FindObjectsOfType<Rigidbody>())
        {
            if (rb.gameObject.layer == LayerMask.NameToLayer("Ball"))
                allRBs.Add(rb);

            var bb = rb.GetComponent<BilliardBall>();
            if (bb && !bb.isCue)            // 목적구(큐볼 제외)
                targetBalls.Add(bb);
        }

        Debug.Log("게임 시작 – Player 1 턴");
    }


    void Update()
    {
        HandleInput();
        CheckTurnEnd();
    }

    /* ────────── 입력 & 발사 ────────── */
    void HandleInput()
    {
        var cue = CurrentCue;
        if (cue == null) return;

        // 충전
        if (!shotDone && Input.GetMouseButton(0))
            holdTimer += Time.deltaTime;

        // 발사
        if (!shotDone && Input.GetMouseButtonUp(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, tableLayer))
            {
                Vector3 dir = hit.point - cue.position; dir.y = 0f;
                if (dir.sqrMagnitude > 0.0001f)
                {
                    float power = Mathf.Clamp01(holdTimer / chargeTime) * maxPower;
                    cue.AddForce(dir.normalized * power, ForceMode.Impulse);

                    // 턴 시작 스냅
                    shotDone = true;
                    objectsHit.Clear();
                    opponentCueHit = false;
                }
            }
            holdTimer = 0f;
        }
    }

    public void RegisterCollision(BilliardBall other)
    {
        if (!shotDone) return; // 샷 전에 충돌한 것은 무시

        if (other.isCue && other.owner != CurrentTurn)
        {
            opponentCueHit = true;          // 상대 큐볼 맞춤 득점 불가
            return;
        }

        if (!other.isCue)                  // 목적구라면 기록
            objectsHit.Add(other);
    }

    void CheckTurnEnd()
    {
        if (!shotDone) return;

        bool allStill = true;
        foreach (var rb in allRBs)
        {
            if (rb.velocity.sqrMagnitude > stillSpeed * stillSpeed)
            {
                allStill = false;
                break;
            }
        }

        stillTimer = allStill ? stillTimer + Time.deltaTime : 0f;
        if (stillTimer >= stillTimeNeeded)
            EndTurn();
    }

    void EndTurn()
    {
        bool allTargetsHit = objectsHit.Count == targetBalls.Count;
        Debug.Log($"오브젝트 힛:{objectsHit.Count}  /  총 목적구:{targetBalls.Count}");

        if (allTargetsHit && !opponentCueHit)
        {
            if (CurrentTurn == BallOwner.Player1) ++p1Score;
            else ++p2Score;

            Debug.Log($"득점!   P1:{p1Score}  /  P2:{p2Score}");
        }

        // 턴 전환
        shotDone = false;
        opponentCueHit = false;
        stillTimer = 0f;
        CurrentTurn = CurrentTurn == BallOwner.Player1 ? BallOwner.Player2 : BallOwner.Player1;
        Debug.Log($"턴 전환 {CurrentTurn}");
    }
}