namespace WizardAdventure.Spells
{
    /// <summary>
    /// Castable spells
    /// </summary>
    public interface ICastable
    {
        void Cast(Unit caster, bool faceRight);
    }
}