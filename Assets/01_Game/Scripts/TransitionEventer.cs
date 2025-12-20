using EasyTransition;
using UnityEngine;
using R3;

public class TransitionEventer : MonoBehaviour
{
    // ---------- SerializeField
    [SerializeField] private TransitionManager _transitionManager;

    // ---------- Property
    public bool IsRunning => _transitionManager.IsRunning;

    // ---------- R3
    public Observable<Unit> OnTransitionStarted => _transitionManager.OnTransitionStarted;
    public Observable<Unit> OnTransitionHalf => _transitionManager.OnTransitionHalf;
    public Observable<Unit> OnTransitionCompleted => _transitionManager.OnTransitionCompleted;
}
