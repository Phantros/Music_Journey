using UnityEngine;

public class PuzzleZone : MonoBehaviour
{
    public MelodyPuzzle puzzle; // Assign the puzzle prefab to this field in the Unity editor.
    public MelodyManager melodyManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player enters the trigger zone.
        {
            // Set the current puzzle to the puzzle in this zone.
            melodyManager.SetCurrentPuzzle(puzzle);

            Debug.Log("Current puzzle is: " + puzzle);

            melodyManager.DebugSolution();
        }
    }
}
