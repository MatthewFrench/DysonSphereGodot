extends RigidBody

#PlayerPhysics class

const moveSpeed = 150.0;
const jumpForce = 30.0;

const humanWalkMetersPerSecond = 1.34;

var m_justJumping = false;
var mCamera;
var meshWeapon;

func _ready():
	mCamera = get_node("CameraBase");

func _integrate_forces(state):

	var mCamBasis = mCamera.get_camera_basis();
	var mMoveDir = calculate_move_dir(mCamBasis);

	mMoveDir = mMoveDir * moveSpeed;
	mMoveDir = state.get_linear_velocity().linear_interpolate(mMoveDir, state.get_step());

	mMoveDir.y = state.get_linear_velocity().y;

	if(Input.is_key_pressed(KEY_F10)):
		get_tree().quit();

	if(Input.is_key_pressed(KEY_F4)):
		OS.window_fullscreen = not OS.window_fullscreen;

	if(Input.is_key_pressed(KEY_SPACE)):
		if(!m_justJumping):
			mMoveDir.y = jumpForce;
			m_justJumping = true;
	else :
		if(m_justJumping):
			m_justJumping = false;


	#mPlayerBase.linear_velocity = mMoveDir;
	state.set_linear_velocity(mMoveDir);



func calculate_move_dir(basisp):

	var mDir = Vector3();

	if(Input.is_key_pressed(KEY_W)):
		mDir -= Vector3(0,0,1);

	if(Input.is_key_pressed(KEY_S)):
		mDir += Vector3(0,0,1);

	if(Input.is_key_pressed(KEY_A)):
		mDir -= Vector3(1,0,0);

	if(Input.is_key_pressed(KEY_D)):
		mDir += Vector3(1,0,0);

	mDir = mDir.normalized();

	mDir = basisp.xform(mDir);

	mDir.y = 0;

	return mDir;


