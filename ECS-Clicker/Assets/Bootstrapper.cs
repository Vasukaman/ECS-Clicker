// Bootstrapper.cs (Modified for a loader scene)
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private EcsStartup _ecsStartupPrefab;

    // Note: The SceneData reference is now an issue,
    // as it exists in a scene that hasn't loaded yet.
    // The solution is to load it from the scene after it's loaded
    // or to use Addressables/Resources, but for this test,
    // let's stick to the single-scene approach to keep it simple.

    // For the sake of demonstrating the pattern, let's assume
    // we don't need a SceneData reference at this stage.
    void Start()
    {
        // 1. Create the ECS system from the prefab
        var ecsStartup = Instantiate(_ecsStartupPrefab);

        // 2. Make it persist across all scenes
        DontDestroyOnLoad(ecsStartup.gameObject);

        // 3. Initialize it (without scene-specific data for now)
    //   ecsStartup.Init(); // A modified Init that doesn't need SceneData yet

        // 4. Load the actual game scene
        SceneManager.LoadScene("GameplayScene");
    }
}