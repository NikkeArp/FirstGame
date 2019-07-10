using UnityEngine;

namespace WizardAdventure.Projectiles
{
    public class Projectile : MonoBehaviour
    {
    #region [Properties]
        protected Rigidbody2D Rigidbody;
        protected Collider2D Collider;
        [SerializeField] public int damage;
    #endregion

    #region [Unity API]

        /// <summary>
        /// Initializes projectile components when object is created.
        /// </summary>
        protected virtual void Awake()
        {
            InitilizeProjectile();
        }

        /// <summary>
        /// Checks if projectile has travelled too far.
        /// Destroys the game object after treshold.
        /// </summary>
        protected virtual void Update()
        {
            if (transform.position.magnitude > 100.0f)
            {
                Destroy(gameObject);
            }
        }
    #endregion

    #region [Public Methods]

        /// <summary>
        /// Launches projectile towards given direction with given force.
        /// </summary>
        /// <param name="direction">Direction in vector object</param>
        /// <param name="force">Force</param>
        public virtual void Launch(Vector2 direction, float force) 
        {
            this.Rigidbody.AddForce(direction * force);
        }

    #endregion

    #region [Protected Methods]

        /// <summary>
        /// Intilizes physics components.
        /// </summary>
        protected virtual void InitilizeProjectile()
        {
            this.Rigidbody = this.GetComponent<Rigidbody2D>();
            this.Collider = this.GetComponent<Collider2D>();
        }

    #endregion
    
    }
}

