using UnityEngine;

namespace WizardAdventure.Items
{
    /// <summary>
    /// Mana item class for mana item game-objects laying around in
    /// the game world. NOT LOOT CONTAINERS! These items are like Healing item
    /// base class, but have Rigidbodies and colliders to implement IWorldItem.
    /// </summary>
    public class WorldManaItem : ManaItem, ICollectable
    {
    #region [Properties]
        /// <summary>
        /// Collider for World mana item.
        /// DO NOT change this property!
        /// </summary>
        /// <value>Get and Set Collider for world mana item</value>
        public Collider2D Collider { get; set; }

        /// <summary>
        /// Rigidbody2D for World healing item. 
        /// DO NOT change this property!
        /// </summary>
        /// <value>Get and Set Rigidbody2D for world mana item.</value>
        public Rigidbody2D Rigidbody { get; set; }
    #endregion

    #region [Unity API]

        /// <summary>
        /// Collision eventhandler for mana item.
        /// When healing item collides with player,
        /// it is picked up by the player. Mana item
        /// dissapears from the game world, and reappears
        /// into players inventory.
        /// </summary>
        /// <param name="other">Other gameobject in collision</param>
        public void OnCollisionEnter2D(Collision2D other) 
        {
            if (other.gameObject.CompareTag("Player"))
            {
                this.Collect(other.gameObject);
            }
        }

    #endregion

    #region [Protected Methods]

        /// <summary>
        /// Overrides initialization from Item base class.
        /// Also initializes components from IWorldItem.
        /// </summary>
        protected override void InitializeItem()
        {
            base.InitializeItem();
            this.InitializeWorldItem();
        }

    #endregion

    #region [Public Methods]

        /// <summary>
        /// Collects this mana item from the gameworld
        /// into players inventory.
        /// </summary>
        public void Collect(GameObject player)
        {
            player.GetComponent<Player>()?.Inventory.AddItem(this);
            Destroy(this.gameObject);
        }

        /// <summary>
        /// Initialize components from IWorldItem.
        /// This method should be called in objects Awake() method.
        /// </summary>
        public void InitializeWorldItem()
        {
            this.Collider = this.GetComponent<Collider2D>();
            this.Rigidbody = this.GetComponent<Rigidbody2D>();
        }

    #endregion
        
    }
}