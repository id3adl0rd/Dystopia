using UnityEngine;
using UnityEngine.Events;

public class AnimationEventHelper : MonoBehaviour
{
    public UnityEvent OnAnimationEventTriggered, OnAttackPerfomed;

    public void TriggerEvent()
    {
        OnAnimationEventTriggered?.Invoke();
    }

    public void TriggerAttack()
    {
        OnAttackPerfomed?.Invoke();
    }
}