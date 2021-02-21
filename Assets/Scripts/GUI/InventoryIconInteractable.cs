﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace GUI
{
	public class InventoryIconInteractable : InventoryIcon, IPointerDownHandler
	{

		private RectTransform rectTransform;
		private Vector3 startPosition;
		private bool isDragging = false;
		private GameObject lastTouchedObject;
		private GameObject originalParent;


		private static GameObject draggingParent; // Parent object to child icons to as they are being dragged
		private static InventoryScreenManager invScreen;
		private static GameObject inventoryBackgroundPanel;

		private void Start()
		{
			renderer = GetComponent<Image>();
			rectTransform = GetComponent<RectTransform>();
			originalParent = gameObject.transform.parent.gameObject;

			draggingParent = GameObject.Find("Drag Parent Object");
			invScreen = FindObjectOfType<InventoryScreenManager>();
			inventoryBackgroundPanel = invScreen.GetBackgroundPanel();
		}

		private void Update()
		{
			if (PauseManager.Paused) return;
			
			if (isDragging)
			{
				rectTransform.position = Input.mousePosition;
				
				// ============== Detect object under cursor ==================
				
				SetRaycastTarget(false);
				PointerEventData pointer = new PointerEventData(EventSystem.current);
				pointer.position = Input.mousePosition;
				List<RaycastResult> raycastResults = new List<RaycastResult>();
				EventSystem.current.RaycastAll(pointer, raycastResults);
				if (raycastResults.Count > 0)
				{
					foreach (RaycastResult rr in raycastResults)
					{
						if (rr.gameObject.tag.EndsWith("Slot"))
						{
							lastTouchedObject = rr.gameObject;
							break;
						}

						lastTouchedObject = raycastResults[0].gameObject;
					}
				}
				SetRaycastTarget(true);
			}
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (!visible) return;
			
			if (!isDragging)
			{
				BeginItemMove();
				
				// Determine selected slot
				
				GameObject activeSlot = eventData.pointerCurrentRaycast.gameObject;

				if (activeSlot.GetComponent<InventoryIconInteractable>())
				{
					activeSlot = activeSlot.transform.parent.gameObject;
				}

				if (activeSlot.CompareTag("InventorySlot") ||
				    activeSlot.CompareTag("HotbarSlot") ||
				    activeSlot.CompareTag("HatSlot") ||
				    activeSlot.CompareTag("ShirtSlot") ||
				    activeSlot.CompareTag("PantsSlot") ||
				    activeSlot.CompareTag("ContainerSlot"))
				{
					invScreen.ManageSlotSelection(activeSlot);
				}
			}
			else
			{
				EndItemMove();
			}
		}

		private void BeginItemMove()
		{
			startPosition = rectTransform.position;
			isDragging = true;
			SetRaycastTarget(true);

			gameObject.transform.SetParent(draggingParent.transform);
		}
		
		private void EndItemMove()
		{
			SetRaycastTarget(true);
			HandleItemMove(lastTouchedObject);
		}

		private void SetRaycastTarget(bool receiveRaycasts)
		{
			renderer.raycastTarget = receiveRaycasts;
		}

		private void OnApplicationFocus(bool isFocused)
		{
			if (!isFocused && isDragging)
			{
				rectTransform.position = startPosition;
				isDragging = false;
				gameObject.transform.SetParent(originalParent.transform);
			}
		}

		private void HandleItemMove(GameObject destination)
		{
			if (destination == null)
			{
				Debug.LogWarning("Item move destination shouldn't be null.");
				return;
			}

			if (destination.GetComponent<InventoryIconInteractable>())
			{
				destination = destination.transform.parent.gameObject;
			}

			if (destination.CompareTag("InventorySlot") ||
				destination.CompareTag("HotbarSlot") ||
				destination.CompareTag("HatSlot") ||
				destination.CompareTag("ShirtSlot") ||
				destination.CompareTag("PantsSlot") ||
				destination.CompareTag("ContainerSlot"))
			{
				if (invScreen.AttemptInventoryMove(originalParent, destination))
					ResetPosition();
			}
			else
			{
				if (destination == inventoryBackgroundPanel)
				{
					invScreen.ManageInventoryDragOutOfWindow(originalParent);
					ResetPosition();
				}
			}
		}

		private void ResetPosition()
		{
			isDragging = false;
			SetRaycastTarget(true);
			rectTransform.position = startPosition;
			gameObject.transform.SetParent(originalParent.transform);
		}
	}
}