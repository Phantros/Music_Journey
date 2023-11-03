using UnityEngine;
using System.Collections.Generic;

public class MelodyManager : MonoBehaviour
{
    private MelodyPuzzle currentPuzzle;

    public void SetCurrentPuzzle(MelodyPuzzle puzzle)
    {
        currentPuzzle = puzzle;
    }

    public void SwapPieces(int indexA, int indexB)
    {
        if (currentPuzzle != null)
        {
            currentPuzzle.SwapPieces(indexA, indexB);
        }
    }

    public bool IsPuzzleSolved()
    {
        return currentPuzzle != null && currentPuzzle.IsPuzzleSolved();
    }

    public MelodyPuzzle CurrentPuzzle()
    {
        if(currentPuzzle != null)
        {
            return currentPuzzle;
        }

        return null;
    }

    public void DebugSolution()
    {
        foreach(MelodyNote melodyNote in currentPuzzle.solutionOrder)
        {
            Debug.Log(melodyNote);
        }
    }
}
