namespace WizardAdventure.Spells
{
    public abstract class DamageSpell : Spell, IDamageSource
    {
        new public static float BaseCooldown { get; private set; }
        protected float damage;
        public float GetDamage()
        {
            return this.damage;
        }
    }
}