using UnityEngine;
using System.Collections;

namespace WizardAdventure.Items
{
    /// <summary>
    /// Interface for items that are physically in the gameworld.
    /// </summary>
    public abstract class WorldItem : Item
    {
    #region [Properties]
        /// <summary>
        /// 2D Rigidbody for item in the gameworld.
        /// </summary>
        /// <value>Get and Set world item's rigidbody component</value>
        protected Rigidbody2D Rigidbody { get; set; }

        /// <summary>
        /// 2D collider for item in the gameworld.
        /// </summary>
        /// <value>Get and Set world item's collider component</value>
        protected Collider2D Collider { get; set; }

        /// <summary>
        /// Unit that gathered this item.
        /// </summary>
        /// <value>Get and Set the Unit that gathered this item.</value>
        protected Unit Gatherer { get; set; }
    #endregion
    
    #region [Unity API]

        /// <summary>
        /// Collision eventhandler for collectable item.
        /// When the item collides with player,
        /// it is picked up by. Item then dissapears from the
        /// game world, and reappears into players inventory.
        /// </summary>
        /// <param name="other">Other gameobject in collision</param>
        protected virtual void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                this.Collect(other.gameObject);
            }
        }
    #endregion


    #region [Protected Methods]

        /// <summary>
        /// Throws item into air, upscaling it.
        /// When item starts to fall down, disables gravity and starts
        /// guideing it towards the unit that collected it. When item gets
        /// close enough, it starts to shrink.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator GatherMovement()
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

        /// <summary>
        /// Collects this mana item from the gameworld
        /// into players inventory.
        /// </summary>
        protected void Collect(GameObject player)
        {
            player.GetComponent<Player>()?.Inventory.AddItem(this);
            Destroy(this.gameObject);
        }

        /// <summary>
        /// Overrides Item base classes initialization
        /// by also setting physics components needed for item located
        /// in the gameworld.
        /// </summary>
        protected override void InitializeItem()
        {
            base.InitializeItem();
            this.Rigidbody = this.GetComponent<Rigidbody2D>();
            this.Collider = this.GetComponent<Collider2D>();
        }

    #endregion

    }
}