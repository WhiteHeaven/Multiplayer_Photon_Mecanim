using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour {

	public int dyingState;
	public int deadBool;
	public int locomotionState;
	public int speedFloat;
	public int playerInSightBool;
	public int aimWeightFloat;
	public int jumpState;


	void Awake()
	{
		dyingState = Animator.StringToHash("Base Layer.Dying");
		deadBool = Animator.StringToHash("Dead");
		locomotionState = Animator.StringToHash("Base Layer.Locomotion");
		speedFloat = Animator.StringToHash("Speed");
		aimWeightFloat = Animator.StringToHash("AimWeight");
		jumpState = Animator.StringToHash("Base Layer.Jump");	

	}

	
}
