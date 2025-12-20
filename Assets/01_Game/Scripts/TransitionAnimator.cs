using EasyTransition;
using UnityEngine;

public class TransitionAnimator : MonoBehaviour
{
    // ---------- Field
    [SerializeField] private TransitionManager _transitionManager;
    [SerializeField] private TransitionSettings _transitionSettings;

    // ---------- Method
    public void StartTransitionAnim()
    {
        _transitionManager.TransitionAnimOnly(_transitionSettings);
    }
}
