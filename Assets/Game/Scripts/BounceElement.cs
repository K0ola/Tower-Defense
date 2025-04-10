using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BounceElement : MonoBehaviour
{
    [Header("Bounce Settings")]
    [SerializeField] private float bigBounceHeight = 30f;
    [SerializeField] private float bigBounceSpeed = 0.1f;
    [SerializeField] private float smallBounceHeight = 10f;
    [SerializeField] private float smallBounceSpeed = 0.25f;

    private RectTransform rectTransform;
    private Vector2 originalPos;
    private Coroutine bounceRoutine;
    private Coroutine loopBounceRoutine;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPos = rectTransform.anchoredPosition;
    }

    public void Bounce()
    {
        if (bounceRoutine != null) StopCoroutine(bounceRoutine);
        bounceRoutine = StartCoroutine(BounceAnimation(bigBounceHeight, bigBounceSpeed));
    }

    public void StartLoopBounce()
    {
        if (loopBounceRoutine != null) return;
        loopBounceRoutine = StartCoroutine(LoopBounce());
    }

    public void StopLoopBounce()
    {
        if (loopBounceRoutine != null)
        {
            StopCoroutine(loopBounceRoutine);
            loopBounceRoutine = null;
            rectTransform.anchoredPosition = originalPos;
        }
    }

    IEnumerator BounceAnimation(float height, float speed)
    {
        Vector2 target = originalPos + Vector2.up * height;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / speed;
            rectTransform.anchoredPosition = Vector2.Lerp(originalPos, target, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / speed;
            rectTransform.anchoredPosition = Vector2.Lerp(target, originalPos, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }

        bounceRoutine = null;
    }

    IEnumerator LoopBounce()
    {
        while (true)
        {
            yield return BounceAnimation(smallBounceHeight, smallBounceSpeed);
            yield return new WaitForSeconds(0.25f); // Pause entre deux petits bonds
        }
    }
}
