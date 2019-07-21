using UnityEngine;

namespace WizardAdventure.Items
{
    /// <summary>
    /// Mana item class for mana item game-objects laying around in
    /// the game world. NOT LOOT CONTAINERS! These items are like Healing item
    /// base class, but have Rigidbodies and colliders to implement IWorldItem.
    /// </summary>
    public abstract class WorldManaItem : WorldItem
    {
    }
}