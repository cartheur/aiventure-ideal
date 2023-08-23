# aiventure-ideal

Implementation of DEvelopmentAl Learning (IDEAL) Course

## Introduction to the embodied paradigm

The main message that we want to convey is:

Do not consider the agent's input data as the agent's perception of its environment.

The agent is not a passive observer of reality, but rather constructs a perception of reality through active interaction. The term embodied means that the agent must be a part of reality for this active interaction to happen. Those of you who have a background in cognitive science or psychology are probably already familiar with this idea theoretically. In Lesson 1, however, we wish to introduce how this idea translates into the practical design of artificial agents and robots.

## Agent and robot design according to the embodied paradigm

The embodied paradigm suggests shifting perspective from:

- the traditional view in which the agent interprets input data as if it represented the environment (Figure 12/left),
to:
- the embodied view in which the agent constructs a perception of the environment through the active experience of interaction (Figure 12/right).

![Fig.12](/media/012-1.png "Figure 12")

Most representations of the cycle agent/environment do not make explicit the conceptual starting point and end point of the cycle. Since the cycle revolves indefinitely, why should we care anyway?

We should care because, depending on the conceptual starting and end points, we design the agent's algorithm, the robot's sensors, or the simulated environment differently.

In the traditional view, we design the agent's input (called observation o in Figure 12/left) as if it represented the environment's state. In the case of simulated environments, we implement o as a function of s, where s is the state of the environment (o = f (s) in Figure 12/left). In the case of robots, we process the sensor data as if it represented the state of the real world, even though this state is not accessible. This is precisely what the embodied paradigm suggests to avoid because it amounts to considering the agent's input as a representation of the world.

In the embodied view, we design the agent's input (called result r in Figure 12/right) as a result of an experiment initiated by the agent. In simulated environments, we implement r as a function of the experiment and of the state (r = f (e,s) in Figure 12/right). In a given state of the environment, the result may vary according to the experiment. We may even implement environments that have no state, as we do in the next page. When designing robots, we process the sensor data as representing the result of an experiment initiated by the robot.

## Agent implementation according to the embodied paradigm

Table 13 presents the algorithm of a rudimentary embodied system.

```
01   experiment = e1
02   Loop(cycle++)
03      if (mood = BORED)
04         selfSatisfiedDuration = 0
05         experiment = pickOtherExperiment(experiment)
06      anticipatedResult = anticipate(experiment)
07      if (experiment = e1)
08         result = r1
09      else
10         result = r2
11      recordTuple(experiment, result)
12      if (result = anticipatedResult)
13         mood = SELF-SATISFIED
14         selfSatisfiedDuration++
15      else
16         mood = FRUSTRATED
17         selfSatisfiedDuration = 0
18      if (selfSatisfiedDuration > 3)
19         mood = BORED
20      print cycle, experiment, result, mood

```
Table 13, Lines 03 to 05: if the agent is bored, it picks another experiment arbitrarily from amongst the predefined list of experiments at its disposal. Line 06: the anticipate(experiment) function searches memory for a previously learned tuple that matches the chosen experiment, and returns its result as the next anticipated result. Lines 07 to 10 implement the environment: e1 always yields r1, and other experiments always yield r2. Line 11: the agent records the tuple ⟨experiment, result⟩ in memory. Lines 12 to 17: if the result was anticipated correctly then the agent is self-satisfied, otherwise it is frustrated. Lines 18 and 19: if the agent has been self-satisfied for too long (arbitrarily 3 cycles), then it becomes bored.

Notably, this system implements a single program called Existence which does not explicitly differentiate the agent from the environment. Lines 07 to 10 are considered the environment, and the other lines the agent. The environment does not have a state.

## Introduction to the sensorimotor paradigm

The key concept that we want to convey is:

Focus on sensorimotor interactions rather than separating perception from action.

Previously, we saw that the agent's input data should not be confused with the agent's perception. Now, we need to better understand how exactly we should consider the input data. If input data is not perception, then what is it? The sensorimotor paradigm suggests that input data should be taken in association with output data, by combining both of them into a single entity called a sensorimotor interaction. With the formalism introduced on Page 12, this gives i = ⟨e,r⟩: an interaction i is a tuple ⟨experiment, result⟩.

In his theory of a [constructivist](https://en.wikipedia.org/wiki/Constructivism_(philosophy_of_education)) mental development, [Jean Piaget](https://en.wikipedia.org/wiki/Jean_Piaget) coined the term sensorimotor scheme to refer to a pattern of interaction between the agent and its environment. In our model, an interaction is a chunk of data that represents a primitive sensorimotor scheme. From now on, we use the expression "to enact an interaction" to refer to performing the experiment and receiving the result that compose a given interaction. The expression "to intend to enact" interaction ⟨e,r⟩ means that the agent performs experiment e while expecting result r. As a result of this intention, the agent may "actually enact" interaction ⟨e,r'⟩ if it receives result r' instead of r.

The sensorimotor paradigm allows implementing a type of motivation called interactional motivation. Using the embodied model introduced on Page 12, Figure 22 compares traditional reinforcement learning (left) with interactional motivation (right).

![Fig.22](/media/022-1.png "Figure 22") 

In reinforcement learning (Figure 22/left), the agent receives a reward r that specifies desirable goals to reach. In the case of simulated environments, the designer programs the reward function r(s) as a function of the environment's state. In the case of robots, a "reward button" is pressed either automatically when the robot reaches the goal, or manually by the experimenter to train the robot to reach the goal. The agent's policy is designed to choose actions based on their estimated utility for getting the reward. As a result, to an observer of the agent's behavior, the agent appears motivated to reach the goal defined by the experimenter. For example, to model an agent that seeks food, the designer assigns a positive reward to states of the world in which the agent reaches food.

In interactional motivation (Figure 22/right), the agent has no predefined goal to reach. The environment may even lack a state, as we saw on Page 13 and as we will see again on the next page. However, the agent has predefined interactions that it can enact. The designer may associate a scalar valence v(i) with interactions, and design the agent's policy to try to enact interactions that have a positive valence. To model an agent that seeks food, the designer specifies an interaction corresponding to eating (the experiment of biting with the result of tasting good food) and assigns a positive valence to this interaction. In doing so, the designer predefines the agent's preferences of interaction without predefining which states or entities of the world constitute food.

Overall, the sensorimotor paradigm allows designing self-motivated agents without modeling the world as a predefined set of states. Instead, the agent is left alone to construct its own model of the world through its individual experience of interaction. Since there is no predefined model of the world, the agent is not bound to a predefined set of goals. For example, it can discover/categorize new edible entities in the world.

Interactional motivation is not the only possible motivational drive for sensorimotor agents. Recall that we introduced the drive to learn to predict the result of experiments on Page 14. Importantly, since there is not data in a sensorimotor agent that directly represents the environment's state, sensorimotor agent can hardly be pre-programmed to perform a predefined task. If you want them to perform a predefined task, you will have to train them rather than program them.

Since sensorimotor agents are not programmed to seek predefined goals, we do not assess their learning by measuring their performance in reaching predefined goals, but by demonstrating the emergence of cognitive behaviors through behavioral analysis.

## Implementation of interactional motivation

Interactional motivation is compatible with the motivation to predict results introduced on Page 13. We can imagine agents implemented with the two motivational principles concurrently. For simplicity, however, the algorithm in Table 23 only implements interactional motivation.

```
01   createPrimitiveInteraction(e1, r1, -1)
02   createPrimitiveInteraction(e2, r2, 1)
03   experiment = e1
04   While()
05      if (mood = PAINED)
06         experiment = getOtherExperiment(experiment) 
07      if (experiment = e1)
08         result = r1
09      else
10         result = r2        
11      enactedInteraction = getInteraction(experiment, result)
12      if (enactedInteraction.valence ≥ 0)
13         mood = PLEASED
14      else
15         mood = PAINED
16      print experiment, result, mood

```

Table 23, line 01: Interactions ⟨e1,r1⟩ is stored in memory and set with a negative valence (-1). 02: Interaction ⟨e2,r2⟩ is stored in memory and set with a positive valence (1). 04: If the agent is pained then it picks another experiment. Lines 07 to 10 implement the same environment as on Page 13. 11: Retrieves the enacted interaction from the experiment and the result 12 to 15: if the enacted interaction has a positive valence then the agent is pleased, otherwise it is pained.

Notably, if more interactions were specified, the agent would stick to the first interaction it tried that has a positive valence. If all the interactions had a negative valence, the agent would keep trying other interactions. This behavior is still very primitive. In the following discussion, we will address the problem of "scaling up" towards more intelligent behaviors in more complex environments.

## Introduction to constructivist epistemology (theory of knowledge)

The key concept that we want to convey is the imperative of [epistemology](https://en.wikipedia.org/wiki/Epistemology):

Cognitive agents must discover, learn, and exploit regularities of interaction.

Regularities of interaction (in short, regularities) are patterns of interaction that occur consistently. Regularities depend on the coupling between the agent and the environment. That is, they depend both on the structure of the environment, and on the possibilities of interaction that the agent has at its disposal. At least since [Immanuel Kant](https://en.wikipedia.org/wiki/Immanuel_Kant) and the superb refinements by [Arthur Schopenhauer](https://en.wikipedia.org/wiki/Arthur_Schopenhauer), philosophers have widely agreed on the fact that cognitive systems can never know "the world as such", but only the world as it appears to them through sensorimotor interactions. For example, in some situations, if you spread your arm repeatedly, and if you consistently experience the same sensorimotor pattern, you may infer that there is something constant out there that always makes this same sensorimotor pattern possible. Note that regularities can be experienced through arbitrarily complex instruments, which may range from a stick in your hand to complex experimental settings such as those used by physicists to interact with something out there known as the Higgs boson. These philosophical ideas translate into AI when we acknowledge the fact that knowledge is constructed from regularities of interactions rather than recorded from input data. Designing a system that would construct complete knowledge of the world out there and exploit this model for the better is a part of AI's long-term objective.

To take this problem gradually, this part begins with implementing an agent that can detect simple sequential regularities and exploit them to satisfy its rudimentary motivational system.

## Learning regularities of interaction

Figure 32 presents the principles of a rudimentary system that learns and exploits two-step regularities of interaction.

![Fig.32](/media/032-1.png "Figure 32")

On time step t, the agent enacts the interaction it = ⟨et,rt⟩. Enacting it means experimenting et and receiving a result rt (Page 21). The agent records the two-step sequence ⟨it-1,it⟩ made by the previously enacted interaction it-1 and of it. The sequence of interactions ⟨it-1,it⟩ is called a composite interaction. it-1 is called ⟨it-1,it⟩'s pre-interaction, and it is called ⟨it-1,it⟩'s post-interaction. From now on, low-level interactions i = ⟨e,r⟩ will be called primitive interactions to differentiate them from composite interactions.

The enacted primitive interaction it activates previously learned composite interactions when it matches their pre-interaction. For example, if it = a and if the composite interaction ⟨a,b⟩ has been learned before time t, then the composite interaction ⟨a,b⟩ is activated, meaning it is recalled from memory. Activated composite interactions propose their post-interaction's experiment, in this case: b's experiment. If the sequence ⟨a,b⟩ corresponds to a regularity of interaction, then it is probable that the sequence ⟨a,b⟩ can be enacted again. Therefore, the agent can anticipate that performing b's experiment will likely produce b's result. The agent can thus base its choice of the next experiment on this anticipation.

Note that the enacted primitive interaction it may activate more than one composite interaction, each of them proposing different experiments. We create an interactionally motivated agent by implementing a decision mechanism that uses the agent's capacity of anticipation to choose experiments that will likely result in interactions that have a positive valence, and avoid experiments that will likely result in interactions that have a negative valence.

## Algorithm for learning regularities of interaction

Here is a rudimentary interactionally motivated algorithm that enables the agent to learn and exploit two-step regularities of interaction. Table 33-1 presents its main loop, and Tables 33-2 and 33-3 present subroutines.

In Table 33-1, we chose a set of valences and a particular environment to demonstrate this learning mechanism: this agent is pleased when it receives result r2, but it must learn that the environment returns r2 only if it alternates experiments e1 and e2 every second cycle. Your programming activities will consist of experimenting with other valences and other environments.

Table 33-1: Main loop of an interactionally motivated algorithm that learns two-step sequences of interaction.
```
01  createPrimitiveInteraction(e1, r1, -1)
02  createPrimitiveInteraction(e1, r2, 1)
03  createPrimitiveInteraction(e2, r1, -1)
04  createPrimitiveInteraction(e2, r2, 1)
05  while()
06     contextInteraction = enactedInteraction
07     anticipations = anticipate(enactedInteraction)
08     experiment = selectExperiment(anticipations)

09     if (experiment = previousExperiment)
10        result = r1
11     else
12        result = r2
13     previousExperiment = experiment

14     enactedInteraction = getInteraction(experiment, result)
15     if (enactedInteraction.valence ≥ 0)
16        mood = PLEASED
17     else
18        mood = PAINED
19     learnCompositeInteraction(contextInteraction, enactedInteraction)

```

Table 33-1, lines 01 to 04 initialize the primitive interactions (similar to Page 23) to specify the agent's preferences. In this particular configuration, interactions whose result is r1 have a negative valence, and interactions whose result is r2 have a positive valence. 06: the previously enacted interaction is memorized as the context interaction. 07: computes anticipations in the context of the previous enacted interaction. 08: selects an experiment from the anticipations.

Lines 09 to 13 implement the environment. This new environment was designed to demonstrate the benefit of learning two-step regularities of interaction. If the experiment equals the previous experiment then result is r1, otherwise the result is r2.

Lines 14 to 18: the enacted interaction is retrieved from memory; and the agent is pleased if its valence is positive (similar to Page 23). Line 19: the agent records the composite interaction as a tuple ‹contextInteraction, enactedInteraction› in memory.

Table 33-2 presents a simple version of the learnCompositeInteraction(), anticipate(), and selectExperiment() functions.

Table 33-2: Pseudocode of a simple version.
```
01   function learnCompositeInteraction(contextInteraction, enactedInteraction)
02      compositeInteraction = create new tuple(contextInteraction, enactedInteraction)
03      if compositeInteraction already in the list of known interactions 
04         do nothing
05      else
06         add compositeInteraction to the list of known interactions

10   function anticipate(enactedInteraction)
11      for each interaction in the list of known interactions
12          if interaction.preInteraction = enactedInteraction
13             create new anticipation(interaction.postInteraction)
14      return the list of anticipations

20   function selectExperiment(anticipations)
21      sort the list anticipations by decreasing valence of their proposed interaction.
22      if anticipation[0].interaction.valence ≥ 0
23         return anticipation[0].interaction.experiment
24      else
25         return another experiment than anticipation[0].interaction.experiment

```

The anticipate() function checks for known composite interactions whose pre-interactions match the last enacted primitive interaction; we call these the activated composite interactions. A new object, anticipation, is created for each activated composite interaction. The activated composite interaction's post-interaction is associated with this anticipation as the anticipation's proposed interaction. The selectExperiment() function sorts the list of anticipations by decreasing valence of their proposed interaction. Then, it takes the fist anticipation (index [0]), which has the highest valence in the list. If this valence is positive, then the agent wants to re-enact this proposed interaction, leading to the agent choosing this proposed interaction's experiment.

This solution works in a very simple environment that generates no competing anticipations. However, for environments that may generate competing anticipations, we want the agent to be able to balance competing anticipations based on their probabilities of realization. We may have an environment that, in a given context, makes all the four interactions likely to happen but with different probabilities. For example, in the context in which e1r1 was enacted, both e1 and e2 may result sometimes in r1 and sometimes in r2. But, e1 is more likely to result in r2 than e2. To handle this kind of environment, we associate a weight to composite interactions, as shown in Table 33-3.

Table 33-3: Pseudocode for weighted anticipations.
```
01   function learnCompositeInteraction(contextInteraction, enactedInteraction)
02      compositeInteraction = create new tuple(contextInteraction, enactedInteraction)
03      if compositeInteraction already in the list of known interactions 
04         increment compositeInteraction's weight
05      else
06         add compositeInteraction to the list of known interactions with a weight of 1

10   function anticipate(enactedInteraction)
11      for each interaction in the list of known interactions
12         if interaction.preInteraction = enactedInteraction
13            proposedExperiment = interaction.postInteraction.experiment
14            proclivity = interaction.weight * interaction.postInteraction.valence
15            if an anticipation already exists for proposedExperience 
16               add proclivity to this anticipation's proclivity
17            else
18               create new anticipation(proposedExperiment) with proclivity proclivity
19      return the list of anticipations

20   function selectExperiment(anticipations)
21      sort the list anticipations by decreasing proclivity.
22      if anticipation[0].proclivity ≥ 0
23         return anticipation[0].experiment
24      else
25         return another experiment than anticipation[0].experiment

```

Now, the learnCompositeInteraction() function either records or reinforces composite interactions. The anticipate() function generates an anticipation for each proposed experiment. Anticipations have a proclivity value computed from the weight of the proposing activated composite interaction multiplied by the valence of the proposed interaction. As a result, the anticipations that are the most likely to result in the primitive interaction that have the highest valence receive the highest proclivity. In the example above, in the context when e1r1 has been enacted, the agent learns to choose e1 because it will more likely result in a positive interaction than e2.

## Behavioral analysis of a rudimentary constructivist agent

Table 35 shows the trace that you should see in your console if you ran Project 3. If you did not run it, you can refer to the pseudocode presented in Tables 33-1 and 33-2 to understand this trace.

Table 35: Activity trace of a rudimentary interactionally motivated regulartity learning agent.
```
Enacted e1r2,1
0: PLEASED
Enacted e1r1,-1
learn e1r2e1r1
1: PAINED
Enacted e1r1,-1
learn e1r1e1r1
2: PAINED
afforded e1r1,-1
Enacted e2r2,1
learn e1r1e2r2
3: PLEASED
Enacted e1r2,1
learn e2r2e1r2
4: PLEASED
afforded e1r1,-1
Enacted e2r2,1
learn e1r2e2r2
5: PLEASED

```

## Errata

It is crucial to pay attention to the replacement of environments to create pleasure or dissatisfaction with a set of experiments and their results:

- Change Existence030 to instantiate Environment010 instead of Environment030 and run it. Observe that the modified Existence030 also learns to get pleased when it implements Environment010 instead of Environment030.
- Change Main.java to instantiate Existence031 and run it. Observe that it learns to be pleased in Environment031.
- Change Existence031 to instantiate Environment010 and then Environment030 and run it. Observe that the modified Existence031 also learns to be pleased when it implements Environment010, Environment030, and Environment031.

## Introduction to self-programming

Now things get *really* interesting.