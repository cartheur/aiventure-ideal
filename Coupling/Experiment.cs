namespace Cartheur.Ideal.Mooc.Coupling
{
    /// <summary>
    /// An experiment that can be chosen by the agent.
    /// </summary>
    public class Experiment
    {

        /**
         * The experience's label.
         */
        private string label;

        public Experiment(string label)
        {
            this.label = label;
        }

        public string getLabel()
        {
            return label;
        }
    }
}
