using Cartheur.Ideal.Mooc.Coupling.Interaction;

namespace Cartheur.Ideal.Mooc.Environment
{
    /// <summary>
    /// An Environment simulates the enaction of an intended primitive interaction and returns the resulting enacted primitive interaction.
    /// </summary>
    public interface IEnvironment
    {

        public Interaction Enact(Interaction intendedInteraction);

    }
}
