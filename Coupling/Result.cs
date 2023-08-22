namespace Cartheur.Ideal.Mooc.Coupling
{
    /// <summary>
    /// A result of an experience.
    /// </summary>
    public class Result
    {

        /**
         * The result's label.
         */
        private string label;

        public Result(string label)
        {
            this.label = label;
        }

        public string getLabel()
        {
            return label;
        }

    }
}
