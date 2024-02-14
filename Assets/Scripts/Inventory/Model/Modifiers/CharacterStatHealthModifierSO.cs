using UnityEngine;

[CreateAssetMenu]
public class CharacterStatHealthModifierSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject character, float val)
    {
        PlayerHealthController _healthController = character.GetComponent<PlayerHealthController>();

        if (_healthController != null)
            _healthController.AddHealth(val);
    }
}