namespace WizardAdventure.Items
{
    /// <summary>
    /// Healing Item class for healing item game-objects laying around in
    /// the game world. NOT LOOT CONTAINERS! These items are like Healing item
    /// base class, but have Rigidbodies and colliders to implement IWorldItem.
    /// </summary>
    public abstract class WorldBuffItem : WorldItem
    {
    }
}