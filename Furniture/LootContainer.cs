using UnityEngine;
using WizardAdventure.Effects;
using System.Collections.Generic;
using WizardAdventure.Items;

namespace WizardAdventure.Props
{

    /// <summary>
    /// Container containing loot for the player
    /// to loot :).
    /// </summary>
    public abstract class LootContainer : MonoBehaviour
    {

    #region [Properties]
        /// <summary>
        /// Loot container's light effect controller.
        /// </summary>
        /// <value>Get and set Loot container's light effect controller</value>
        public LightEffects LootLightEffect { get; private set; }

        /// <summary>
        /// 2D Collider component for detecting when player walks
        /// close to container.
        /// </summary>
        /// <value>Get and Set Loot container's 2D collider component.</value>
        public Collider2D LootCollider { get; private set; }

        /// <summary>
        /// Sprite renderer component.
        /// </summary>
        /// <value>Get and Set loot container's sprite renderer.</value>
        public SpriteRenderer Renderer { get; private set; }

        /// <summary>
        /// Loot container's praticle effect that
        /// indicates that container contains loot.
        /// </summary>
        /// <value>Get and Set particle effect component.</value>
        public ParticleSystem LootEffect { get; private set; }

        /// <summary>
        /// List of the loot contained inside
        /// loot container.
        /// </summary>
        /// <value>Get and Set loot list.</value>
        public List<Item> Container { get; private set; }
    #endregion

    #region [Unity API]

        /// <summary>
        /// Trigger collision event for staying inside
        /// collider. When player stays inside the collider,
        /// they can loot the container.
        /// </summary>
        /// <param name="other">Player</param>
        protected virtual void OnTriggerStay2D(Collider2D other) 
        {
            if (Tags.TagsContainTag(other.gameObject.tag, "Player"))
            {
                if(Input.GetKeyDown(KeyCode.E))
                {
                    this.GetComponent<Animator>().SetTrigger("Open");
                    this.LootEffect.gameObject?.SetActive(true);
                }
            }
        }

        /// <summary>
        /// Trigger collision event for leaving collider
        /// borders. When Player leave's collider's borders,
        /// loot container closes.
        /// </summary>
        /// <param name="other">Player</param>
        protected virtual void OnTriggerExit2D(Collider2D other) 
        {
            if (Tags.TagsContainTag(other.gameObject.tag, "Player"))
            {
                this.GetComponent<Animator>().SetTrigger("Close");
                this.LootEffect.gameObject?.SetActive(false);
            }
        }

        /// <summary>
        /// Starts the glow effect signaling treasure.
        /// </summary>
        protected virtual void Start()
        {
            this.LootLightEffect.BeginGlow(4.0f, 0.3f, 0.08f);
        }

        /// <summary>
        /// Initializes components and attributes.
        /// </summary>
        protected virtual void Awake() 
        {
            InitializeLootContainer();
        }

    #endregion

    #region [Protected Methods]

        /// <summary>
        /// Initializes components and attributes.
        /// </summary>
        protected virtual void InitializeLootContainer()
        {
            this.LootEffect = this.transform.Find("TreasureEffect").GetComponent<ParticleSystem>();
            this.LootLightEffect = this.GetComponentInChildren<LightEffects>();
            this.LootCollider = this.GetComponent<Collider2D>();
            this.Renderer = this.GetComponent<SpriteRenderer>();
        }

    #endregion

    }
}

