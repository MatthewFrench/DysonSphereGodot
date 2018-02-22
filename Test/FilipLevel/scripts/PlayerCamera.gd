
extends Spatial

#PlayerCamera class

# class member variables go here, for example:
# var a = 2
# var b = "textvar"

var mCamera;
var mpitch;
var mYaw;
var meshWeapon;

func _ready():
	# Called every time the node is added to the scene.
	# Initialization here
	mCamera = get_node("Camera"); 
	meshWeapon = get_node("Camera/playermesh");
	
	mCamera.make_current(); 
	
	mpitch = 0.0;
	mYaw = 0.0;
	
func _enter_tree():	
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED);
	
func _exit_tree():
	Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE);
	
func _input(event):
		
	if(Input.is_key_pressed(KEY_ESCAPE)):
		Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE);
	
	if(Input.is_key_pressed(KEY_ENTER)):
		Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED);
	
		
	if(event is InputEventMouseMotion):
		mpitch = clamp(mpitch - event.relative.y*0.3, -89.0 , 89.0);
		mYaw = fmod(mYaw - event.relative.x *0.3 , 360);		
		update_camera(); 		
		
func update_camera() : 		
	mCamera.rotation.x = deg2rad(mpitch) ; 
	mCamera.rotation.y = deg2rad(mYaw) ; 
	print("Cam update call");
	
func get_camera_basis():
	return mCamera.global_transform.basis; 
		

#func _process(delta):
	
		
#func _process(delta):
#	# Called every frame. Delta is time since last frame.
#	# Update game logic here.
#	pass
