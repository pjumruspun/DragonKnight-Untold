[System.Serializable]
public struct ItemStats
{
    public int AtkAddModifier;
    public int AgiAddModifier;
    public int VitAddModifier;
    public int TalAddModifier;
    public int LukAddModifier;
    public float AtkMultModifier;
    public float AgiMultModifier;
    public float VitMultModifier;
    public float TalMultModifier;
    public float LukMultModifier;

    // public ItemStats() { }

    public ItemStats(
        int atkAddModifier,
        int agiAddModifier,
        int vitAddModifier,
        int talAddModifier,
        int lukAddModifier,
        float atkMultModifier = 0.0f,
        float agiMultModifier = 0.0f,
        float vitMultModifier = 0.0f,
        float talMultModifier = 0.0f,
        float lukMultModifier = 0.0f
    )
    {
        this.AtkAddModifier = atkAddModifier;
        this.AgiAddModifier = agiAddModifier;
        this.VitAddModifier = vitAddModifier;
        this.TalAddModifier = talAddModifier;
        this.LukAddModifier = lukAddModifier;
        this.AtkMultModifier = atkMultModifier;
        this.AgiMultModifier = agiMultModifier;
        this.VitMultModifier = vitMultModifier;
        this.TalMultModifier = talMultModifier;
        this.LukMultModifier = lukMultModifier;
    }

    public static ItemStats operator +(ItemStats a) => a;

    public static ItemStats operator -(ItemStats a)
    {
        return new ItemStats(
            -a.AtkAddModifier,
            -a.AgiAddModifier,
            -a.VitAddModifier,
            -a.TalAddModifier,
            -a.LukAddModifier,
            -a.AtkMultModifier,
            -a.AgiMultModifier,
            -a.VitMultModifier,
            -a.TalMultModifier,
            -a.LukMultModifier
        );
    }

    public static ItemStats operator +(ItemStats a, ItemStats b)
    {
        return new ItemStats(
            a.AtkAddModifier + b.AtkAddModifier,
            a.AgiAddModifier + b.AgiAddModifier,
            a.VitAddModifier + b.VitAddModifier,
            a.TalAddModifier + b.TalAddModifier,
            a.LukAddModifier + b.LukAddModifier,
            a.AtkMultModifier + b.AtkMultModifier,
            a.AgiMultModifier + b.AgiMultModifier,
            a.VitMultModifier + b.VitMultModifier,
            a.TalMultModifier + b.TalMultModifier,
            a.LukMultModifier + b.LukMultModifier
        );
    }

    public static ItemStats operator -(ItemStats a, ItemStats b)
    {
        return new ItemStats(
            a.AtkAddModifier - b.AtkAddModifier,
            a.AgiAddModifier - b.AgiAddModifier,
            a.VitAddModifier - b.VitAddModifier,
            a.TalAddModifier - b.TalAddModifier,
            a.LukAddModifier - b.LukAddModifier,
            a.AtkMultModifier - b.AtkMultModifier,
            a.AgiMultModifier - b.AgiMultModifier,
            a.VitMultModifier - b.VitMultModifier,
            a.TalMultModifier - b.TalMultModifier,
            a.LukMultModifier - b.LukMultModifier
        );
    }

    public override string ToString()
    {
        return "(" +
        AtkAddModifier + ", " +
        AgiAddModifier + ", " +
        VitAddModifier + ", " +
        TalAddModifier + ", " +
        LukAddModifier +
        "), (" +
        AtkMultModifier + ", " +
        AgiMultModifier + ", " +
        VitMultModifier + ", " +
        TalMultModifier + ", " +
        LukMultModifier + ")";
    }
}
