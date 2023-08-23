using Cartheur.Ideal.Mooc.Agent;
using Cartheur.Ideal.Mooc.Coupling;
using Cartheur.Ideal.Mooc.Environment;
using Cartheur.Ideal.Mooc.Interfaces;
using Cartheur.Ideal.Mooc.Tracer;

namespace Cartheur.Ideal.Mooc
{
    public enum Mood { SATISFIED, FRUSTRATED, BORED, PAINED, PLEASED };

    /// <summary>
    /// An Existence010 simulates a "stream of intelligence" made of a succession of Experiences and Results. The Existence010 is SELF-SATISFIED when the Result corresponds to the Result it expected, and FRUSTRATED otherwise. Additionally, the Existence0 is BORED when it has been SELF-SATISFIED for too long, which causes it to try another Experience. An Existence1 is still a single entity rather than being split into an explicit Agent and Environment.
    /// </summary>
    /// <seealso cref="IExistence" />
    public class Existence : IExistence
    {
        public string LABEL_E1 = "e1";
        public string LABEL_E2 = "e2";
        public string LABEL_R1 = "r1";
        public string LABEL_R2 = "r2";

        protected int T1 = 8;
        protected int T2 = 15;
        private int clock = 0;
        protected int BOREDOM = 4;
        private int selfSatisfactionCounter = 0;

        private Mood mood;
        private Experiment previousExperience;

        private Interaction previousSuperInteraction;
        private Interaction lastSuperInteraction;
        private Interaction currentSuperInteraction;
        private Interaction enactedInteraction;
        private Interaction affordedInteraction;

        public Dictionary<string, Experiment> Experiences = new Dictionary<string, Experiment>();
        protected Dictionary<string, Result> Results = new Dictionary<string, Result>();
        protected Dictionary<string, Interaction> Interactions = new Dictionary<string, Interaction>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Existence"/> class.
        /// </summary>
        public Existence()
        {
            Initialize();
        }

        void Initialize()
        {
            // 010-level features
            //SetPreviousExperience(e1); <-- 030-level feature requisite
            // 040 (050)-level features
            Experiment e1 = (Experiment)AddOrGetExperience(LABEL_E1);
            Experiment e2 = (Experiment)AddOrGetExperience(LABEL_E2);
            Result r1 = CreateOrGetResult(LABEL_R1);
            Result r2 = CreateOrGetResult(LABEL_R2);
            // Change the valence depending on the environment to obtain better behaviours.
            Interaction e11 = (Interaction)AddOrGetPrimitiveInteraction(e1, r1, -1);
            Interaction e12 = (Interaction)AddOrGetPrimitiveInteraction(e1, r2, 1); // Use valence 1 for Environment040 and 2 for Environment041
            Interaction e21 = (Interaction)AddOrGetPrimitiveInteraction(e2, r1, -1);
            Interaction e22 = (Interaction)AddOrGetPrimitiveInteraction(e2, r2, 1); // Use valence 1 for Environment040 and 2 for Environment041
            e1.SetIntendedInteraction(e12); e1.ResetAbstract();
            e2.SetIntendedInteraction(e22); e2.ResetAbstract();
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
            //Interaction intendedInteraction = SelectInteraction(anticipations);
            Experiment experience = SelectExperience(anticipations);
            Interaction intendedInteraction = experience.GetIntendedInteraction();
            //Experiment experience = SelectInteraction(anticipations).GetExperience();

            // Change the call to the function returnResult to change the environment
            //Result result = ReturnResult010(experience);
            //Result result = ReturnResult030(experience);
            Result result = ReturnResult041(experience);

            Interaction enactedInteraction = Enact(intendedInteraction);
            //Interaction enactedInteraction = GetInteraction(experience.GetLabel() + result.GetLabel());
            Console.WriteLine("Enacted " + enactedInteraction.toString());

            if (enactedInteraction != intendedInteraction)
            {
                intendedInteraction.AddAlternateInteraction(enactedInteraction);
                Console.WriteLine("Alternate " + enactedInteraction.GetLabel());
            }
            if (enactedInteraction != intendedInteraction && experience.IsAbstract())
            {
                Result failResult = CreateOrGetResult(enactedInteraction.GetLabel().Replace('e', 'E').Replace('r', 'R') + ">");
                int valence = enactedInteraction.GetValence();
                enactedInteraction = (Interaction)AddOrGetPrimitiveInteraction(experience, failResult, valence);
            }

            if (enactedInteraction.GetValence() >= 0)
                SetMood(Mood.PLEASED);
            else
                SetMood(Mood.PAINED);
            if (enactedInteraction == intendedInteraction)
            {
                SetMood(Mood.SATISFIED);
                IncrementSelfSatisfactionCounter();
            }
            else
            {
                SetMood(Mood.FRUSTRATED);
                SetSelfSatisfactionCounter(0);
            }
            if (enactedInteraction.GetValence() >= 0)
                SetMood(Mood.PLEASED);
            else
                SetMood(Mood.PAINED);

            LearnCompositeInteraction(enactedInteraction);
            SetPreviousSuperInteraction(GetLastSuperInteraction());
            SetEnactedInteraction(enactedInteraction);

            return "" + GetMood();

        }
        /// <summary>
        /// Learn the composite interaction from the previous enacted interaction and the current enacted interaction.
        /// </summary>
        /// <param name="enactedInteraction">The enacted interaction.</param>
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

        /// <summary>
        /// Records a composite interaction in memory.
        /// </summary>
        /// <param name="preInteraction">The composite interaction's pre-interaction</param>
        /// <param name="postInteraction">The composite interaction's post-interaction</param>
        /// <returns>The learned composite interaction.</returns>
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

        protected static Interaction CreateInteraction(String label)
        {
            return new Interaction(label);
        }

        public List<Anticipation> Anticipate()
        {
            List<Anticipation> anticipations = GetDefaultAnticipations();
            List<Interaction> activatedInteractions = GetActivatedInteractions();

            if (GetEnactedInteraction() != null)
            {
                foreach (Interaction activatedInteraction in activatedInteractions)
                {
                    if (((Interaction)activatedInteraction).GetPostInteraction().GetExperience() != null)
                    {
                        Anticipation anticipation = new Anticipation(((Interaction)activatedInteraction).GetPostInteraction().GetExperience(), ((Interaction)activatedInteraction).GetWeight() * ((Interaction)activatedInteraction).GetPostInteraction().GetValence());
                        int index = anticipations.IndexOf(anticipation);
                        if (index < 0)
                            anticipations.Add(anticipation);
                        else
                            ((Anticipation)anticipations[index]).AddProclivity(((Interaction)activatedInteraction).GetWeight() * ((Interaction)activatedInteraction).GetPostInteraction().GetValence());
                    }
                }
            }

            foreach (Interaction activatedInteraction in activatedInteractions)
            {
                Interaction proposedInteraction = (Interaction)((Interaction)activatedInteraction).GetPostInteraction();
                int proclivity = ((Interaction)activatedInteraction).GetWeight() * proposedInteraction.GetValence();
                Anticipation anticipation = new Anticipation(proposedInteraction, proclivity);
                int index = anticipations.IndexOf(anticipation);
                if (index < 0)
                    anticipations.Add(anticipation);
                else
                    ((Anticipation)anticipations[index]).AddProclivity(((Interaction)activatedInteraction).GetWeight() * ((Interaction)activatedInteraction).GetPostInteraction().GetValence());
            }

            foreach (Anticipation anticipation in anticipations)
            {
                foreach (Interaction interaction in ((Experiment)((Anticipation)anticipation).GetExperience()).GetEnactedInteractions())
                {
                    foreach (Interaction activatedInteraction in activatedInteractions)
                    {
                        if (interaction == ((Interaction)activatedInteraction).GetPostInteraction())
                        {
                            int proclivity = ((Interaction)activatedInteraction).GetWeight() * ((Interaction)interaction).GetValence();
                            ((Anticipation)anticipation).AddProclivity(proclivity);
                        }
                    }
                }
            }

            return anticipations;
        }
        public List<Anticipation> Anticipate10()
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
                affordedInteraction = (Interaction)anticipations[0].GetInteraction();
                if (affordedInteraction.GetValence() >= 0)
                    intendedInteraction = affordedInteraction;
                else
                    intendedInteraction = (Interaction)GetOtherInteraction(affordedInteraction);
            }
            else
                intendedInteraction = GetOtherInteraction(null);
            return (Interaction)intendedInteraction;
        }

        public Interaction SelectInteraction(List<Anticipation> anticipations, bool evolve)
        {

            anticipations.Sort();
            Interaction intendedInteraction = (Interaction)GetOtherInteraction(null);
            if (GetSelfSatisfactionCounter() < BOREDOM)
            {
                if (anticipations.Count > 0)
                {
                    Interaction proposedInteraction = (Interaction)((Anticipation)anticipations[0]).GetInteraction();
                    if (proposedInteraction.GetValence() >= 0)
                        intendedInteraction = proposedInteraction;
                    else
                        intendedInteraction = (Interaction)GetOtherInteraction(proposedInteraction);
                }
            }
            else
            {
                Trace.AddEventElement("mood", "BORED");
                SetSelfSatisfactionCounter(0);

                if (anticipations.Count == 1)
                    intendedInteraction = (Interaction)GetOtherInteraction(((Anticipation)anticipations[0]).GetInteraction());
                else if (anticipations.Count > 1)
                    intendedInteraction = (Interaction)((Anticipation)anticipations[1]).GetInteraction();
            }
            return intendedInteraction;
        }

        /// <summary>
        /// Get the list of activated interactions. An activated interaction is a composite interaction whose preInteraction matches the enactedInteraction.
        /// </summary>
        /// <returns>The list of anticipations</returns>
        public List<Interaction> GetActivatedInteractions()
        {
            List<Interaction> activatedInteractions = new List<Interaction>();
            if (GetEnactedInteraction() != null)
                foreach (Interaction activatedInteraction in Interactions.Values)
                    if (((Interaction)activatedInteraction).GetPreInteraction() == GetEnactedInteraction())
                        activatedInteractions.Add((Interaction)activatedInteraction);
            return activatedInteractions;
        }

        protected Interaction GetInteraction(String label)
        {
            return (Interaction)Interactions[label];
        }
        /// <summary>
        /// Gets the other interaction.
        /// </summary>
        /// <param name="interaction">The interaction.</param>
        /// <returns></returns>
        public Interaction GetOtherInteraction(Interaction interaction)
        {
            Interaction otherInteraction = (Interaction)Interactions.Values.ToArray()[0];
            if (interaction != null)
                foreach (Interaction e in Interactions.Values)
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
            foreach (Experiment experience in Experiences.Values)
            {
                Anticipation anticipation = new Anticipation(experience, 0);
                anticipations.Add(anticipation);
            }
            return anticipations;
        }
        public void SetPreviousSuperInteraction(Interaction previousSuperInteraction)
        {
            this.previousSuperInteraction = previousSuperInteraction;
        }
        public Interaction GetLastSuperInteraction()
        {
            return lastSuperInteraction;
        }
        public void SetLastSuperInteraction(Interaction lastSuperInteraction)
        {
            this.lastSuperInteraction = lastSuperInteraction;
        }

        public static Experiment SelectExperience(List<Anticipation> anticipations)
        {
            // The list of anticipations is never empty because all the experiences are proposed by default with a proclivity of 0
            anticipations.Sort();
            foreach (Anticipation anticipation in anticipations)
                Console.WriteLine("propose " + anticipation.ToString());

            Anticipation selectedAnticipation = (Anticipation)anticipations[0];
            return selectedAnticipation.GetExperience();
        }      

        /// <summary>
        /// Create an interaction as a tuple <experience, result>.
        /// </summary>
        /// <param name="experience">The experience.</param>
        /// <param name="result">The result.</param>
        /// <returns>The created interaction</returns>
        public Interaction AddOrGetPrimitiveInteraction(Experiment experience, Result result)
        {
            Interaction interaction = AddOrGetInteraction(experience.GetLabel() + result.GetLabel());
            interaction.SetExperience(experience);
            interaction.SetResult(result);
            return interaction;
        }
        /// <summary>
        /// Create an interaction from its label.
        /// </summary>
        /// <param name="label">This interaction's label.</param>
        /// <param name="valence">The interaction's valence.</param>
        /// <returns>The created interaction</returns>
        public Interaction AddOrGetPrimitiveInteraction(string label, int valence)
        {
            Interaction interaction;

            if (!Interactions.ContainsKey(label))
            {
                interaction = CreateInteraction(label);
                interaction.SetValence(valence);
                Interactions.Add(label, interaction);
            }
            interaction = (Interaction)Interactions[label];

            return interaction;
        }

        /// <summary>
        /// Records an interaction in memory.
        /// </summary>
        /// <param name="label">The label of this interaction.</param>
        /// <returns>The interaction.</returns>
        protected Interaction AddOrGetInteraction(string label)
        {
            if (!Interactions.ContainsKey(label))
                Interactions.Add(label, CreateInteraction(label));
            return Interactions.ContainsKey(label) ? Interactions[label] : null;
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
            if (!Interactions.ContainsKey(label))
            {
                Interaction interactions = CreateInteraction(label);
                interactions.SetExperience(experience);
                interactions.SetResult(result);
                interactions.SetValence(valence);
                Interactions.Add(label, interactions);
            }
            Interaction interaction = (Interaction)Interactions[label];
            return interaction;
        }

        /// <summary>
        /// Finds an interaction from its experience.
        /// </summary>
        /// <param name="experience">The experience.</param>
        /// <returns>The interaction.</returns>
        protected Result Predict(Experiment experience)
        {
            Interaction interaction = null;
            Result anticipatedResult = null;

            foreach (Interaction i in Interactions.Values)
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
            if (!Experiences.ContainsKey(label))
                Experiences.Add(label, CreateExperience(label));
            return Experiences.ContainsKey(label) ? Experiences[label] : null;
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
            foreach (Experiment e in Experiences.Values)
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
            if (!Results.ContainsKey(label))
                Results[label] = new Result(label);
            return Results[label];
        }

        #region Needs refactoring

        public Interaction Enact(Interaction intendedInteraction)
        {

            if (intendedInteraction.IsPrimitive())
                return EnactPrimitiveIntearction(intendedInteraction);
            else
            {
                // Enact the pre-interaction
                Interaction enactedPreInteraction = Enact(intendedInteraction.GetPreInteraction());
                if (!enactedPreInteraction.Equals(intendedInteraction.GetPreInteraction()))
                    // if the preInteraction failed then the enaction of the intendedInteraction is interrupted here.
                    return enactedPreInteraction;
                else
                {
                    // Enact the post-interaction
                    Interaction enactedPostInteraction = Enact(intendedInteraction.GetPostInteraction());
                    return (Interaction)AddOrGetCompositeInteraction(enactedPreInteraction, enactedPostInteraction);
                }
            }
        }

        public Experiment AddOrGetAbstractExperience(Interaction interaction)
        {
            String label = interaction.GetLabel().Replace('e', 'E').Replace('r', 'R').Replace('>', '|');
            if (!Experiences.ContainsKey(label))
            {
                Experiment abstractExperience = new Experiment(label);
                abstractExperience.SetIntendedInteraction(interaction);
                interaction.SetExperience(abstractExperience);
                Experiences.Add(label, abstractExperience);
            }
            return (Experiment)Experiences[label];
        }

        /**
         * Implements the cognitive coupling between the agent and the environment
         * @param intendedPrimitiveInteraction: The intended primitive interaction to try to enact against the environment
         * @param The actually enacted primitive interaction.
         */
        public Interaction EnactPrimitiveIntearction(Interaction intendedPrimitiveInteraction)
        {
            Experiment experience = intendedPrimitiveInteraction.GetExperience();
            /** Change the returnResult() to change the environment 
             *  Change the valence of primitive interactions to obtain better behaviors */
            //Result result = returnResult010(experience);
            //Result result = returnResult030(experience);
            //Result result = returnResult031(experience);
            //Result result = ReturnResult040(experience);
            Result result = ReturnResult041(experience);
            return (Interaction)AddOrGetPrimitiveInteraction(experience, result);
        }

        public Interaction GetPreviousSuperInteraction()
        {
            return previousSuperInteraction;
        }

        #endregion

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
        public void IncrementSelfSatisfactionCounter()
        {
            selfSatisfactionCounter++;
        }
        protected int GetClock()
        {
            return clock;
        }
        protected void IncrementClock()
        {
            clock++;
        }

        #region Results to be returned
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
        public Result ReturnResult031(Experiment experience)
        {

            Result result = null;

            IncrementClock();

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
        public Result ReturnResult040(Experiment experience)
        {
            Result result = CreateOrGetResult(LABEL_R1);

            if (Environment40.GetPenultimateExperience() != experience &&
                GetPreviousExperience() == experience)
                result = CreateOrGetResult(LABEL_R2);

            Environment40.SetPenultimateExperience(GetPreviousExperience());
            SetPreviousExperience(experience);

            return result;
        }
        public Result ReturnResult041(Experiment experience)
        {
            Result result = CreateOrGetResult(LABEL_R1);

            if (Environment41.GetAntePenultimateExperience() != experience &&
                Environment40.GetPenultimateExperience() == experience &&
                GetPreviousExperience() == experience)
                result = CreateOrGetResult(LABEL_R2);

            Environment41.SetAntePenultimateExperience(Environment40.GetPenultimateExperience());
            Environment40.SetPenultimateExperience(GetPreviousExperience());
            SetPreviousExperience(experience);

            return result;
        }
        #endregion
    }
}
