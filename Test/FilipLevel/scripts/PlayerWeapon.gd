extends Node
 
#PlayerWeapon class 

var playerBase;


var mIsFiring; 
var mNextAttack;
 
var mAnimator;

func _init(): 
	mIsFiring = false;
	mNextAttack = 0.0; 

	
func ready(): 
	mAnimator = self.get_node("AnimationPlayer");
	mAnimator.connect("finished",self,"on_finished"); 
	mAnimator.play("FIRE");
	mAnimator.get_animation("FIRE").set_loop(true); 
	
		
func _input(event):
	if(event is InputEventMouseButton):
		if(event.is_pressed() && event.button_index == BUTTON_LEFT && !mIsFiring):
			mIsFiring = true; 
			print("fire pressed");
		if(!event.is_pressed() && mIsFiring):
			mIsFiring = false;	

	
func _process(delta): 
	#print("weapon delta script");
	
	if(mNextAttack>0.0):
		mNextAttack -= delta;		
	else:
		weaponShoot();	
		
func weaponShoot():
	
	if(!mIsFiring):
		#mAnimator.play("IDLE");
		return;
		
	mNextAttack = 0.1;			
	#mAnimator.play("FIRE");
	
func on_finished():
	mAnimator.play("IDLE");
		
