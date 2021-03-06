var collider = null;
var meshInstance = null;
var position = Vector2(0, 0);

func updateCollider():
	clearCollider();
	if collider == null:
		# TODO This 'helper' is very confusing.
		# It does stuff behind the scenes I had no idea of, and returns nothing :/
		meshInstance.create_trimesh_collision();
		collider = _getCollider();

func clearCollider():
	if collider != null:
		collider.queue_free();
		collider = null;

# Helper to get the collider generated by the other helper...
func _getCollider():
	var count = meshInstance.get_child_count();
	if count == 0:
		return null;
	# Get last node, assuming it's the latest generated collider
	var body = meshInstance.get_child(count - 1);
	if body is StaticBody:
		return body;
	print('ERROR: toy_terrain_chunk.gd: could not retrieve generated trimesh collision');
	return null;
