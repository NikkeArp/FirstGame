using UnityEngine;

namespace WizardAdventure.Items
{
    /// <summary>
    /// Gameobjects that implement ICollectable can be
    /// picked up into players inventory. Also implements IWorldItem
    /// wich makes sure that items have Rigidbody2D and Collider2D
    /// components.
    /// </summary>
    public interface ICollectable : IWorldItem
    {
        /// <summary>
        /// Collision eventhandler for collectable item.
        /// When the item collides with player,
        /// it is picked up by. Item then dissapears from the
        /// game world, and reappears into players inventory.
        /// </summary>
        /// <param name="other">Other gameobject in collision</param>
        void OnCollisionEnter2D(Collision2D other);

        /// <summary>
        /// Collects the item from game world.
        /// </summary>
        void Collect(GameObject collector);
    }
}