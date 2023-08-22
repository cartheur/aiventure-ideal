using Cartheur.Ideal.Mooc.Interfaces;

namespace Cartheur.Ideal.Mooc.Coupling
{
    /// <summary>
    /// An interaction is the association of an experience with a result.
    /// </summary>
    public class Interaction : IParticipant
    {
        private string label;
        protected Experiment experience;
        protected Result result;
        private int valence;
        /// <summary>
        /// Initializes a new instance of the <see cref="Interaction"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        public Interaction(string label)
        {
            this.label = label;
        }
        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <returns>
        /// The interaction's label
        /// </returns>
        public string GetLabel()
        {
            return label;
        }
        /// <summary>
        /// Gets the experience from the <see cref="Experiment"/>.
        /// </summary>
        /// <returns>
        /// The interaction's experience
        /// </returns>
        public Experiment GetExperience()
        {
            return experience;
        }
        /// <summary>
        /// Sets the experience.
        /// </summary>
        /// <param name="experience">The interaction's experience.</param>
        public void SetExperience(Experiment experience)
        {
            this.experience = experience;
        }
        /// <summary>
        /// Gets the <see cref="Result"/>.
        /// </summary>
        /// <returns>
        /// The interaction's result
        /// </returns>
        public Result GetResult()
        {
            return result;
        }
        /// <summary>
        /// Sets the result.
        /// </summary>
        /// <param name="result">The interaction's result.</param>
        public void SetResult(Result result)
        {
            this.result = result;
        }
        public void SetValence(int valence)
        {
            this.valence = valence;
        }
        public int GetValence()
        {
            return valence;
        }
        /// <summary>
        /// Returns the label of the experience.
        /// </summary>
        /// <returns></returns>
        public string toString()
        {
            return experience.GetLabel() + result.GetLabel() + "," + GetValence();
        }

    }
}
