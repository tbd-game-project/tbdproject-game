using UnityEngine;

public abstract class PlayerState : ScriptableObject
{
    protected Player owner = null;
    protected PlayerInputReader input = null;

    public virtual void EnterState(Player owner, PlayerInputReader input) { if (!this.owner) { this.owner = owner; } if (!this.input) { this.input = input; } }
    public virtual void ExitState() { }
    public virtual void UpdateState() { }
    public virtual void FixedUpdateState() { }
}
