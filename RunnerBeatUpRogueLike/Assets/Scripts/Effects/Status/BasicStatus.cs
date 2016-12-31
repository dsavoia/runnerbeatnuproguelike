using System;

class BasicStatus : IStatusEffect
{

    protected float power = 0f;
    protected StatusEffectsTypes statusType;

    public float GetPower()
    {
        return power;
    }

    public StatusEffectsTypes GetStatusEffectType()
    {
        return statusType;
    }

    public void SetPower(float power)
    {
        this.power = power;
    }

    public void SetStatusEffectType(StatusEffectsTypes status)
    {
        this.statusType = status;
    }
}