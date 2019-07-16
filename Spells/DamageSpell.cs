namespace WizardAdventure.Spells
{
    public abstract class DamageSpell : Spell, IDamageSource
    {
        protected float damage;
        public float GetDamage()
        {
            return this.damage;
        }
    }
}