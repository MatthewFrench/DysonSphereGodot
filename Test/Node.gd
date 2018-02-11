extends Node

# class member variables go here, for example:
# var a = 2
# var b = "textvar"

func _ready():
	# Called every time the node is added to the scene.
	# Initialization here
	var surfTool = SurfaceTool.new()
	var mesh = Mesh.new()
	var material = SpatialMaterial.new()
	# material.set_parameter(material.PARAM_DIFFUSE,Color(1,0,0,1))
	
	surfTool.set_material(material)
	surfTool.begin(Mesh.PRIMITIVE_TRIANGLES)
	
	surfTool.add_uv(Vector2(0,0))
	surfTool.add_vertex(Vector3(-10,-10,0))
	
	surfTool.add_uv(Vector2(0.5,1))
	surfTool.add_vertex(Vector3(0,10,0))
	
	surfTool.add_uv(Vector2(1,0))
	surfTool.add_vertex(Vector3(10,-10,0))
	
	
	surfTool.generate_normals()
	surfTool.index()
	
	surfTool.commit(mesh)
	
	var meshInstance = MeshInstance.new()
	meshInstance.set_mesh(mesh)
	self.add_child(meshInstance)
	
	var Hexasphere = preload("res://Hexasphere.cs")
	
	var h = Hexasphere.new(30, 25, 0.95);
	#var a = Point.new()
	
	#print(a.GetStuff())
	
	pass

#func _process(delta):
#	# Called every frame. Delta is time since last frame.
#	# Update game logic here.
#	pass
