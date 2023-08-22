using Cartheur.Ideal.Mooc.Coupling;
using Cartheur.Ideal.Mooc.Interfaces;

namespace Cartheur.Ideal.Mooc.Existence
{
    /// <summary>
    /// An Existence010 simulates a "stream of intelligence" made of a succession of Experiences and Results. The Existence010 is SELF-SATISFIED when the Result corresponds to the Result it expected, and FRUSTRATED otherwise. Additionally, the Existence0 is BORED when it has been SELF-SATISFIED for too long, which causes it to try another Experience. An Existence1 is still a single entity rather than being split into an explicit Agent and Environment.
    /// </summary>
    /// <seealso cref="IExistence" />
    public class Existence010 : IExistence
    {
        public string LABEL_E1 = "e1";
        public string LABEL_E2 = "e2";
        public string LABEL_R1 = "r1";
        public string LABEL_R2 = "r2";
        public enum Mood { SELF_SATISFIED, FRUSTRATED, BORED, PAINED, PLEASED };

        protected Dictionary<string, Experiment> EXPERIENCES = new Dictionary<string, Experiment>();
        protected Dictionary<string, Result> RESULTS = new Dictionary<string, Result>();
        protected Dictionary<string, Interaction> INTERACTIONS = new Dictionary<string, Interaction>();

        protected int BOREDOME_LEVEL = 4;

        private Mood mood;
        private int selfSatisfactionCounter = 0;
        private Experiment previousExperience;
        /// <summary>
        /// Initializes a new instance of the <see cref="Existence010"/> class.
        /// </summary>
        public Existence010()
        {
            InitializeExistence();
        }

        protected void InitializeExistence()
        {
            // 010-level features
            //Experiment e1 = AddOrGetExperience(LABEL_E1);
            //AddOrGetExperience(LABEL_E2);
            //SetPreviousExperience(e1);
            // 020-level features
            Experiment e1 = AddOrGetExperience(LABEL_E1);
            Experiment e2 = AddOrGetExperience(LABEL_E2);
            Result r1 = CreateOrGetResult(LABEL_R1);
            Result r2 = CreateOrGetResult(LABEL_R2);
            // Change the valence of interactions to change the agent's motivation.
            AddOrGetPrimitiveInteraction(e1, r1, -1);
            AddOrGetPrimitiveInteraction(e1, r2, 1);
            AddOrGetPrimitiveInteraction(e2, r1, -1);
            AddOrGetPrimitiveInteraction(e2, r2, 1);
            SetPreviousExperience(e1);
        }

        /// <summary>
        /// Perform one step of a "stream of intelligence".
        /// </summary>
        /// <returns>
        /// A string representing the "event of intelligence" that was performed.
        /// </returns>
        public string Step()
        {
            Experiment experience = GetPreviousExperience();
            if (GetMood() == Mood.BORED)
            {
                experience = GetOtherExperience(experience);
                SetSelfSatisfactionCounter(0);
            }

            if (GetMood() == Mood.PAINED)
                experience = GetOtherExperience(experience);

            Result anticipatedResult = Predict(experience);

            Result result = ReturnResult010(experience);

            Interaction enactedInteraction = (Interaction)AddOrGetPrimitiveInteraction(experience, result);

            if (enactedInteraction.GetValence() >= 0)
                SetMood(Mood.PLEASED);
            else
                SetMood(Mood.PAINED);

            AddOrGetPrimitiveInteraction(experience, result);

            if (result == anticipatedResult)
            {
                SetMood(Mood.SELF_SATISFIED);
                IncSelfSatisfactionCounter();
            }
            else
            {
                SetMood(Mood.FRUSTRATED);
                SetSelfSatisfactionCounter(0);
            }
            if (GetSelfSatisfactionCounter() >= BOREDOME_LEVEL)
                SetMood(Mood.BORED);

            SetPreviousExperience(experience);

            return experience.GetLabel() + result.GetLabel() + GetMood();
        }

        #region Interaction
        /// <summary>
        /// Create an interaction as a tuple <experience, result>.
        /// </summary>
        /// <param name="experience">The experience.</param>
        /// <param name="result">The result.</param>
        /// <returns>The created interaction</returns>
        protected Interaction AddOrGetPrimitiveInteraction(Experiment experience, Result result)
        {
            Interaction interaction = AddOrGetInteraction(experience.GetLabel() + result.GetLabel());
            interaction.SetExperience(experience);
            interaction.SetResult(result);
            return interaction;
        }

        /// <summary>
        /// Records an interaction in memory.
        /// </summary>
        /// <param name="label">The label of this interaction.</param>
        /// <returns>The interaction.</returns>
        protected Interaction AddOrGetInteraction(string label)
        {
            if (!INTERACTIONS.ContainsKey(label))
                INTERACTIONS.Add(label, CreateInteraction(label));
            return INTERACTIONS.ContainsKey(label) ? INTERACTIONS[label] : null;
        }
        /// <summary>
        /// Creates the interaction.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <returns></returns>
        protected Interaction CreateInteraction(string label)
        {
            return new Interaction(label);
        }

        /// <summary>
        /// Finds an interaction from its label
        /// </summary>
        /// <param name="label">The label of this interaction.</param>
        /// <returns>The interaction.</returns>
        protected Interaction GetInteraction(string label)
        {
            return INTERACTIONS.ContainsKey(label) ? INTERACTIONS[label] : null;
        }
        /// <summary>
        /// Gets the interaction.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="check">Check this</param>
        /// <returns></returns>
        protected Interaction GetInteraction(string label, bool check)
        {
            return (Interaction)INTERACTIONS[label];
        }

        /// <summary>
        /// Create an interaction as a tuple <experience, result>.
        /// </summary>
        /// <param name="experience">The experience.</param>
        /// <param name="result">The result.</param>
        /// <param name="valence">The interaction's valence.</param>
        /// <returns>The created interaction</returns>
        protected Interaction AddOrGetPrimitiveInteraction(Experiment experience, Result result, int valence)
        {
            string label = experience.GetLabel() + result.GetLabel();
            if (!INTERACTIONS.ContainsKey(label))
            {
                Interaction interactions = CreateInteraction(label);
                interactions.SetExperience(experience);
                interactions.SetResult(result);
                interactions.SetValence(valence);
                INTERACTIONS.Add(label, interactions);
            }
            Interaction interaction = (Interaction)INTERACTIONS[label];
            return interaction;
        }

        #endregion

        /// <summary>
        /// Finds an interaction from its experience.
        /// </summary>
        /// <param name="experience">The experience.</param>
        /// <returns>The interaction.</returns>
        protected Result Predict(Experiment experience)
        {
            Interaction interaction = null;
            Result anticipatedResult = null;

            foreach (Interaction i in INTERACTIONS.Values)
                if (i.GetExperience().Equals(experience))
                    interaction = i;

            if (interaction != null)
                anticipatedResult = interaction.GetResult();

            return anticipatedResult;
        }

        /// <summary>
        /// Creates a new experience from its label and stores it in memory.
        /// </summary>
        /// <param name="label">The experience's label</param>
        /// <returns>The experience.</returns>
        protected Experiment AddOrGetExperience(string label)
        {
            if (!EXPERIENCES.ContainsKey(label))
                EXPERIENCES.Add(label, CreateExperience(label));
            return EXPERIENCES.ContainsKey(label) ? EXPERIENCES[label] : null;
        }

        protected Experiment CreateExperience(string label)
        {
            return new Experiment(label);
        }

        /// <summary>
        /// Finds an experience different from that passed in parameter.
        /// </summary>
        /// <param name="experience">The undesired experience.</param>
        /// <returns>The other experience.</returns>
        protected Experiment GetOtherExperience(Experiment experience)
        {
            Experiment otherExperience = null;
            foreach (Experiment e in EXPERIENCES.Values)
            {
                if (e != experience)
                {
                    otherExperience = e;
                    break;
                }
            }
            return otherExperience;
        }

        /// <summary>
        /// Creates a new result from its label and stores it in memory.
        /// </summary>
        /// <param name="label">The result's label.</param>
        /// <returns>The result.</returns>
        protected Result CreateOrGetResult(string label)
        {
            if (!RESULTS.ContainsKey(label))
                RESULTS[label] = new Result(label);
            return RESULTS[label];
        }

        public Mood GetMood()
        {
            return mood;
        }
        public void SetMood(Mood mood)
        {
            this.mood = mood;
        }

        public Experiment GetPreviousExperience()
        {
            return previousExperience;
        }
        public void SetPreviousExperience(Experiment previousExperience)
        {
            this.previousExperience = previousExperience;
        }

        public int GetSelfSatisfactionCounter()
        {
            return selfSatisfactionCounter;
        }
        public void SetSelfSatisfactionCounter(int selfSatisfactionCounter)
        {
            this.selfSatisfactionCounter = selfSatisfactionCounter;
        }
        public void IncSelfSatisfactionCounter()
        {
            selfSatisfactionCounter++;
        }

        /// <summary>
        /// The Environment010 
        /// * E1 results in R1.E2 results in R2.
        /// </summary>
        /// <param name="experience">The current experience.</param>
        /// <returns>The result of this experience.</returns>
        public Result ReturnResult010(Experiment experience)
        {
            if (experience.Equals(AddOrGetExperience(LABEL_E1)))
                return CreateOrGetResult(LABEL_R1);
            else
                return CreateOrGetResult(LABEL_R2);
        }
    }
}
