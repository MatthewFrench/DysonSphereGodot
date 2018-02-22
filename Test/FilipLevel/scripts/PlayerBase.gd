extends RigidBody

# class member variables go here, for example:
# var a = 2
# var b = "textvar"
var mController =  preload("PlayerController.gd");

var mCameraBase; 

func _ready():
	# Called every time the node is added to the scene.
	# Initialization here
	mController = mController.new(self);
	add_child(mController);
	
	mCameraBase = get_node("CameraBase");
	
func getCamera():
	return mCameraBase; 
	
	
#func _process(delta):
#	# Called every frame. Delta is time since last frame.
#	# Update game logic here.
#	pass
