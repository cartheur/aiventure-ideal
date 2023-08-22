using Cartheur.Ideal.Mooc.Agent;
using Cartheur.Ideal.Mooc.Coupling;
using Cartheur.Ideal.Mooc.Interfaces;
using System.Collections;

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
        private Interaction enactedInteraction;

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
            Experiment e1 = AddOrGetExperience(LABEL_E1);
            AddOrGetExperience(LABEL_E2);
            SetPreviousExperience(e1);
            // 020-level features
            Experiment e2 = AddOrGetExperience(LABEL_E2);
            Result r1 = CreateOrGetResult(LABEL_R1);
            Result r2 = CreateOrGetResult(LABEL_R2);

            AddOrGetPrimitiveInteraction(e1, r1, -1);
            AddOrGetPrimitiveInteraction(e1, r2, 1);
            AddOrGetPrimitiveInteraction(e2, r1, -1);
            AddOrGetPrimitiveInteraction(e2, r2, 1);
            //SetPreviousExperience(e1); <-- 030-level feature requisite
        }

        /// <summary>
        /// Perform one step of a "stream of intelligence".
        /// </summary>
        /// <returns>
        /// A string representing the "event of intelligence" that was performed.
        /// </returns>
        public string Step()
        {
            List<Anticipation> anticipations = Anticipate();
            Experiment experience = SelectExperience(anticipations);
            //Experiment experience = SelectInteraction(anticipations).GetExperience();

            /** Change the call to the function returnResult to change the environment */
            //Result result = ReturnResult010(experience);
            //Result result = ReturnResult030(experience);
            Result result = ReturnResult031(experience);

            Interaction enactedInteraction = GetInteraction(experience.GetLabel() + result.GetLabel());
            Console.WriteLine("Enacted " + enactedInteraction.toString());

            if (enactedInteraction.GetValence() >= 0)
                SetMood(Mood.PLEASED);
            else
                SetMood(Mood.PAINED);

            LearnCompositeInteraction(enactedInteraction);

            SetEnactedInteraction(enactedInteraction);

            return "" + GetMood();

            #region Previous version
            //Experiment experience = GetPreviousExperience();
            //if (GetMood() == Mood.PAINED)
            //    experience = GetOtherExperience(experience);
            //if (GetMood() == Mood.BORED)
            //{
            //    experience = GetOtherExperience(experience);
            //    SetSelfSatisfactionCounter(0);
            //}
            //Result anticipatedResult = Predict(experience);
            //Result result = ReturnResult010(experience);
            //AddOrGetPrimitiveInteraction(experience, result);

            //Interaction enactedInteraction = (Interaction)AddOrGetPrimitiveInteraction(experience, result);

            //if (result == anticipatedResult)
            //{
            //    SetMood(Mood.SELF_SATISFIED);
            //    IncSelfSatisfactionCounter();
            //}
            //else
            //{
            //    SetMood(Mood.FRUSTRATED);
            //    SetSelfSatisfactionCounter(0);
            //}
            //if (GetSelfSatisfactionCounter() >= BOREDOME_LEVEL)
            //    SetMood(Mood.BORED);
            //if (enactedInteraction.GetValence() >= 0)
            //    SetMood(Mood.PLEASED);
            //else
            //    SetMood(Mood.PAINED);

            //SetPreviousExperience(experience);

            //return experience.GetLabel() + result.GetLabel() + "--" + GetMood();
            #endregion
        }

        /**
	 * Learn the composite interaction from the previous enacted interaction and the current enacted interaction
	 */
        public void LearnCompositeInteraction(Interaction enactedInteraction)
        {
            Interaction preInteraction = GetEnactedInteraction();
            Interaction postInteraction = enactedInteraction;
            if (preInteraction != null)
            {
                Interaction interaction = (Interaction)AddOrGetCompositeInteraction(preInteraction, postInteraction);
                interaction.IncrementWeight();
            }
        }

        /**
         * Records a composite interaction in memory
         * @param preInteraction: The composite interaction's pre-interaction
         * @param postInteraction: The composite interaction's post-interaction
         * @return the learned composite interaction
         */
        public Interaction AddOrGetCompositeInteraction(Interaction preInteraction, Interaction postInteraction)
        {
            int valence = preInteraction.GetValence() + postInteraction.GetValence();
            Interaction interaction = (Interaction)AddOrGetInteraction(preInteraction.GetLabel() + postInteraction.GetLabel());
            interaction.SetPreInteraction(preInteraction);
            interaction.SetPostInteraction(postInteraction);
            interaction.SetValence(valence);
            Console.WriteLine("learn " + interaction.GetLabel());
            return interaction;
        }

        protected Interaction CreateInteraction(String label)
        {
            return new Interaction(label);
        }

        /// <summary>
        /// Computes the list of anticipations.
        /// </summary>
        /// <returns>The list of anticipations</returns>       
        public List<Anticipation> Anticipate()
        {
            List<Anticipation> anticipations = GetDefaultAnticipations();

            if (GetEnactedInteraction() != null)
            {
                foreach (Interaction activatedInteraction in GetActivatedInteractions())
                {
                    Anticipation proposition = new Anticipation(((Interaction)activatedInteraction).GetPostInteraction().GetExperience(), ((Interaction)activatedInteraction).GetWeight() * ((Interaction)activatedInteraction).GetPostInteraction().GetValence());
                    int index = anticipations.IndexOf(proposition);
                    if (index < 0)
                        anticipations.Add(proposition);
                    else
                        ((Anticipation)anticipations[index]).AddProclivity(((Interaction)activatedInteraction).GetWeight() * ((Interaction)activatedInteraction).GetPostInteraction().GetValence());
                }
            }
            return anticipations;
        }

        protected Interaction SelectInteraction(List<Anticipation> anticipations)
        {
            anticipations.Sort();
            Interaction intendedInteraction;

            if (anticipations.Count > 0)
            {
                Interaction affordedInteraction = anticipations[0].GetInteraction();
                if (affordedInteraction.GetValence() >= 0)
                    intendedInteraction = affordedInteraction;
                else
                    intendedInteraction = (Interaction)GetOtherInteraction(affordedInteraction);
            }
            else
                intendedInteraction = GetOtherInteraction(null);
            return (Interaction)intendedInteraction;
        }
        /// <summary>
        /// Get the list of activated interactions. An activated interaction is a composite interaction whose preInteraction matches the enactedInteraction.
        /// </summary>
        /// <returns>The list of anticipations</returns>
        public List<Interaction> GetActivatedInteractions()
        {
            List<Interaction> activatedInteractions = new List<Interaction>();
            if (GetEnactedInteraction() != null)
                foreach (Interaction activatedInteraction in INTERACTIONS.Values)
                    if (((Interaction)activatedInteraction).GetPreInteraction() == GetEnactedInteraction())
                        activatedInteractions.Add((Interaction)activatedInteraction);
            return activatedInteractions;
        }

        protected Interaction GetInteraction(String label)
        {
            return (Interaction)INTERACTIONS[label];
        }
        /// <summary>
        /// Gets the other interaction.
        /// </summary>
        /// <param name="interaction">The interaction.</param>
        /// <returns></returns>
        public Interaction GetOtherInteraction(Interaction interaction)
        {
            Interaction otherInteraction = (Interaction)INTERACTIONS.Values.ToArray()[0];
            if (interaction != null)
                foreach (Interaction e in INTERACTIONS.Values)
                {
                    if (e.GetExperience() != null && e.GetExperience() != interaction.GetExperience())
                    {
                        otherInteraction = e;
                        break;
                    }
                }
            return otherInteraction;
        }

        protected void SetEnactedInteraction(Interaction enactedInteraction)
        {
            this.enactedInteraction = enactedInteraction;
        }
        protected Interaction GetEnactedInteraction()
        {
            return enactedInteraction;
        }

        protected List<Anticipation> GetDefaultAnticipations()
        {
            List<Anticipation> anticipations = new List<Anticipation>();
            foreach (Experiment experience in EXPERIENCES.Values)
            {
                Anticipation anticipation = new Anticipation(experience, 0);
                anticipations.Add(anticipation);
            }
            return anticipations;
        }

        public Experiment SelectExperience(List<Anticipation> anticipations)
        {
            // The list of anticipations is never empty because all the experiences are proposed by default with a proclivity of 0
            anticipations.Sort();
            foreach (Anticipation anticipation in anticipations)
                Console.WriteLine("propose " + anticipation.ToString());

            Anticipation selectedAnticipation = (Anticipation)anticipations[0];
            return selectedAnticipation.GetExperience();
        }

        /**
         * Environment030
         * Results in R1 when the current experience equals the previous experience
         * and in R2 when the current experience differs from the previous experience.
         */
        protected Result ReturnResult030(Experiment experience)
        {
            Result result = null;
            if (GetPreviousExperience() == experience)
                result = CreateOrGetResult(LABEL_R1);
            else
                result = CreateOrGetResult(LABEL_R2);
            SetPreviousExperience(experience);

            return result;
        }

        #region Old version of interaction
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
        ///// <summary>
        ///// Creates the interaction.
        ///// </summary>
        ///// <param name="label">The label.</param>
        ///// <returns></returns>
        //protected Interaction CreateInteraction(string label)
        //{
        //    return new Interaction(label);
        //}

        ///// <summary>
        ///// Finds an interaction from its label
        ///// </summary>
        ///// <param name="label">The label of this interaction.</param>
        ///// <returns>The interaction.</returns>
        //protected Interaction GetInteraction(string label)
        //{
        //    return INTERACTIONS.ContainsKey(label) ? INTERACTIONS[label] : null;
        //}
        ///// <summary>
        ///// Gets the interaction.
        ///// </summary>
        ///// <param name="label">The label.</param>
        ///// <param name="check">Check this</param>
        ///// <returns></returns>
        //protected Interaction GetInteraction(string label, bool check)
        //{
        //    return (Interaction)INTERACTIONS[label];
        //}

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

        /**
	 * Environment031
	 * Before time T1 and after time T2: E1 results in R1; E2 results in R2
	 * between time T1 and time T2: E1 results R2; E2results in R1.
	 */
        protected int T1 = 8;
        protected int T2 = 15;
        private int clock = 0;
        protected int GetClock()
        {
            return clock;
        }
        protected void IncClock()
        {
            clock++;
        }

        public Result ReturnResult031(Experiment experience)
        {

            Result result = null;

            IncClock();

            if (GetClock() <= T1 || GetClock() > T2)
            {
                if (experience.Equals(AddOrGetExperience(LABEL_E1)))
                    result = CreateOrGetResult(LABEL_R1);
                else
                    result = CreateOrGetResult(LABEL_R2);
            }
            else
            {
                if (experience.Equals(AddOrGetExperience(LABEL_E1)))
                    result = CreateOrGetResult(LABEL_R2);
                else
                    result = CreateOrGetResult(LABEL_R1);
            }
            return result;
        }
    }
}
