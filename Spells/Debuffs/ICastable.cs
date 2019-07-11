namespace WizardAdventure.Spells
{
    public interface ICastable
    {
        void Cast(Unit caster, bool faceRight);
    }
}