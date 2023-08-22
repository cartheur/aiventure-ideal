namespace Cartheur.Ideal.Mooc.Coupling
{
    /// <summary>
    /// An experiment that can be chosen by the agent.
    /// </summary>
    public class Experiment
    {
        private readonly string label;
        /// <summary>
        /// Initializes a new instance of the <see cref="Experiment"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        public Experiment(string label)
        {
            this.label = label;
        }
        /// <summary>
        /// Gets the label associated with the <see cref="Experiment"/>.
        /// </summary>
        /// <returns></returns>
        public string GetLabel()
        {
            return label;
        }
    }
}
