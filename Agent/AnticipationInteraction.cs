using Cartheur.Ideal.Mooc.Coupling;
using Cartheur.Ideal.Mooc.Interfaces;

namespace Cartheur.Ideal.Mooc.Agent
{
    /// <summary>
    /// An Anticipation is created for each proposed primitive interaction. An Anticipation is greater than another if its interaction has a greater valence than the other's.
    /// </summary>
    /// <seealso cref="Cartheur.Ideal.Mooc.Interfaces.IAnticipation" />
    public class AnticipationInteraction : IAnticipation
    {
        Interaction interaction;
        /// <summary>
        /// Initializes a new instance of the <see cref="Anticipation"/> class.
        /// </summary>
        /// <param name="interaction">The interaction.</param>
        public Anticipation(Interaction interaction)
        {
            this.interaction = interaction;
        }
        /// <summary>
        /// Gets the interaction.
        /// </summary>
        /// <returns></returns>
        public Interaction GetInteraction()
        {
            return interaction;
        }
        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="anticipation">The anticipation.</param>
        /// <returns></returns>
        public int CompareTo(IAnticipation anticipation)
        {
            return ((int)((Anticipation)anticipation).GetInteraction().GetValence()).CompareTo(this.interaction.GetValence());
        }
    }
}
