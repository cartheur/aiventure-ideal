using Cartheur.Ideal.Mooc.Coupling;

namespace Cartheur.Ideal.Mooc.Interfaces
{
    public interface IParticipant
    {
        /// <summary>
        /// Gets the label of the interaction (with a participant).
        /// </summary>
        /// <returns>The interaction's label</returns>
        public string GetLabel();
        /// <summary>
        /// Gets the experience from an interaction.
        /// </summary>
        /// <returns>The interaction's experience</returns>
        public Experiment GetExperience();
        /// <summary>
        /// Gets the result from an interaction.
        /// </summary>
        /// <returns>The interaction's result</returns>
        public Result GetResult();
        /// <summary>
        /// Sets the experience from the interaction.
        /// </summary>
        /// <param name="experience">The interaction's experience.</param>
        public void SetExperience(Experiment experience);
        /// <summary>
        /// Sets the result of the interaction.
        /// </summary>
        /// <param name="result">The interaction's result.</param>
        public void SetResult(Result result);

    }
}
