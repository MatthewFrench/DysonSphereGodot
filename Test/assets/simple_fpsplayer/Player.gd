extends RigidBody
#Variables
var global = "root/global"

var X = 0.00
var Y = 0.00

var speed = 25
var moving = true
var MOUSESPEED = 0.005
var JUMP_VEL = 12
var jumping = false
var relativeMouseX = null;

onready var playerfeet = get_node("playerfeet")
onready var camera = get_node("Camera")

func _ready():
	Input.set_mouse_mode(Input.MOUSE_MODE_HIDDEN)
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
	set_physics_process(true)
	set_process_input(true)

func _input(event):
	# For Mouse Look. The Camera node has a script for the X rotation.
	if event is InputEventMouseMotion:
		relativeMouseX = rotation.y - event.relative.x * MOUSESPEED
		rotation.y = relativeMouseX
	# Function to close game.
	if Input.is_key_pressed(KEY_ESCAPE) or Input.is_key_pressed(KEY_Q):
		get_tree().quit()

func _integrate_forces(state):
	if relativeMouseX != null:
		rotation.y = relativeMouseX
		relativeMouseX = null

func _process(delta):
	var is_on_ground = playerfeet.is_colliding()
	var on_top = playerfeet.get_collider()
	#Section for jumping
	if is_on_ground:
		jumping = false
	if Input.is_key_pressed(KEY_SPACE) and not jumping:
		var velocity = get_linear_velocity()
		velocity.y = JUMP_VEL
		set_linear_velocity(velocity)
		jumping = true
	#Section for basic movement.
	if jumping == false:
		if Input.is_key_pressed(KEY_W):
			apply_impulse(Vector3(0,0,0),get_transform().basis.z * speed*delta)
		if Input.is_key_pressed(KEY_S):
			apply_impulse(Vector3(0,0,0),-get_transform().basis.z * speed*delta)
		if Input.is_key_pressed(KEY_A):
			apply_impulse(Vector3(0,0,0),get_transform().basis.x * speed*delta)
		if Input.is_key_pressed(KEY_D):
			apply_impulse(Vector3(0,0,0),-get_transform().basis.x * speed*delta)
