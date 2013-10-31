using UnityEngine;
using System.Collections;
using Photon;

public class PlayerControl : Photon.MonoBehaviour {
		
	public float lookWeight;					// the amount to transition when using head look
	
	
	public float animSpeed = 1.5f;				// a public setting for overall animator animation speed
	public float lookSmoother = 3f;				// a smoothing setting for camera motion
	

	
	private Animator anim;							// a reference to the animator on the character
	private AnimatorStateInfo currentBaseState;			// a reference to the current state of the animator, used for base layer
	private AnimatorStateInfo layer2CurrentState;	// a reference to the current state of the animator, used for layer 2
	private HashIDs hash;


	private float netSpeed;
	private float netDirection;
	private bool netFalling;
	private bool netJump;
	private bool netWave;
	private bool netLookAtEnemy;

	static int idleState = Animator.StringToHash("Base Layer.Idle");	
	static int locoState = Animator.StringToHash("Base Layer.Locomotion");			// these integers are references to our animator's states
	static int jumpState = Animator.StringToHash("Base Layer.Jump");				// and are used to check state for various actions to occur
	static int jumpDownState = Animator.StringToHash("Base Layer.JumpDown");		// within our FixedUpdate() function below
	static int fallState = Animator.StringToHash("Base Layer.Fall");
	static int rollState = Animator.StringToHash("Base Layer.Roll");
	static int waveState = Animator.StringToHash("Layer2.Wave");
	public bool isControllable = true;


	void Awake()
	{
		if (photonView.isMine)
        {
           
            
        }
        else
        {           
            
           isControllable = false;
        }

        gameObject.name = gameObject.name + photonView.viewID;
	}
	void Start ()
	{
		// initialising reference variables
		anim = GetComponent<Animator>();	
		hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();				  
		if(anim.layerCount ==2)
			anim.SetLayerWeight(1, 1);
	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			//We own this player: send the others our data
			stream.SendNext(netSpeed);
			stream.SendNext(netDirection);
			
			stream.SendNext(netJump);
			
			
		}
		else
		{
			//Network player, receive data
			netSpeed = (float)stream.ReceiveNext();
			netDirection = (float)stream.ReceiveNext();
			
			netJump = (bool)stream.ReceiveNext();
			
		}
	}
	void FixedUpdate ()
	{
		//float h = Input.GetAxis("Horizontal");				// setup h variable as our horizontal input axis
		//float v = Input.GetAxis("Vertical");				// setup v variables as our vertical input axis
		//anim.SetFloat(hash.speedFloat, v);							// set our animator's float parameter 'Speed' equal to the vertical input axis				
		//anim.SetFloat("Direction", h); 						// set our animator's float parameter 'Direction' equal to the horizontal input axis		
		anim.speed = animSpeed;								// set the speed of our animator to the public variable 'animSpeed'
		anim.SetLookAtWeight(lookWeight);					// set the Look At Weight - amount to use look at IK vs using the head's animation
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);	// set our currentState variable to the current state of the Base Layer (0) of animation
		
		if(anim.layerCount ==2)		
			layer2CurrentState = anim.GetCurrentAnimatorStateInfo(1);	// set our layer2CurrentState variable to the current state of the second Layer (1) of animation
		
		
		if (photonView.isMine)
		{
			if(isControllable)
			{


				netDirection = Input.GetAxis("Horizontal");			// setup h variable as our horizontal input axis
				netSpeed = Input.GetAxis("Vertical");				// setup v variables as our vertical input axis
				
				
				

				if (currentBaseState.nameHash == locoState)
				{
					if (Input.GetButtonDown("Jump"))
					{
						netJump = true;
					}
				}
				// if we are in the jumping state... 
				else if (currentBaseState.nameHash == jumpState)
				{
					//  ..and not still in transition..
					if (!anim.IsInTransition(0))
					{
						

						// reset the Jump bool so we can jump again, and so that the state does not loop 
						netJump = false;
					}

				}
				// IDLE

				// check if we are at idle, if so, let us Wave!
				else if (currentBaseState.nameHash == idleState)
				{
					if (Input.GetButtonUp("Jump"))
					{
						
					}
				}
			}
		}
		else
		{


		}
		anim.SetFloat("Direction", netDirection); 			// set our animator's float parameter 'Direction' equal to the horizontal input axis		
		anim.SetFloat("Speed", netSpeed);					// set our animator's float parameter 'Speed' equal to the vertical input axis				
		anim.SetBool("Falling", netFalling);
		anim.SetBool("Jump", netJump);
		
	}
}