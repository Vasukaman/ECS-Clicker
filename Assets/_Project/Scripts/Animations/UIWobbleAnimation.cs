// Filename: UIWobbleAnimation.cs
// Location: _Project/Scripts/Views/ (or /Animations/)
using System.Collections;
using UnityEngine;

public class UIWobbleAnimation : MonoBehaviour
{
    [SerializeField] private UIWobbleAnimationSettingsSO _animationSettings;

    private Vector3 _originalScale;
    private Vector3 _currentVelocity = Vector3.zero;
    private Coroutine _currentWobble;

    private void Awake()
    {
        _originalScale = transform.localScale;
    }

    /// <summary>
    /// This is the public method you will call from the Button's onClick event.
    /// </summary>
    public void PlayWobble()
    {
        if (_currentWobble != null)
        {
            StopCoroutine(_currentWobble);
        }
        _currentWobble = StartCoroutine(WobbleCoroutine());
    }

    private IEnumerator WobbleCoroutine()
    {
        // Give the animation an initial "punch" downwards in scale
        _currentVelocity = Vector3.one * _animationSettings.InitialPunch;

        // This loop runs until the animation comes to a rest
        while (!IsStopped())

        {   // Clamping delta time to prevent explosions on lag spikes.
            float deltaTime = Mathf.Min(Time.deltaTime, 0.1f);
            // Spring physics calculation
            Vector3 force = (_originalScale - transform.localScale) * _animationSettings.Springiness;
            _currentVelocity = (_currentVelocity + force * Time.deltaTime) * (1f - _animationSettings.Damping * Time.deltaTime);
            transform.localScale += _currentVelocity * Time.deltaTime;

            yield return null;
        }

        // Ensure it ends exactly on the original scale
        transform.localScale = _originalScale;
        _currentWobble = null;
    }

    // A check to see if the animation is basically finished
    private bool IsStopped()
    {
        return _currentVelocity.sqrMagnitude < 0.001f &&
               Vector3.SqrMagnitude(_originalScale - transform.localScale) < 0.001f;
    }
}