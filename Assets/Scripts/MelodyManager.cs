using UnityEngine;
using System.Collections.Generic;

public class MelodyManager : MonoBehaviour
{
    public static MelodyManager Instance;

    [SerializeField]
    public List<MelodyNote> currentOrder = new List<MelodyNote>();

    public List<MelodyNote> solutionOrder = new List<MelodyNote>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SwapPieces(int indexA, int indexB)
    {
        // Check if the indices are valid.
        if (indexA < 0 || indexA >= currentOrder.Count || indexB < 0 || indexB >= currentOrder.Count)
        {
            Debug.LogError("Invalid indices for swapping.");
            return;
        }

        // Swap the puzzle pieces in the current order list.
        MelodyNote temp = currentOrder[indexA];
        currentOrder[indexA] = currentOrder[indexB];
        currentOrder[indexB] = temp;

        // Update the MelodyNote component indices.
        currentOrder[indexA].index = indexA;
        currentOrder[indexB].index = indexB;

        for (int i = 0; i < currentOrder.Count; i++)
        {
            Debug.Log("Order: " + currentOrder[i]);
        }
    }

    public bool IsPuzzleSolved()
    {
        // Check if the current order matches the solution order.
        if (currentOrder.Count != solutionOrder.Count)
        {
            return false;
        }

        for (int i = 0; i < currentOrder.Count; i++)
        {
            if (currentOrder[i] != solutionOrder[i])
            {
                return false;
            }
        }

        return true;
    }
    
    public List<MelodyNote> GetCurrentOrder()
    {
        return currentOrder;
    }
}
