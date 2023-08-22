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