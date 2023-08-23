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

In his theory of mental development, (Jean Piaget)[https://en.wikipedia.org/wiki/Jean_Piaget] coined the term sensorimotor scheme to refer to a pattern of interaction between the agent and its environment. In our model, an interaction is a chunk of data that represents a primitive sensorimotor scheme. From now on, we use the expression "to enact an interaction" to refer to performing the experiment and receiving the result that compose a given interaction. The expression "to intend to enact" interaction ⟨e,r⟩ means that the agent performs experiment e while expecting result r. As a result of this intention, the agent may "actually enact" interaction ⟨e,r'⟩ if it receives result r' instead of r.

The sensorimotor paradigm allows implementing a type of motivation called interactional motivation. Using the embodied model introduced on Page 12, Figure 22 compares traditional reinforcement learning (left) with interactional motivation (right).

![Fig.12](/media/022-1.png "Figure 22") 

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

