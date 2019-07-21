using UnityEngine;

namespace WizardAdventure.Items
{
    /// <summary>
    /// Game Item for player to gather and use.
    /// </summary>
    public abstract class Item : MonoBehaviour
    {
        #region [Properties]
        /// <summary>
        /// Name of the Item.
        /// </summary>
        /// <value>Get and Set the name of the Item.</value>
        public string Name { get; protected set; }

        /// <lore>
        /// Short description of the item's lore.
        /// </lore>
        /// <value>Get and Set the item's summary.</value>
        public string Lore { get; protected set; }

        /// <summary>
        /// Item description.
        /// </summary>
        /// <value>Get and Set item's description.</value>
        public string Description { get; protected set; }

        /// <summary>
        /// Item ID.
        /// </summary>
        /// <value>Get and Set item's ID</value>
        public string Id { get; protected set; }

        public Sprite icon;
        #endregion

    #region [Unity API]
        
        /// <summary>
        /// 
        /// </summary>
        protected void Awake()
        {
            this.InitializeItem();
        }

    #endregion

    #region [Public Methods]
        public override string ToString()
        {
            return this.Name;
        }

    #endregion

    #region [Protected Methods]
        
        protected virtual void InitializeItem()
        {

        }

    #endregion

    }
}