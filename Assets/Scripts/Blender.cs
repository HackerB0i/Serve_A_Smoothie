using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Blender : MonoBehaviour
{
    [SerializeField] private int selectedFrame;
    [SerializeField] private bool isInteracting;
    [SerializeField] private bool subMenuOpen;
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private GameObject menuObject;
    [SerializeField] private GameObject subMenuObject;
    
    private List<InventoryFrame> m_Frames = new();

    private float m_ChangeCooldown;
    private int m_InteractingPlayer = -1;

    private InputActionMap m_PMap;
    private InputAction m_PlayerHorizontal;
    
    private PlayerHoldItem playerHoldItem;
    private Recipe currentRecipe;
    
    
    private void Start()
    {
        m_Frames = GetComponentsInChildren<InventoryFrame>().ToList();
        m_PMap = inputActions.FindActionMap($"Player");
        m_PMap.FindAction($"P1X").Enable();
    }

    private void Update()
    {
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
            if (subMenuOpen)
            {
                if (playerHoldItem.holdingFruitObject == Resources.Load("Fruits/Objects/Air") && !m_Frames[selectedFrame].isPreview)
                {
                    playerHoldItem.SetFruitObject(m_Frames[selectedFrame].currentFruitObject);
                    m_Frames[selectedFrame].currentFruitObject = Resources.Load<FruitObject>("Fruits/Objects/Air");
                    m_Frames[selectedFrame].isPreview = true;
                    if (selectedFrame == m_Frames.Count - 1)
                    {
                        for (int i = 10; i < m_Frames.Count; i++)
                        {
                            m_Frames[i].isPreview = true;
                        }
                    }
                }
                else if (m_Frames[selectedFrame].currentFruitObject == playerHoldItem.holdingFruitObject && m_Frames[selectedFrame].isPreview)
                {
                    m_Frames[selectedFrame].isPreview = false;
                    playerHoldItem.SetFruitObject(Resources.Load<FruitObject>("Fruits/Objects/Air"));
                }
            }
            else if (m_Frames[selectedFrame].currentFruitObject.type != FruitObject.Type.Nothing)
            {
                currentRecipe = Resources.Load<Recipe>($"Recipes/RecipeObjects/{m_Frames[selectedFrame].currentFruitObject.name}");
                subMenuOpen = true;
            }
        }

        if (subMenuOpen)
        {
            for (int i = 10; i < m_Frames.Count-1; i++)
            {
                if (i - 10 < currentRecipe.recipeList.Count)
                {
                    m_Frames[i].currentFruitObject = currentRecipe.recipeList[i-10];
                }
                else
                {
                    m_Frames[i].currentFruitObject = Resources.Load<FruitObject>("Fruits/Objects/Air");
                }
            }
            m_Frames[m_Frames.Count-1].currentFruitObject = currentRecipe.resultFruitObject;
            m_Frames[m_Frames.Count-1].isPreview = !FramesMatchRecipe(10, currentRecipe);
            
            menuObject.SetActive(false);
            subMenuObject.SetActive(true);
            selectedFrame = Mathf.Clamp(selectedFrame, 10, m_Frames.Count - 1);
        }
        else
        {
            menuObject.SetActive(isInteracting);
            subMenuObject.SetActive(false);
            selectedFrame = Mathf.Clamp(selectedFrame, 0, 9);
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
            collision.GetComponent<PlayerMovement>().ControlHorizontal(true);
            playerHoldItem = collision.GetComponent<PlayerHoldItem>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player" && m_InteractingPlayer != -1)
        {
            if (FramesEmpty(10))
            {
                subMenuOpen = false;
                for (int i = 10; i < m_Frames.Count; i++)
                {
                    m_Frames[i].isPreview = true;
                }
            }
            isInteracting = false;
            m_InteractingPlayer = -1;
            collision.GetComponent<PlayerMovement>().ControlHorizontal(false);
        }
    }

    private bool FramesEmpty(int startIndex)
    {
        for (int i = startIndex; i < m_Frames.Count; i++)
        {
            if (m_Frames[i].currentFruitObject.type != FruitObject.Type.Nothing && !m_Frames[i].isPreview)
            {
                return false;
            }
        }

        return true;
    }

    private bool FramesMatchRecipe(int startIndex, Recipe recipe)
    {
        for (int i = 0; i < recipe.recipeList.Count; i++)
        {
            if (m_Frames[i+startIndex].currentFruitObject.fruitName != recipe.recipeList[i].fruitName | m_Frames[i+startIndex].isPreview)
            {
                return false;
            }
        }

        return true;
    }
}
