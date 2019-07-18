using UnityEngine;

namespace WizardAdventure.Items
{
    /// <summary>
    /// Interface for items that are physically in the gameworld.
    /// Gameobjects that implement IWorldItem must have
    /// Rigidbody2D and Collider2D components and method to
    /// initialize them.
    /// </summary>
    public interface IWorldItem
    {
        #region [Properties]
        /// <summary>
        /// 2D Rigidbody for item in the gameworld.
        /// </summary>
        /// <value>Get and Set world item's rigidbody component</value>
        Rigidbody2D Rigidbody { get; set; }

        /// <summary>
        /// 2D collider for item in the gameworld.
        /// </summary>
        /// <value>Get and Set world item's collider component</value>
        Collider2D Collider { get; set; }
        #endregion

        /// <summary>
        /// Set collider and rigidbody objects.
        /// This method should be called in game objects Awake() method.
        /// </summary>
        void InitializeWorldItem();
    }
}