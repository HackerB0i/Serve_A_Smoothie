using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class FrameManager : MonoBehaviour
{
    [SerializeField] private int selectedFrame;
    [SerializeField] private bool isInteracting;
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private GameObject menuObject;
    
    private List<InventoryFrame> m_Frames = new();

    private float m_ChangeCooldown;
    private int m_InteractingPlayer = -1;

    private InputActionMap m_PMap;
    private InputAction m_PlayerHorizontal;
    
    private PlayerHoldItem playerHoldItem;

    private void Start()
    {
        m_Frames = GetComponentsInChildren<InventoryFrame>().ToList();
        m_PMap = inputActions.FindActionMap($"Player");
        m_PMap.FindAction($"P1X").Enable();
    }

    private void Update()
    {
        menuObject.SetActive(isInteracting);
        for (int i = 0; i < m_Frames.Count; i++)
        {
            if (i == selectedFrame)
            {
                m_Frames[i].isSelected = true;
            }
            else
            {
                m_Frames[i].isSelected = false;
            }
        }
        if (isInteracting && m_PlayerHorizontal.IsPressed() && m_ChangeCooldown <= 0)
        {
            selectedFrame += (int)m_PlayerHorizontal.ReadValue<float>();
            if (selectedFrame >= m_Frames.Count)
            {
                selectedFrame = 0;
            }
            else if (selectedFrame < 0)
            {
                selectedFrame = m_Frames.Count - 1;
            }
            m_ChangeCooldown = 0.15f;
        }

        if (isInteracting && m_PMap.FindAction($"P{m_InteractingPlayer}X").triggered)
        {
            if (playerHoldItem.holdingFruitObject == Resources.Load("Fruits/Objects/Air"))
            {
                playerHoldItem.SetFruitObject(m_Frames[selectedFrame].currentFruitObject);
                m_Frames[selectedFrame].currentFruitObject = Resources.Load<FruitObject>("Fruits/Objects/Air");
            }
            else if (m_Frames[selectedFrame].currentFruitObject == Resources.Load("Fruits/Objects/Air"))
            {
                m_Frames[selectedFrame].currentFruitObject = playerHoldItem.holdingFruitObject;
                playerHoldItem.SetFruitObject(Resources.Load<FruitObject>("Fruits/Objects/Air"));
            }
        }
        m_ChangeCooldown -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player" && m_InteractingPlayer == -1)
        {
            isInteracting = true;
            m_InteractingPlayer = collision.GetComponent<PlayerDesignator>().GetPlayerNumber();
            m_PlayerHorizontal = m_PMap.FindAction($"P{m_InteractingPlayer}H");
            collision.GetComponent<PlayerMovement>().LockMovement(true);
            playerHoldItem = collision.GetComponent<PlayerHoldItem>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player" && m_InteractingPlayer != -1)
        {
            isInteracting = false;
            m_InteractingPlayer = -1;
            collision.GetComponent<PlayerMovement>().LockMovement(false);
        }
    }
}
