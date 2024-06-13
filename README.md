# EX1-Drone-Mapping
 
# Autonomous Robotics - Ex1: Drone Simulator
This project comprises two main parts:
1.	Developing a 2D drone simulator based on specified physical rules.
2.	Implementing an algorithm and a PID controller to enable the drone to explore a world efficiently without colliding with walls.
## How to Use
Install Dependencies
This project requires the following libraries:
```sh
```
> **NOTE**: check **requiremets.txt**
## Running the Project
To run the project, use the following commands:
```sh
```
## Project Implementation

<b> Drone Simulator </b>

We chose Unity for the drone simulator due to its robust capabilities in simulating objects with physical properties. 

Unity provides an ideal environment for testing and visualizing the drone's behavior under various conditions.

<b> Control Algorithm and PID Controller </b>

Our control strategy involves the drone "hugging" the right wall. When the drone encounters a tunnel (being close to both left and right walls), it positions itself in the center. The PID controller plays a crucial role in maintaining a safe distance from the right wall, and preventing crashes. It also assists in determining when and how much the drone should turn. 

Additionally, the algorithm adjusts the drone's speed based on its proximity to obstacles. If the path ahead is clear, the drone speeds up and stays steady at the max speed. Conversely, if it's near a wall, it slows down or stops. Intermediate distances result in speed adjustments proportional to the distance.


## Physics of the Simulator


•	World Scale: Each pixel represents 2.5 centimeters.

•	Drone Specifications:

&emsp; o	Radius: 10 centimeters

&emsp; o	Lidar Sensors: 4 (forward, backward, left, right)

&emsp; o	Sensor Range: Up to 3 meters

&emsp; o	Sensor Sampling Rate: 10 times per second

&emsp; o	Maximum Speed: 3 meters per second

&emsp; o	Acceleration: 1 meter per second²

&emsp; o	Maximum Rotation: 100 degrees per second

## Visuals

