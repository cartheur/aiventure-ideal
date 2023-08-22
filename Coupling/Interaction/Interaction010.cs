using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cartheur.Ideal.Mooc.Coupling;

namespace Cartheur.Ideal.Mooc.Coupling.Interaction
{
    /**
 * An interaction010 is the association of an experience with a result.
 */
    public class Interaction010 : Interaction
    {


        private string label;
        protected Experiment experience;
        protected Result result;

        public Interaction010(string label)
        {
            this.label = label;
        }

        public string GetLabel()
        {
            return label;
        }

        public Experiment GetExperience()
        {
            return experience;
        }

        public void SetExperience(Experiment experience)
        {
            this.experience = experience;
        }

        public Result GetResult()
        {
            return result;
        }

        public void SetResult(Result result)
        {
            this.result = result;
        }

        public string toString()
        {
            return experience.getLabel() + result.getLabel();
        }

    }
}
