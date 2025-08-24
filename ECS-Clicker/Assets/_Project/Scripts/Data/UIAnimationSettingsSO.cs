using UnityEngine;

[CreateAssetMenu(fileName = "UIAnimationSettings", menuName = "UI/Wobble Animation Settings")]
public class UIWobbleAnimationSettingsSO : ScriptableObject
{
    [Tooltip("How strongly the scale pulls back to its original size.")]
    public float Springiness = 150f;

    [Tooltip("How quickly the wobble effect slows down. Lower values are bouncier.")]
    public float Damping = 0.6f;

    [Tooltip("The initial 'punch' given to the scale on click.")]
    public float InitialPunch = -15f;
}