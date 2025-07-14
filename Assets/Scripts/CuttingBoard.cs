using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CuttingBoard : MonoBehaviour
{
    [SerializeField] private int selectedFrame;
    [SerializeField] private bool isInteracting;
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private GameObject menuObject;
    
    private List<InventoryFrame> m_Frames = new();

    private float m_ChangeCooldown;
    private int m_InteractingPlayer = -1;

    private bool _inGame = false;
    private bool _inputWindow = false;

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
        if (m_Frames[0].currentFruitObject.type == FruitObject.Type.Fruit)
        {
            m_Frames[1].currentFruitObject = Resources.Load<FruitObject>($"Fruits/Objects/Cut {m_Frames[0].currentFruitObject.name}");
        }
        else if (!_inGame)
        {
            m_Frames[1].currentFruitObject = Resources.Load<FruitObject>($"Fruits/Objects/Air");
        }
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

            if (_inGame)
            {
                selectedFrame = Mathf.Clamp(selectedFrame, 1, m_Frames.Count - 1);
            }
            m_ChangeCooldown = 0.15f;
        }

        if (isInteracting && m_PMap.FindAction($"P{m_InteractingPlayer}X").triggered)
        {
            if (playerHoldItem.holdingFruitObject.type == FruitObject.Type.Nothing && !_inGame)
            {
                if (selectedFrame == 1 && m_Frames[selectedFrame].currentFruitObject.type == FruitObject.Type.Cut)
                {
                    StartCoroutine(Minigame());
                }
                else
                {
                    playerHoldItem.SetFruitObject(m_Frames[selectedFrame].currentFruitObject);
                    m_Frames[selectedFrame].currentFruitObject = Resources.Load<FruitObject>("Fruits/Objects/Air");
                }
            }
            else if (m_Frames[selectedFrame].currentFruitObject.type == FruitObject.Type.Nothing && selectedFrame == 0)
            {
                m_Frames[selectedFrame].currentFruitObject = playerHoldItem.holdingFruitObject;
                playerHoldItem.SetFruitObject(Resources.Load<FruitObject>("Fruits/Objects/Air"));
            }
        }

        if (_inputWindow)
        {
            if (m_PMap.FindAction($"P{m_InteractingPlayer}X").triggered)
            {
                MoneyDisplay.Instance.Money += 2;
                _inputWindow = false;
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

    private IEnumerator Minigame()
    {
        _inGame = true;
        GetComponent<CuttingMinigame>().StartGame();
        yield return new WaitForSeconds(0.65f);
        _inputWindow = true;
        yield return new WaitForSeconds(0.2f);
        _inputWindow = false;
        yield return new WaitForSeconds(0.65f);
        m_Frames[0].currentFruitObject = Resources.Load<FruitObject>("Fruits/Objects/Air");
        playerHoldItem.SetFruitObject(m_Frames[selectedFrame].currentFruitObject);
        m_Frames[selectedFrame].currentFruitObject = Resources.Load<FruitObject>("Fruits/Objects/Air");
        _inGame = false;
    }
}
