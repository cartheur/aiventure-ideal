# aiventure-ideal

`I`mplemented `DE`velopment`A`l `L`earning - `IDEAL` - for machines.

## Introduction to the embodied paradigm

The *embodied* paradigm operates from this foundational statement:

**Do not consider the agent's input data as the agent's perception of its environment.**

The agent is not a passive observer of reality, but rather constructs a perception of reality through active interaction. The term embodied means that the agent must be a part of reality for this active interaction to happen. Those of you who have a background in cognitive science or psychology are probably already familiar with this idea theoretically. In Lesson 1, however, we wish to introduce how this idea translates into the practical design of artificial agents and robots.

## Agent and robot design according to the embodied paradigm

The embodied paradigm suggests shifting perspective from two distinct views:

- the traditional view in which the agent interprets input data as if it represented the environment (Figure 12/left),
to:
- the embodied view in which the agent constructs a perception of the environment through the active experience of interaction (Figure 12/right).

![Fig.12](/media/012-1.png "Figure 12")

Most representations of the cycle agent/environment do not make explicit the conceptual starting point and end point of the cycle. Since the cycle revolves indefinitely, why should we care anyway?

We should care because, depending on the conceptual starting and end points, we design the agent's algorithm, the robot's sensors, or the simulated environment differently.

In the traditional view, we design the agent's input (called observation o in Figure 12/left) as if it represented the environment's state. In the case of simulated environments, we implement o as a function of s, where s is the state of the environment (o = f (s) in Figure 12/left). In the case of robots, we process the sensor data as if it represented the state of the real world, even though this state is not accessible. This is precisely what the embodied paradigm suggests to avoid because it amounts to considering the agent's input as a representation of the world.

In the embodied view, we design the agent's input (called result r in Figure 12/right) as a result of an experiment initiated by the agent. In simulated environments, we implement r as a function of the experiment and of the state (r = f (e,s) in Figure 12/right). In a given state of the environment, the result may vary according to the experiment. We may even implement environments that have no state, as we do in the next page. When designing robots, we process the sensor data as representing the result of an experiment initiated by the robot. This is illustrated in the next sections.

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
01    Enacted e1r2,1
02    0: PLEASED
03    Enacted e1r1,-1
04    learn e1r2e1r1
05    1: PAINED
06    Enacted e1r1,-1
07    learn e1r1e1r1
08    2: PAINED
09    afforded e1r1,-1
10    Enacted e2r2,1
11    learn e1r1e2r2
12    3: PLEASED
13    Enacted e1r2,1
14    learn e2r2e1r2
15    4: PLEASED
16    afforded e1r1,-1
17    Enacted e2r2,1
18    learn e1r2e2r2
19    5: PLEASED

```

## Errata

It is crucial to pay attention to the replacement of environments to create pleasure or dissatisfaction with a set of experiments and their results:

- Change Existence030 to instantiate Environment010 instead of Environment030 and run it. Observe that the modified Existence030 also learns to get pleased when it implements Environment010 instead of Environment030.
- Change Main.java to instantiate Existence031 and run it. Observe that it learns to be pleased in Environment031.
- Change Existence031 to instantiate Environment010 and then Environment030 and run it. Observe that the modified Existence031 also learns to be pleased when it implements Environment010, Environment030, and Environment031.

## Introduction to self-programming

*Defining self-programming*

We define a self-programming agent as an agent that can autonomously acquire executable code and re-execute this code appropriately.

Similar to other machine learning agents, self-programming agents record data in memory as they learn. Traditional machine learning agents, however, run a predefined program that exploits this data as parameters. Self-programming agents also run a predefined program, but this program can control the execution of learned data as sequences of instructions.

To understand the full implication of this definition, it is important to take a cognitive science perspective rather than a software development perspective. A natural cognitive system (an animal) does not have a compiler or an interpreter to exploit a programming language. The only thing at its disposal that remotely resembles an instruction set is the set of interaction possibilities it has with the world around it. The only thing at its disposal that remotely resembles an execution engine is its cognitive system which allows it to execute and learn sequences of interactions with the world.

We draw inspiration from natural cognitive systems to design self-programming agents. Like biological systems, these agents program themselves using the instruction set at their disposal (the set of interaction possibilities they have with the world), and the execution engine at their disposal (their cognitive system that executes and learns sequences of interactions with the world).

*Why self-programming is important?*

There are profound theoretical reasons why self-programming is decisive to achieve artificial intelligence. Page 45 will refer you to some articles that elaborate on these reasons, specifically coming from the *theory of enaction*.

Nevertheless, we can already give a simple and intuitive answer. If we build two identical robots, it would be no fun if these two robots generated similar behaviors in the same circumstances. As they develop, we would like them to assess situations differently, make different choices, and carry out different behaviors. Only when we see these significant behavioral differences emerge will we be willing to consider each robot as an intelligent being as opposed to a mere automaton.

Since assessing the situation, making choices, and carrying out behaviors is the role of programs, we will only manage to make significant behavioral differences emerge autonomously if the robot can program itself autonomously. Of course, defining "significant behavioral differences" is challenging. Here is a video explaining this concept:

![Vid.01](/media/Rudimentary%20self-programming.mp4 "Video 01")

Self-programming raises many questions.

*Self-programming to what end?*

An obvious question is why the system should choose to learn a specific program rather than another. We must implement driving principles to give a direction to the system's development, without specifying a final goal so that the system can keep learning new programs indefinitely.

Interactional motivation, presented in Lesson 2, provides us with a possible answer to this challenge because it allows us to specify inborn behavioral preferences without specifying a predefined goal. An interactionally motivated self-programming agent will choose to learn programs that can help it enact interactions that have a high positive valence, while avoiding situations where interactions with a negative valence are likely to happen.

Interactional motivation is not the only principle that can drive self-programming, but it is the approach we will continue using in the examples in Lesson 4.

*Where do the learned programs come from?*

Constructivist epistemology, presented in Lesson 3, provides us with a possible answer to this question: the learned programs can come from the same place where all of the agent's knowledge comes from: regularities of interaction. This leads us to this section's key concept:

Self-programming consists of the re-enaction of regularities of interaction.

## Exploiting regularities through self-programming

We already introduced the problem of learning regularities on Page 32. There was, however, no self-programming in this exemple because the system could not re-enact the learned regularities as full sequences of interactions. Figure 42 builds upon the regularity learning mechanism to present our design principles for self-programming.

![Fig.42](/media/042-1.png "Figure 42")

Figure 42: 1) The time line at the bottom represents the stream of interactions that occur over time as the system interacts. Symbols in this time line represent enacted interactions as in the bottom line of Figure 32. 2) The agent finds episodes of interest made of sequences of interactions. The symbols above Line 1 represent episodes delimited by curly brackets. These episodes are learned hierarchically in a bottom-up way; higher-level episodes are made of sequences of lower-level episodes. 3) At a certain level of abstraction (white vertical half-circle), the current sequence of episodes matches previously learned sequences and re-activates them. 4) Re-activated sequences propose subsequent episodes. These are, thus, the episodes that are afforded by the current context. 5) Afforded episodes are categorized as experiments (gray symbols). These experiments are proposed for selection. 6) The agent chooses an experiment from amongst the proposed experiments (gray arrow). 7) The agent tries to enact the sequence of primitive interactions that correspond to the chosen experiment. The success or failure of this tentative enaction depends on the environment. If the activated sequences do indeed represent a regularity of interaction, then it is probable that the tentative enaction will succeed (white arrow). However, it is not certain.

The self-programming effect occurs when the chosen experiment corresponds to a composite interaction. In this case, the decision engages the agent into executing several steps of interaction.

Self-programming results in a bottom-up automatization of behaviors so that the agent constructs increasingly abstract behaviors and delegates the control of their enaction to an automatic subsystem of its cognitive system. The agent can focus on the abstract behavior (Decision Time arrow in Figure 42) which helps it to deal with the complexity of its environment and recursively construct even higher levels of abstraction.

The agent represents its current context in terms of previously learned abstract episodes of interaction. This amounts to modeling the environment in terms of abstract affordances, as, for example, Gibson suggests in his theory of affordances.

This learning process is incremental and open-ended; it only stops when it runs out of memory for recording new sequences. Memory could be optimized, for example, by deleting (forgetting) sequences that have not been used for a while, but we did not implement this for the sake of simplicity. More fundamentally, regularities should be used to construct a coherent model of the world; we will examine this issue further in section six.

## Architecture of a recursive self-programming agent

By building upon the regularity learning algorithm presented on Page 33, Table 43 presents our first algorithm for self-programming agents.

Table 43: Algorithm of a recursive self-programming agent.

```
001   createPrimitiveInteraction(e1, r1, -1)
002   createPrimitiveInteraction(e1, r2, 1)
003   createPrimitiveInteraction(e2, r1, -1)
004   createPrimitiveInteraction(e2, r2, 1)
005   while()
006      anticipations = anticipate()
007      experiment = selectExperiment(anticipations)
008      intendedInteraction = experiment.intendedInteraction()

009      enactedInteraction = enact(intendedInteraction)

010      learn()
011      if (enactedInteraction.valence ≥ 0)
012         mood = PLEASED
013      else
014         mood = PAINED

101   function enact(intendedInteraction)
102      if intendedInteraction.isPrimitive 
103         experiment = intendedInteraction.experiment
104         result = environment.getResult(experiment)
105         return primitiveInteraction(experiment, result)
106      else
107         enactedPreInteraction = enact(intendedInteraction.preInteraction)
108         if (enactedPreInteraction ≠ intendedInteraction.preInteraction)
109            return enactedPreInteraction
110         else
111            enactedPostInteraction = enact(intendedInteraction.postInteraction)
112            return getOrLearn(enactedPreInteraction, enactedPostInteraction)

201   function environment.getResult(experiment)
202      if penultimateExperiment ≠ experiment and previousExperiment = experiment
203         return r2
204      else
205         return r1

```

Table 43, lines 001-014: The main loop of the algorithm is very similar to that on Page 33. 001-004: Initialization of the primitive interactions. 006: Get the list of anticipations. Now, the anticipate() function also returns anticipations for abstract experiments corresponding to enacting composite interactions. 007: Choose the next experiment from among primitive and abstract experiments in the list of anticipations. 008: Get the intended primitive or composite interaction from the selected primitive or abstract experiment.

Line 009: The enaction of the intended interaction is now delegated to the recursive function enact(intendedInteraction). The intended interaction constitutes the learned program that the agent intends to execute, and the enact() function implements the engine that executes it. Let us emphasize the fact that the agent now chooses an interaction to enact rather than an experiment to perform (as it was the case on Page 33). In return, the agent receives an enacted interaction rather than a result. This design choice follows from constructivist epistemology which suggests that sensorimotor patterns of interaction constitute the basic elements from which the agent constructs knowledge of the world.

Line 010: Learns composite interactions and abstract experiments from the experience gained from enacting the enacted interaction. The learn() function will be further explained on the next page. Lines 011 to 014: like before, specify that the agent is pleased if the enacted interaction's valence is positive, and pained otherwise. The valence of a composite interaction is equal to the sum of the valences of its primitive interactions, meaning that enacting a full sequence of interactions has the same motivational valence as enacting all its elements successively.

Lines 101-116: The function enact(intendedInteraction) is used recursively to control the enaction of the intended interaction all the way down to the primitive interactions. It returns the enacted interaction. 102-105: If the intended interaction is primitive then it is enacted in the environment. 103: Specifies that the experiment is the intended primitive interaction's experiment. 104: The environment returns the result of this experiment. 105: The function returns the enacted primitive interaction made from the experiment and its result. 106-112: Control the enaction of a composite intended interaction. Enacting a composite interaction consists of successively enacting its pre-interaction and its post-interaction. 107: Call itself to enact the pre-interaction. 108-109: If the enacted pre-interaction differs from the intended pre-interaction the enact() function is interrupted and returns the enacted pre-interaction. 110-111: if the enaction of the pre-interaction was a success, then the enact() function proceeds to the enaction of the post-interaction. 112 The function returns the enacted composite interaction made of the enacted pre-interaction and of the enacted post-interaction.

Lines 201-205 implement the environment. This environment is the simplest we could imagine that requires the agent to program itself if it wants to be PLEASED. The result r2 occurs when the current experiment equals the previous experiment but differs from the penultimate experiment, and r1 otherwise. Since r2 is the only result that produces interactions that have a positive valence, and since the agent can at best obtain r2 every second step, it must learn to alternate between two e1 and two e2 experiments: e1→r1 e1→r2 e2→r1 e2→r2 etc. The agent must not base its decision on the anticipation of what it can get in the next step, but on the anticipation of what it can get in the next two steps.

Figure 43 illustrates the architecture of this algorithm.

![Fig.43](/media/043-1.png "Figure 43")

The whole program is called *Existence*. The lower dashed line (Line 2) separates the part of existence corresponding to the agent (above Line 2) from the part corresponding to the environment (below Line 2, implemented by the function environment.getResult(experiment)). The upper dashed line (Line 1) separates the proactive part of existence (above Line 1) from the reactive part of existence (below Line 1). The program that implements the proactive part is called the Decider; you can consider this as the "cognitive part" of the agent. It corresponds to the main loop in Table 43: lines 005 to 014. The reactive part of the existence includes both the Enacter (above Line 2) and the environment (below Line 2). The enacter (function enact()) controls the enaction of the intended interaction by sending a sequence of experiments to the environment. The enacter receives a result after each experiment. After the last experiment, the enacter returns the enacted composite interaction to the decider.

As the agent develops, it constructs abstract possibilities of interaction (composite interactions) that it can enact with reference to the reactive part. From the agent's cognitive point of view (the proactive part), the reactive part appears as an abstract environment (abstracted away from the real environment by the agent itself). Line 1 represents what we call the cognitive coupling between the agent and its environment.

In sections five and six, we will discuss the question of increasing the complexity associated with these different levels of coupling.

## The construction of reality in the developmental agent (A direct connection to Schopenhauer)

*Anticipating the effects of composite interactions*

As showed in the algorithm in Table 33-3, the anticipate() function returns a list of anticipations. Each anticipation corresponds to an experiment associated with a proclivity value for performing this experiment. The proclivity is computed on the basis of the possible interactions that may actually be enacted as an effect of performing this experiment, as far as the system can tell from its experience.

For the anticipate() function to work similarly with composite interactions as it does with primitive interactions, composite interactions must also be associated with experiments. In fact, the system must learn that a composite interaction corresponds to an abstract experiment performed with reference to an abstract environment (the reactive part) that returns an abstract result.

See how this problem fits nicely with the constructivist learning challenge (introduced in the readings on Page 36): learning to interpret sensorimotor interactions as consisting of performing experiments on an external reality, and to interpret the results of these experiments as information about that reality.

The rest of this page presents our first step towards addressing this challenge. We will develop this question further in the next lesson.

*Recursively learning composite interactions*

Figure 44 illustrates the implementation of the learnCompositeInteraction() function so as to implement recursive learning of a hierarchy of composite interactions.

![Fig.44](/media/044-1.png "Figure 44")

Figure 44 distinguishes between the Interaction Time (arrow at the bottom corresponding to the agent/environment coupling) and the Decision Time (staircase shaped arrow corresponding to the proactive/reactive coupling that rises over time). The learning occurs at the level of the Decision Time to learn higher-level composite interactions on top of enacted composite interactions. In gray rectangles indicate the composite interactions that are learned or reinforced at the end of decision cycle td. The system learns the composite interaction ⟨ecd-1,ecd⟩ made of the sequence of the previous enacted composite interaction ecd-1 and the last enacted composite interaction ecd. This is similar to Page 32 except that the learning can apply to composite interactions rather than primitive interactions only. Additionally, the system learns the composite interaction ⟨ecd-2,⟨ecd-1,ecd⟩⟩. This way, if ecd-2 is enacted again, ⟨ecd-2,⟨ecd-1,ecd⟩⟩ will be re-activated and will propose to enact its post-interaction ⟨ecd-1,ecd⟩. The system has thus learned to re-enact ⟨ecd-1,ecd⟩ as a sequence, hence the self-programming effect. The higher-level composite interaction ⟨⟨ecd-2,ecd-1⟩,ecd⟩ is also learned so that it can be re-activated in the context when ⟨ecd-2,ecd-1⟩ is enacted again, and propose its post-interaction ecd.

*Associating abstract experiments and results with composite interactions*

When a new composite interaction ic is added to the set Id of known interactions at time td, a new abstract experiment ea is added to the set Ed of known experiments at time td, and a new abstract result ra is added to the set Rd of known results at time td, such that ic = ⟨ea,ra⟩.

Abstract experiments are called abstract because the environment cannot process them directly. The environment (or robot's interface) is only programmed to interpret a predefined set of experiments that we now call concrete. To perform an abstract experiment ea, the agent must perform a series of concrete experiments and check their results. That is, the agent must try to enact the composite interaction ic from which the abstract experiment ea was constructed.

If the chooseExperiment() function chooses experiment ea, then the system tries to enact ic. If this tentative enaction fails and results in the enacted composite interaction ec ∈ Id+1, then the system creates the abstract result rf ∈ Rd+1, so that ec = ⟨ea,rf⟩ .

The next time the system considers choosing experiment ea, it will compute the proclivity for ea based on the anticipation of succeeding enacting ic and getting result ra, balanced with the anticipation of actually enacting ec and getting result rf.

As a result of this mechanism, composite interactions can have two forms: the sequential form ⟨pre-interaction,post-interaction⟩ and the abstract form ⟨experiment,result⟩. We differentiate between these two forms by noting abstract experiments and results in initial caps separated by the "|" symbol: ⟨EXPERIMENT|RESULT⟩. We will use this notation in the trace in Page 46.

This mechanism is a critical step to implementing self-programming agents. Nevertheless, it opens many questions that remain to be addressed. For example, how to organize experiments and results to construct a coherent model of reality?

## Behavioral analysis of a self-programming agent

Table 46 shows the trace that you should see in your console if you ran Project 4. If you did not run it, you can refer to pages 44 and 45 to understand this trace. Observe that the system learns to be always PLEASED from Decision 23 (on Line 197) by alternatively enacting the composite interactions ⟨e2r1e2r2⟩ and ⟨e1r1e1r2⟩.

Table 46: Activity trace of a rudimentary self-programming agent.

```
001    propose e1 proclivity 0
002    propose e2 proclivity 0
003    Enacted e1r1 valence -1 weight 0
004    0: PAINED
005    propose e1 proclivity 0
006    propose e2 proclivity 0
007    Enacted e1r2 valence 1 weight 0
008    learn <e1r1e1r2> valence 0 weight 1
009    1: PLEASED
010    propose e1 proclivity 0
011    propose e2 proclivity 0
012    Enacted e1r1 valence -1 weight 0
013    learn <e1r2e1r1> valence 0 weight 1
014    learn <e1r1<e1r2e1r1>> valence -1 weight 1
015    learn <<e1r1e1r2>e1r1> valence -1 weight 1
016    2: PAINED
017    propose e1 proclivity 1
018    propose e2 proclivity 0
019    propose <E1R2E1R1| proclivity 0
020    Enacted e1r1 valence -1 weight 0
021    learn <e1r1e1r1> valence -2 weight 1
022    learn <e1r2<e1r1e1r1>> valence -1 weight 1
023    learn <<e1r2e1r1>e1r1> valence -1 weight 1
024    3: PAINED
025    propose e1 proclivity 0
026    propose e2 proclivity 0
027    propose <E1R2E1R1| proclivity 0
028    Enacted e1r1 valence -1 weight 0
029    reinforce <e1r1e1r1> valence -2 weight 2
030    learn <e1r1<e1r1e1r1>> valence -3 weight 1
031    learn <<e1r1e1r1>e1r1> valence -3 weight 1
032    4: PAINED
033    propose e2 proclivity 0
034    propose <E1R2E1R1| proclivity 0
035    propose e1 proclivity -2
036    propose <E1R1E1R1| proclivity -2
037    Enacted e2r1 valence -1 weight 0
038    learn <e1r1e2r1> valence -2 weight 1
039    learn <e1r1<e1r1e2r1>> valence -3 weight 1
040    learn <<e1r1e1r1>e2r1> valence -3 weight 1
041    5: PAINED
042    propose e1 proclivity 0
043    propose e2 proclivity 0
044    Enacted e1r1 valence -1 weight 0
045    learn <e2r1e1r1> valence -2 weight 1
046    learn <e1r1<e2r1e1r1>> valence -3 weight 1
047    learn <<e1r1e2r1>e1r1> valence -3 weight 1
048    6: PAINED
049    propose <E1R2E1R1| proclivity 0
050    propose e1 proclivity -1
051    propose e2 proclivity -1
052    propose <E2R1E1R1| proclivity -2
053    propose <E1R1E1R1| proclivity -2
054    propose <E1R1E2R1| proclivity -2
055    Enacted <e1r2e1r1> valence 0 weight 1
056    reinforce <e1r1<e1r2e1r1>> valence -1 weight 2
057    7: PLEASED
058    propose <E1R2E1R1| proclivity 0
059    propose e2 proclivity -1
060    propose e1 proclivity -2
061    propose <E2R1E1R1| proclivity -2
062    propose <E1R1E1R1| proclivity -2
063    propose <E1R1E2R1| proclivity -2
064    Enacted e1r1 valence -1 weight 0
065    learn <<e1r2e1r1><E1R2E1R1|E1R1>> valence -1 weight 1
066    8: PAINED
067    propose e1 proclivity 0
068    propose e2 proclivity 0
069    Enacted e1r1 valence -1 weight 0
070    learn <<E1R2E1R1|E1R1>e1r1> valence -2 weight 1
071    learn <<e1r2e1r1><<E1R2E1R1|E1R1>e1r1>> valence -2 weight 1
072    learn <<<e1r2e1r1><E1R2E1R1|E1R1>>e1r1> valence -2 weight 1
073    9: PAINED
074    propose <E1R2E1R1| proclivity 0
075    propose e1 proclivity -1
076    propose e2 proclivity -1
077    propose <E2R1E1R1| proclivity -2
078    propose <E1R1E1R1| proclivity -2
079    propose <E1R1E2R1| proclivity -2
080    Enacted e1r1 valence -1 weight 0
081    learn <e1r1<E1R2E1R1|E1R1>> valence -2 weight 1
082    learn <<E1R2E1R1|E1R1><e1r1<E1R2E1R1|E1R1>>> valence -3 weight 1
083    learn <<<E1R2E1R1|E1R1>e1r1><E1R2E1R1|E1R1>> valence -3 weight 1
084    10: PAINED
085    propose e2 proclivity 0
086    propose e1 proclivity -1
087    propose <E1R1<E1R2E1R1|E1R1|| proclivity -2
088    Enacted e2r1 valence -1 weight 0
089    learn <<E1R2E1R1|E1R1>e2r1> valence -2 weight 1
090    learn <e1r1<<E1R2E1R1|E1R1>e2r1>> valence -3 weight 1
091    learn <<e1r1<E1R2E1R1|E1R1>>e2r1> valence -3 weight 1
092    11: PAINED
093    propose e2 proclivity 0
094    propose e1 proclivity -1
095    Enacted e2r2 valence 1 weight 0
096    learn <e2r1e2r2> valence 0 weight 1
097    learn <<E1R2E1R1|E1R1><e2r1e2r2>> valence -1 weight 1
098    learn <<<E1R2E1R1|E1R1>e2r1>e2r2> valence -1 weight 1
099    12: PLEASED
100    propose e1 proclivity 0
101    propose e2 proclivity 0
102    Enacted e1r1 valence -1 weight 0
103    learn <e2r2e1r1> valence 0 weight 1
104    learn <e2r1<e2r2e1r1>> valence -1 weight 1
105    learn <<e2r1e2r2>e1r1> valence -1 weight 1
106    13: PAINED
107    propose e1 proclivity -1
108    propose e2 proclivity -1
109    propose <E1R2E1R1| proclivity -1
110    propose <<E1R2E1R1|E1R1|E2R1| proclivity -2
111    propose <E2R1E1R1| proclivity -2
112    propose <E1R1E1R1| proclivity -2
113    propose <E1R1E2R1| proclivity -2
114    Enacted e1r2 valence 1 weight 0
115    reinforce <e1r1e1r2> valence 0 weight 2
116    learn <e2r2<e1r1e1r2>> valence 1 weight 1
117    learn <<e2r2e1r1>e1r2> valence 1 weight 1
118    14: PLEASED
119    propose e2 proclivity 0
120    propose e1 proclivity -2
121    propose <E1R1E1R1| proclivity -2
122    Enacted e2r1 valence -1 weight 0
123    learn <e1r2e2r1> valence 0 weight 1
124    learn <e1r1<e1r2e2r1>> valence -1 weight 1
125    learn <<e1r1e1r2>e2r1> valence -1 weight 1
126    15: PAINED
127    propose e2 proclivity 1
128    propose <E2R2E1R1| proclivity 0
129    propose e1 proclivity -1
130    Enacted e2r2 valence 1 weight 0
131    reinforce <e2r1e2r2> valence 0 weight 2
132    learn <e1r2<e2r1e2r2>> valence 1 weight 1
133    learn <<e1r2e2r1>e2r2> valence 1 weight 1
134    16: PLEASED
135    propose e2 proclivity 0
136    propose <E1R1E1R2| proclivity 0
137    propose e1 proclivity -2
138    Enacted e2r1 valence -1 weight 0
139    learn <e2r2e2r1> valence 0 weight 1
140    learn <e2r1<e2r2e2r1>> valence -1 weight 1
141    learn <<e2r1e2r2>e2r1> valence -1 weight 1
142    17: PAINED
143    propose e2 proclivity 2
144    propose <E2R2E2R1| proclivity 0
145    propose <E2R2E1R1| proclivity 0
146    propose e1 proclivity -1
147    Enacted e2r1 valence -1 weight 0
148    learn <e2r1e2r1> valence -2 weight 1
149    learn <e2r2<e2r1e2r1>> valence -1 weight 1
150    learn <<e2r2e2r1>e2r1> valence -1 weight 1
151    18: PAINED
152    propose e2 proclivity 1
153    propose <E2R2E2R1| proclivity 0
154    propose <E2R2E1R1| proclivity 0
155    propose e1 proclivity -1
156    Enacted e2r1 valence -1 weight 0
157    reinforce <e2r1e2r1> valence -2 weight 2
158    learn <e2r1<e2r1e2r1>> valence -3 weight 1
159    learn <<e2r1e2r1>e2r1> valence -3 weight 1
160    19: PAINED
161    propose <E2R2E2R1| proclivity 0
162    propose <E2R2E1R1| proclivity 0
163    propose e1 proclivity -1
164    propose e2 proclivity -1
165    propose <E2R1E2R1| proclivity -2
166    Enacted e2r1 valence -1 weight 0
167    learn <e2r1<E2R2E2R1|E2R1>> valence -2 weight 1
168    learn <e2r1<e2r1<E2R2E2R1|E2R1>>> valence -3 weight 1
169    learn <<e2r1e2r1><E2R2E2R1|E2R1>> valence -3 weight 1
170    20: PAINED
171    propose e1 proclivity 0
172    propose e2 proclivity 0
173    Enacted e1r1 valence -1 weight 0
174    learn <<E2R2E2R1|E2R1>e1r1> valence -2 weight 1
175    learn <e2r1<<E2R2E2R1|E2R1>e1r1>> valence -3 weight 1
176    learn <<e2r1<E2R2E2R1|E2R1>>e1r1> valence -3 weight 1
177    21: PAINED
178    propose e1 proclivity 0
179    propose <E1R2E2R1| proclivity 0
180    propose e2 proclivity -1
181    propose <E1R2E1R1| proclivity -1
182    propose <E2R1E1R1| proclivity -2
183    propose <<E1R2E1R1|E1R1|E2R1| proclivity -2
184    propose <E1R1E2R1| proclivity -2
185    propose <E1R1E1R1| proclivity -2
186    Enacted e1r2 valence 1 weight 0
187    reinforce <e1r1e1r2> valence 0 weight 3
188    learn <<E2R2E2R1|E2R1><e1r1e1r2>> valence -1 weight 1
189    learn <<<E2R2E2R1|E2R1>e1r1>e1r2> valence -1 weight 1
190    22: PLEASED
191    propose <E2R1E2R2| proclivity 0
192    propose e1 proclivity -2
193    propose e2 proclivity -2
194    propose <E1R1E1R1| proclivity -2
195    Enacted <e2r1e2r2> valence 0 weight 2
196    reinforce <e1r2<e2r1e2r2>> valence 1 weight 2
197    23: PLEASED
198    propose <E1R1E1R2| proclivity 0
199    propose e1 proclivity -2
200    propose e2 proclivity -2
201    propose <E2R1E2R1| proclivity -2
202    Enacted <e1r1e1r2> valence 0 weight 3
203    learn <<e2r1e2r2><e1r1e1r2>> valence 0 weight 1
204    24: PLEASED
205    propose <E2R1E2R2| proclivity 0
206    propose e1 proclivity -2
207    propose e2 proclivity -2
208    propose <E1R1E1R1| proclivity -2
209    Enacted <e2r1e2r2> valence 0 weight 2
210    learn <<e1r1e1r2><e2r1e2r2>> valence 0 weight 1
211    25: PLEASED

```

Lines 1-2: predefined experiments are proposed with a default proclivity of 0. Proposals are sorted by decreasing proclivity. The first proposed experiment is selected: e1. Lines 3-4: Decision 0: The interaction e1r1 is enacted. The system is PAINED because this interaction has a negative valence (-1).

Line 8: the first composite interaction ⟨e1r1e1r2⟩ is learned from the primitive interaction e1r1 enacted on Decision 0 (Line 3) and the primitive interaction e1r2 enacted on Decision 1 (Line 7). Simultaneously, the system records an abstract experiment noted ⟨E1R1E1R2| (not in the trace). This experiment will be proposed for the first time on Decision 16 (Line 136) but not selected. It is proposed again on Decision 24 (Line 198) and selected, resulting in the successful enaction of composite interaction ⟨e1r1e1r2⟩ (Line 202).

On Decision 8, the experiment ⟨E1R2E1R1| was selected (Line 58), leading to the tentative enaction of composite interaction ⟨e1r2e1r1⟩. This tentative enaction failed due to obtaining result r1 instead of the expected result r2, thus resulting in the enaction of primitive interaction e1r1 instead (Line 64). The abstract result |E1R1⟩ is created, as well as the enacted interaction ⟨E1R2E1R1|E1R1⟩, using the notation introduced on Page 44.

## Introduction to radical interactionism

