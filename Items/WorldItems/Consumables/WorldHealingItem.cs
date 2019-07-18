using UnityEngine;
using System.Collections;

namespace WizardAdventure.Items
{
    /// <summary>
    /// Healing Item class for healing item game-objects laying around in
    /// the game world. NOT LOOT CONTAINERS! These items are like Healing item
    /// base class, but have Rigidbodies and colliders to implement IWorldItem.
    /// </summary>
    public abstract class WorldHealingItem : HealingItem, ICollectable
    {
    #region [Properties]
        /// <summary>
        /// Collider for World healing item.
        /// DO NOT change this property!
        /// </summary>
        /// <value>Get and Set Collider for world healing item</value>
        public Collider2D Collider { get; set; }

        /// <summary>
        /// Rigidbody2D for World healing item.
        /// DO NOT change this property!
        /// </summary>
        /// <value>Get and Set Rigidbody2D for world healing item.</value>
        public Rigidbody2D Rigidbody { get; set; }

        public bool FlyFlag { get; set; } = false;

        public Unit Gatherer { get; set; }
    #endregion

    #region [Unity API]

        /// <summary>
        /// Collision eventhandler for healing item.
        /// When healing item collides with player,
        /// it is picked up by the player. Healing item
        /// dissapears from the game world, and reappears
        /// into players inventory.
        /// </summary>
        /// <param name="other">Other game object in collision.</param>
        public void OnCollisionEnter2D(Collision2D other) 
        {
            if (other.gameObject.CompareTag("Player"))
            {
                this.Collect(other.gameObject);
            }
        }

        private IEnumerator GatherMovement()
        {
            float speed = 6f * Time.deltaTime;

            // Idle while item flyes into air
            while (this.Rigidbody.velocity.y >= 0)
            {
                if (this.transform.localScale.x < 1.4f)
                {
                    Vector3 newScale = this.transform.localScale;
                    newScale.x += 0.05f;
                    newScale.y += 0.05f;
                    this.transform.localScale = newScale;
                }
                yield return null;
            }

            // remove gravity and start guideing item towards gatherer.
            this.Rigidbody.gravityScale = 0.0f;

            bool itemIsFar = true;
            while (itemIsFar)
            {
                // Move item towards gatherer.
                this.transform.position = Vector3.MoveTowards(
                    this.transform.position,
                    this.Gatherer.GetCenterPoint<SpriteRenderer>() + new Vector3(0, -0.2f, 0),
                    speed
                );

                // Recalculate items distance to gatherer.
                itemIsFar = Vector3.Distance(this.transform.position,
                    this.Gatherer.GetCenterPoint<SpriteRenderer>() + new Vector3(0, -0.2f, 0)) >= 1f;

                // return control back to unity.
                yield return null;
            }
            while (this.transform.localScale.x > 0)
            {
                // Move Item towards gatherer.
                this.transform.position = Vector3.MoveTowards(
                    this.transform.position,
                    this.Gatherer.GetCenterPoint<SpriteRenderer>() + new Vector3(0, -0.2f, 0),
                    speed
                );

                // Scale item down untill it dissapears.
                Vector3 newScale = this.transform.localScale;
                newScale.x -= 0.08f;
                newScale.y -= 0.08f;
                this.transform.localScale = newScale;

                // return control to unity.
                yield return null;
            }
            Destroy(this.gameObject);
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
        /// Collects this healing item from the game world into
        /// players inventory.
        /// </summary>
        public void Collect(GameObject player)
        {
            this.Gatherer = player.GetComponent<Player>();
            player.GetComponent<Player>()?.Inventory.AddItem(this);
            this.Collider.enabled = false;
            this.Rigidbody.AddForce(new Vector2(10, 300));
            this.StartCoroutine(GatherMovement());

        }

        /// <summary>
        /// Initialize components from IWorldItem.
        /// </summary>
        public void InitializeWorldItem()
        {
            this.Collider = this.GetComponent<Collider2D>();
            this.Rigidbody = this.GetComponent<Rigidbody2D>();
        }

    #endregion

    }
}