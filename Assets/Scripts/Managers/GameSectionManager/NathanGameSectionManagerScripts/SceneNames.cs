/// <summary>
/// SceneNames
/// 
/// One place for the exact name of every scene in the game loop.
///
/// A typo in a string only shows up as an error while the game is running.
/// A typo in SceneNames.Delivery is a compile error, so you see it right away.
/// And if a scene gets renamed, you only change it here.
///
/// "const" means the value is fixed when the code compiles and can never change.
///
/// IMPORTANT: each value must match the scene's file name exactly (without .unity),
/// including capitals and spaces. Change them to the real scene names.
/// </summary>
public static class SceneNames
{
    public const string StartMenu = "StartMenu"; //"StartMenu"

    // Grabbing, scanning and sorting all happen in this scene, as phases.
    // If the team keeps the current name, change this to "Sorting System".
    public const string Warehouse = "Warehouse"; //"Warehouse"

    public const string Delivery = "DeliveryScene"; //"DeliveryScene"

    public const string EndScreen = "EndScreen"; //"EndScreen
}
