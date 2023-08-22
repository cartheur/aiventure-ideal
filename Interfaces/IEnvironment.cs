using Cartheur.Ideal.Mooc.Coupling;

namespace Cartheur.Ideal.Mooc.Interfaces
{
    /// <summary>
    /// An Environment simulates the enaction of an intended primitive interaction and returns the resulting enacted primitive interaction.
    /// </summary>
    public interface IEnvironment
    {
        /// <summary>
        /// Enacts an intended interaction.
        /// </summary>
        /// <param name="intendedInteraction">The intended interaction.</param>
        /// <returns></returns>
        public Interaction Enact(Interaction intendedInteraction);

    }
}
