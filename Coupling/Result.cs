namespace Cartheur.Ideal.Mooc.Coupling
{
    /// <summary>
    /// A result of an experience.
    /// </summary>
    public class Result
    {
        private readonly string label;
        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        public Result(string label)
        {
            this.label = label;
        }
        /// <summary>
        /// Gets the label of the result.
        /// </summary>
        /// <returns></returns>
        public string GetLabel()
        {
            return label;
        }
    }
}
