// StatusEffectInstance — runtime state for one active status on a unit.
// References a StatusEffectSO for definition data.
// Managed by StatusEffectManager.

public class StatusEffectInstance
{
    public StatusEffectSO definition;
    public int            remainingDuration;
    public int            currentStacks;
    public RuntimeCharacterState source; // who applied it (null = environment)

    public StatusEffectInstance(
        StatusEffectSO def,
        int duration,
        RuntimeCharacterState src = null)
    {
        definition        = def;
        remainingDuration = duration;
        currentStacks     = 1;
        source            = src;
    }

    /// <summary>Tick one turn. Returns true if the status expired.</summary>
    public bool Tick()
    {
        if (definition.durationType == StatusEffectSO.DurationType.Turns)
        {
            remainingDuration--;
            return remainingDuration <= 0;
        }
        // Room / NextPressure / NextAction durations are decremented by
        // StatusEffectManager when the relevant event fires, not here.
        return false;
    }
}
