extends Node

# class member variables go here, for example:
# var a = 2
# var b = "textvar"
var mAnimator;


func _ready():
	mAnimator = self.get_node("AnimationPlayer");
	mAnimator.connect("finished",self,"on_finished");
	
	mAnimator.play("FIRE");
	mAnimator.get_animation("FIRE").set_loop(true); 
	
#func _process(delta): 

func on_finished():
	mAnimator.play("IDLE");
	
