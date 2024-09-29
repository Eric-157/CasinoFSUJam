using UnityEngine;

[CreateAssetMenu]
public class PlayerInputs : ScriptableObject
{
    public Player.Action Action { get; set; }

    public void Hit() { Action = Player.Action.Hit; }
    public void Stand() { Action = Player.Action.Stand; }
}
