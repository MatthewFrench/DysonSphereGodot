extends Node

# class member variables go here, for example:
# var a = 2
# var b = "textvar"

var mParticle;

func _ready():
	mParticle = get_node("particles");

#func _process(delta):	
	#if(!mParticle.is_emitting()):
	#	print("destroy particle");
	#	mParticle.queue_free();
	#	self.queue_free();
		
#	# Called every frame. Delta is time since last frame.
#	# Update game logic here.
#	pass
