![WhatsApp Image 2020-11-04 at 08 44 05](https://user-images.githubusercontent.com/57855070/98078036-f4b04180-1e79-11eb-9bde-48b3d32a201f.jpeg)
# EX1-Drone-Mapping
This project was done as part of the "Autonomous-robots" course at Ariel University.

You can see the assignment at the following link: 

https://docs.google.com/document/d/1eo34T_M7jfduRZm_oevy94YY2LkGLzRT/edit?usp=sharing&ouid=113711744349547563645&rtpof=true&sd=true


Before we start detailing we wanted to show you what the simulation looks like:
![2024-06-1719-07-44-ezgif com-crop](https://github.com/Autonomous-robots-robota/EX1-Drone-Mapping/assets/57872024/222f7117-61fc-4ffc-a0e2-5b2e40ebc7de)

you can run it yourself in our itch.io page: https://rob-robota.itch.io/drone-mapping


This project comprises two main parts:
1.	Developing a 2D drone simulator based on specified physical rules.
2.	Implementing an algorithm and a PID controller to enable the drone to explore a world efficiently without colliding with walls.

## Project Implementation

<b> Drone Simulator </b>

We chose Unity for the drone simulator due to its robust capabilities in simulating objects with physical properties. 

Unity provides an ideal environment for testing and visualizing the drone's behavior under various conditions.

<b> Control Algorithm and PID Controller </b>

Given information from distance sensors in 4 directions, 

the drone will decide on its progress

The progress of the drone was reduced to 2 components:

1.	Forward progress (Pitch)
2.	Rotation (YAW)
   
The drone's forward progress is calculated as follows:

Given a maximum speed, the speed range is set between 0 and the maximum speed.

so that the current speed is <b> proportional </b> to the information received from the front sensor,

When the forward distance is equal to the defined emergency distance, the drone doesn't move,

When the forward distance is completely free, i.e. equal to the sensor range, the drone will move at maximum speed

Proportion formula is defined like this-

$\displaystyle \frac{forward\ distance\ -\ emergency\ distance}{sensor\ range\ -\ emergency\ distance}$

The rotation of the drone is calculated as follows:

The drone is in one of two modes: 
1. sticking to the right
2. sticking to the left.
   
When the drone approaches a front wall, that is, a short-range front sensor, the drone will turn against the current clinging direction.

When the drone is in the tunnel meaning the side sensors are shorter than a given value - 
the drone will keep an equal distance from both sides by using <b> PID controller </b>

otherwise the drone will keep a set distance from the wall on the side that matches its condition by using <b> PID controller </b>.

Switching between the modes takes place under the following conditions:
1.	Switch between modes every five seconds.
2.	in detecting arrival at the intersection.
The drone defines arrival at the intersection when it exits a tunnel and one of the side sensors is completely free - it is equal to the range of the sensor

In addition the acceleration of the drone is limited by a value defined as required

## Physics of the Simulator

•	World Scale: Each pixel represents 2.5 cm.

•	Drone Specifications:

&emsp; o	Radius: 10 cm

&emsp; o	Lidar Sensors: 4 (forward, backward, left, right)

&emsp; o	Sensor Range: Up to 3 meters

&emsp; o	Sensor Sampling Rate: 10 times per second

&emsp; o	Maximum Speed: 3 meters per second

&emsp; o	Acceleration: 1 meter per second²

&emsp; o	Maximum Rotation: 100 degrees per second

## Visuals

![צילום מסך 2024-06-16 225106](https://github.com/Autonomous-robots-robota/EX1-Drone-Mapping/assets/57872024/2558ea88-8835-4bbb-9dd1-12a5b3639141)

![צילום מסך 2024-06-17 115808](https://github.com/Autonomous-robots-robota/EX1-Drone-Mapping/assets/57872024/39947a1c-de57-4819-9fc0-0eb7d9bba2fe)

![צילום מסך 2024-06-16 230025](https://github.com/Autonomous-robots-robota/EX1-Drone-Mapping/assets/57872024/56c182a1-5ece-4bc9-9127-23e5474eccbf)

![צילום מסך 2024-06-16 230919](https://github.com/Autonomous-robots-robota/EX1-Drone-Mapping/assets/57872024/a29c9c6e-c107-4b57-8917-49487a260357)

![צילום מסך 2024-06-17 120556](https://github.com/Autonomous-robots-robota/EX1-Drone-Mapping/assets/57872024/37c591ed-8768-44e2-819e-28ff4800b9d0)


