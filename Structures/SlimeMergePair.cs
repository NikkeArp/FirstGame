namespace WizardAdventure.Structures
{
    /// <summary>
    /// Custom datastructure for Slime-object pairs.
    /// When slimes collide they can merge. During collision event, both slimes
    /// Invoke merge-function in static GameController class and pass this object as 
    /// a parameter.
    /// </summary>
    public class SlimeMergePair 
    {
        public Slime slime1, slime2;

        /// <summary>
        /// Default constructor.
        /// Creates an instance of this class.
        /// Both slimes are null by default.
        /// </summary>
        public SlimeMergePair()
        {
            slime1 = null;
            slime2 = null;
        }


        /// <summary>
        /// Checks if both slime attributes are null.
        /// </summary>
        /// <returns>True if both slimes are null</returns>
        public bool IsEmpty()
        {
            if (slime1 == null && slime2 == null)
                return true;
            return false;
        }

        /// <summary>
        /// Creates an instance of this class.
        /// Self-Slime parameter is stored in slime1-attribute,
        /// other-Slime parameter is strored in slime2-attribute.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        public SlimeMergePair(Slime self, Slime other)
        {
            this.slime1 = self;
            this.slime2 = other;
        }
    }
}