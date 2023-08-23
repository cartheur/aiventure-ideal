using Cartheur.Ideal.Mooc.Coupling;
using Cartheur.Ideal.Mooc.Interfaces;

namespace Cartheur.Ideal.Mooc.Environment
{
    public class Environment50 : IEnvironment
    {
        private Existence existence;
        private Interaction previousInteraction;
        private Interaction penultimateInteraction;

        public Environment50(Existence existence)
        {
            this.existence = existence;
            Initialize();
        }

        protected void Initialize()
        {
            this.GetExistence().AddOrGetPrimitiveInteraction(GetExistence().LABEL_E1 + GetExistence().LABEL_R1, -1);
            Interaction i12 = this.GetExistence().AddOrGetPrimitiveInteraction(this.GetExistence().LABEL_E1 + this.GetExistence().LABEL_R2, 1);
            this.GetExistence().AddOrGetPrimitiveInteraction(this.GetExistence().LABEL_E2 + this.GetExistence().LABEL_R1, -1);
            Interaction i22 = this.GetExistence().AddOrGetPrimitiveInteraction(this.GetExistence().LABEL_E2 + this.GetExistence().LABEL_R2, 1);
            this.GetExistence().AddOrGetAbstractExperience(i12);
            this.GetExistence().AddOrGetAbstractExperience(i22);
        }

        protected Existence GetExistence()
        {
            return existence;
        }


        protected void SetPreviousInteraction(Interaction previousInteraction)
        {
            this.previousInteraction = previousInteraction;
        }
        protected Interaction getPreviousInteraction()
        {
            return previousInteraction;
        }


        protected void SetPenultimateInteraction(Interaction penultimateInteraction)
        {
            this.penultimateInteraction = penultimateInteraction;
        }
        protected Interaction GetPenultimateInteraction()
        {
            return penultimateInteraction;
        }

        public Interaction Enact(Interaction intendedInteraction)
        {
            Interaction enactedInteraction = null;

            if (intendedInteraction.GetLabel().Contains(GetExistence().LABEL_E1))
            {
                if (this.getPreviousInteraction() != null &&
                    (this.GetPenultimateInteraction() == null || this.GetPenultimateInteraction().GetLabel().Contains(this.GetExistence().LABEL_E2)) &&
                        this.getPreviousInteraction().GetLabel().Contains(this.GetExistence().LABEL_E1))
                    enactedInteraction = this.GetExistence().AddOrGetPrimitiveInteraction(this.GetExistence().LABEL_E1 + this.GetExistence().LABEL_R2, 0);
                else
                    enactedInteraction = this.GetExistence().AddOrGetPrimitiveInteraction(this.GetExistence().LABEL_E1 + this.GetExistence().LABEL_R1, 0);
            }
            else
            {
                if (this.getPreviousInteraction() != null &&
                    (this.GetPenultimateInteraction() == null || this.GetPenultimateInteraction().GetLabel().Contains(this.GetExistence().LABEL_E1)) &&
                        this.getPreviousInteraction().GetLabel().Contains(this.GetExistence().LABEL_E2))
                    enactedInteraction = this.GetExistence().AddOrGetPrimitiveInteraction(this.GetExistence().LABEL_E2 + this.GetExistence().LABEL_R2, 0);
                else
                    enactedInteraction = this.GetExistence().AddOrGetPrimitiveInteraction(this.GetExistence().LABEL_E2 + this.GetExistence().LABEL_R1, 0);
            }

            this.SetPenultimateInteraction(this.getPreviousInteraction());
            this.SetPreviousInteraction(enactedInteraction);

            return enactedInteraction;
        }


    }
}
