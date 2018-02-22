extends RigidBody

# class member variables go here, for example:
# var a = 2
# var b = "textvar"

var mAnimation ; 
var health = 100 ; 
var ko = false;
var timerKo = 2;
var tag = "zombie"; 

func _ready():
	mAnimation = get_node("AnimationPlayer"); 
	mAnimation.play("WALKNEW");
	mAnimation.get_animation("IDLE").set_loop(true);
	mAnimation.get_animation("WALKNEW").set_loop(true);  
 
func deHealth():
	health -= 30;
	print("zomb health:", health);
	if(health < 0):
		ko = true;
		mAnimation.play("IDLE"); 
		
func _process(delta):
	if(ko): 
		timerKo -= delta;
		if(timerKo <0.0):
			self.queue_free();
			
		
			
