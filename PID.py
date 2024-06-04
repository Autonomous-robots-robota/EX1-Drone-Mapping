import numpy as np

# Define the wall positions
walls = [
    {'position': np.array([0, 0]), 'type': 'left'},
    {'position': np.array([4, 4]), 'type': 'right'},
    {'position': np.array([2, 2]), 'type': 'forward'},
    {'position': np.array([1, 0]), 'type': 'backward'}
]

# Initial drone position
output_drone = np.array([1.0, 1.0])

# PID constants
speed = 5
pGain = 1.0  # Proportional gain
iGain = 0.0  # Integral gain
dGain = 0.0  # Derivative gain
fixedDeltaTime = 1.0  # Time step
max_distance_from_wall = 3
# Initialize error terms
integral = np.array([0.0, 0.0])
previous_error = np.array([0.0, 0.0])


def cheack_if_the_distance_is_max(output_drone, wall, max_distance_from_wall):
    direction = output_drone - wall
    distance = np.linalg.norm(direction)
    if distance == max_distance_from_wall:
        return True
    else:
        return False


def calculate_repulsive_forces(output_drone, walls):
    total_force = np.array([0.0, 0.0])
    for wall in walls:
        wall_position = wall['position']
        direction = output_drone - wall_position
        distance = np.linalg.norm(direction)
        if distance != 0:
            force_magnitude = 1 / distance  # The closer the wall, the stronger the repulsive force
            force = (direction / distance) * force_magnitude
            total_force += force
    return total_force


def calculate_error(repulsive_force):
    error = -repulsive_force  # We want to move away from the walls
    return error


def update(integral, previous_error, error):
    proportional = pGain * error

    # Integral term
    integral += error * fixedDeltaTime
    integral_term = iGain * integral

    # Derivative term
    derivative = (error - previous_error) / fixedDeltaTime
    derivative_term = dGain * derivative

    # Combine terms
    pid_output = proportional + integral_term + derivative_term

    # Update the previous error
    previous_error = error

    # Calculate velocity
    if np.linalg.norm(pid_output) != 0:
        direction = pid_output / np.linalg.norm(pid_output)
    else:
        direction = np.array([0.0, 0.0])
    velocity = direction * speed

    return velocity, previous_error, integral

# Simulation loop
for _ in range(100):  # Simulate for 100 time steps

    repulsive_force = calculate_repulsive_forces(output_drone, walls)
    error = calculate_error(repulsive_force)

    velocity, previous_error, integral = update(integral, previous_error, error)

    # Update drone position
    output_drone += velocity * fixedDeltaTime

    # check if we have split in the way:
    move_way = 'forward'
    for wall in walls:
        if cheack_if_the_distance_is_max(output_drone=output_drone, wall=wall['position'], max_distance_from_wall=max_distance_from_wall):
            if max_way != 'right':# if there is two way- chose the right.
                max_way = wall['type']


    print(f"Drone Position: {output_drone}, Velocity: {velocity}")
    print(f"Drone way: {move_way}")

