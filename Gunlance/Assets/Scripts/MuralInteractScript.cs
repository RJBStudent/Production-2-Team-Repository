using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuralInteractScript : MonoBehaviour
{
	 [SerializeField] GameObject muralReveal;
	 [SerializeField] GameObject prompt;
	 bool revealed = false;
	 bool close = false;
	 bool showing = false;

	 private void Awake()
	 {
		  muralReveal.SetActive(false);
		  prompt.SetActive(false);
	 }

	 private void OnTriggerStay(Collider other)
	 {
		  if (!revealed && other.CompareTag("Player"))
		  {
		      close = true;
			   if (showing)
			   {
					other.gameObject.GetComponent<PlayerMovementScript>().canJump = false;
			   }
		  }
		  else if (revealed)
		  {
				prompt.SetActive(false);
		  }
	 }

	 private void OnTriggerExit(Collider other)
	 {
	     if (other.CompareTag("Player"))
		  {
				close = false;
			   other.gameObject.GetComponent<PlayerMovementScript>().canJump = true;
		  }
	 }

	 private void Update()
	 {
		  if (close && !showing)
		  {
				prompt.SetActive(true);
				showing = true;
		  }
		  else if (!close && showing)
		  {
				prompt.SetActive(false);
				showing = false;
		  }

		  //prompt displayed and A pressed
		  if (showing && Input.GetButtonDown("Jump"))
		  {
				muralReveal.SetActive(true);
				revealed = true;
		  }
	 }
}
