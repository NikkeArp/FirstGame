using UnityEngine;
using WizardAdventure.Effects;
using System.Collections.Generic;

namespace WizardAdventure.Props
{
    public class Item : MonoBehaviour
    {
        public string Name { get; private set; }
        public int Id { get; private set; }
    }

    public class LootContainer : MonoBehaviour
    {

    #region [Properties]
        public LightEffects LootLightEffect { get; private set; }
        public Collider2D LootCollider { get; private set; }
        public SpriteRenderer Renderer { get; private set; }
        public ParticleSystem LootEffect { get; private set; }
        public List<Item> Container { get; private set; }
    #endregion

    #region [Unity API]

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        protected virtual void OnTriggerStay2D(Collider2D other) {
            if(Input.GetKeyDown(KeyCode.E))
            {
                this.GetComponent<Animator>().SetTrigger("Open");
                this.LootEffect.gameObject?.SetActive(true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        protected virtual void OnTriggerExit2D(Collider2D other) 
        {
            this.GetComponent<Animator>().SetTrigger("Close");
            this.LootEffect.gameObject?.SetActive(false);
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

        /// <summary>
        /// 
        /// </summary>
        protected virtual void Update()
        {
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

