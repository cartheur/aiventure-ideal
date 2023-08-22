# aiventure-ideal

Implementation of DEvelopmentAl Learning (IDEAL) Course

## Introduction to the embodied paradigm

The main message that we want to convey is:

Do not consider the agent's input data as the agent's perception of its environment.

The agent is not a passive observer of reality, but rather constructs a perception of reality through active interaction. The term embodied means that the agent must be a part of reality for this active interaction to happen.

Those of you who have a background in cognitive science or psychology are probably already familiar with this idea theoretically. In Lesson 1, however, we wish to introduce how this idea translates into the practical design of artificial agents and robots.

## Agent and robot design according to the embodied paradigm
The embodied paradigm suggests shifting perspective from:

- the traditional view in which the agent interprets input data as if it represented the environment (Figure 12/left),
to:
- the embodied view in which the agent constructs a perception of the environment through the active experience of interaction (Figure 12/right).

![Fig.12](/media/012-1.png "Figure 12?")

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