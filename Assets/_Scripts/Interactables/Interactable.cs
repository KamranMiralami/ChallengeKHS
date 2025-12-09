using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected GameObject indicator;
    public abstract void OnInteract();
    public abstract void OnHighlight();
    public abstract void OnUnHighlight();
}
