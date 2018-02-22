extends Node


const moveSpeed = 4.0; 

# class member variables go here, for example:
# var a = 2
# var b = "textvar"
var mPlayerBase ;


func _init(playerBase):
	mPlayerBase = playerBase ;
	set_name("controller"); 
	
	
func _physics_process(delta):
	 
	var mCamBasis = get_node("CameraBase").get_camera_basis();
	
	var mMoveDir = calculate_move_dir(); 
	
	mMoveDir = mMoveDir * moveSpeed ;
	
	mMoveDir = linear_velocity.linear_interpolate(mMoveDir, 8.0 * delta);
	
	mMoveDir.y = linear_velocity.y; 
	
	#mPlayerBase.linear_velocity = mMoveDir; 
	linear_velocity  = mMoveDir;
	
	
	
func calculate_move_dir():
	var mDir = Vector3();
	if(Input.is_action_pressed("move_forward")):
		mDir -= Vector3(0,0,1);
		print("move_forward");
	if(Input.is_action_pressed("move_backwards")):
		mDir += Vector3(0,0,1);
	
	if(Input.is_action_pressed("move_left")):
		mDir -= Vector3(1,0,0);	
	if(Input.is_action_pressed("move_right")):
		mDir += Vector3(1,0,0);		
	
	mDir = mDir.normalized();
	
	mDir.y = 0.0;
	
	print("MDir",mDir.x, mDir.y, mDir.z);
	
	return mDir; 
		
		
#func _process(delta):
#	# Called every frame. Delta is time since last frame.
#	# Update game logic here.
#	pass
