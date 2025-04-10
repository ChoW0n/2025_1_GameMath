using UnityEngine;

public class HitEffect : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;   // 인스펙터에서 할당
    public float flashDuration = 0.1f;      // 점멸 시간
    public int flashCount = 3;              // 점멸 횟수
    public float shakeAmount = 0.1f;        // 흔들림 강도
    public float shakeDuration = 0.3f;      // 흔들림 시간

    Vector3 originalPosition;

    void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        originalPosition = transform.localPosition;
    }

    public void PlayHitEffect()
    {
        StopAllCoroutines();  // 여러번 맞았을 때 겹치지 않도록
        StartCoroutine(Flash());
        StartCoroutine(Shake());
    }

    System.Collections.IEnumerator Flash()
    {
        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(flashDuration);
        }
    }

    System.Collections.IEnumerator Shake()
    {
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            Vector3 offset = Random.insideUnitCircle * shakeAmount;
            transform.localPosition = originalPosition + offset;
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPosition;
    }
}