using System.Collections;
using UnityEngine;

public class GameFeelManager : MonoBehaviour
{
    [SerializeField] private CameraFollower cameraFollower;
    [SerializeField] private float punchScaleMultiplier = 1.35f;
    [SerializeField] private float punchDuration = 0.12f;
    [SerializeField] private float shakeStrength = 0.08f;
    [SerializeField] private float shakeDuration = 0.12f;

    private Coroutine _scalePunchRoutine;
    private Coroutine _cameraShakeRoutine;

    private void Awake()
    {
        GameEvents.OnLanding += HandleLanding;
    }

    private void OnDestroy()
    {
        GameEvents.OnLanding -= HandleLanding;

        if (cameraFollower != null)
        {
            cameraFollower.ShakeOffset = Vector3.zero;
        }
    }

    private void HandleLanding()
    {
        Transform playerTransform = PlayerRegistry.Player;
        if (playerTransform == null)
        {
            return;
        }

        if (_scalePunchRoutine == null)
        {
            _scalePunchRoutine = StartCoroutine(ScalePunch(playerTransform));
        }

        EnsureCameraFollower();
        if (cameraFollower != null && _cameraShakeRoutine == null)
        {
            _cameraShakeRoutine = StartCoroutine(CameraShake());
        }
    }

    private void EnsureCameraFollower()
    {
        if (cameraFollower != null)
        {
            return;
        }

        cameraFollower = FindFirstObjectByType<CameraFollower>();
    }

    private IEnumerator ScalePunch(Transform playerTransform)
    {
        Vector3 baseScale = playerTransform.localScale;
        Vector3 punchScale = baseScale * punchScaleMultiplier;

        yield return AnimateScale(playerTransform, baseScale, punchScale, punchDuration * 0.5f);
        yield return AnimateScale(playerTransform, punchScale, baseScale, punchDuration * 0.5f);

        _scalePunchRoutine = null;
    }

    private static IEnumerator AnimateScale(Transform target, Vector3 from, Vector3 to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = duration <= 0f ? 1f : elapsed / duration;
            target.localScale = Vector3.Lerp(from, to, t);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        target.localScale = to;
    }

    private IEnumerator CameraShake()
    {
        if (cameraFollower == null)
        {
            _cameraShakeRoutine = null;
            yield break;
        }

        Vector3 startOffset = new Vector3(Random.Range(-shakeStrength, shakeStrength), Random.Range(-shakeStrength, shakeStrength), 0f);
        cameraFollower.ShakeOffset = startOffset;

        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float t = shakeDuration <= 0f ? 1f : elapsed / shakeDuration;
            cameraFollower.ShakeOffset = Vector3.Lerp(startOffset, Vector3.zero, t);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        cameraFollower.ShakeOffset = Vector3.zero;
        _cameraShakeRoutine = null;
    }
}
