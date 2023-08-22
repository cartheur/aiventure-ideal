using Cartheur.Ideal.Mooc.Coupling;
using Cartheur.Ideal.Mooc.Interfaces;

namespace Cartheur.Ideal.Mooc.Agent
{
    /// <summary>
    /// An Anticipation is created for each proposed primitive interaction. An Anticipation is greater than another if its interaction has a greater valence than the other's.
    /// </summary>
    /// <seealso cref="Cartheur.Ideal.Mooc.Interfaces.IAnticipation" />
    public class Anticipation : IAnticipation
    {
        Experiment experience;
        int proclivity;

        public Anticipation(Experiment experience, int proclivity)
        {
            this.experience = experience;
            this.proclivity = proclivity;
        }

        public int CompareTo(IAnticipation anticipation)
        {
            return Convert.ToInt32(((Anticipation)anticipation).GetProclivity().CompareTo(proclivity));
        }

        public new bool Equals(object otherProposition)
        {
            return ((Anticipation)otherProposition).GetExperience() == experience;
        }

        public Experiment GetExperience()
        {
            return this.experience;
        }

        public void SetExperience(Experiment experience)
        {
            this.experience = experience;
        }

        public int GetProclivity()
        {
            return proclivity;
        }

        public void AddProclivity(int proclivity)
        {
            this.proclivity += proclivity;
        }

        public new string ToString()
        {
            return this.experience.GetLabel() + " proclivity " + this.proclivity;
        }

        internal Interaction GetInteraction()
        {
            throw new NotImplementedException();
        }
    }
}
