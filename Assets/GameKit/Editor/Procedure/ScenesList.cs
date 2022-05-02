#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
public static class ScenesList
{
    [MenuItem("Scenes/Demo_UI")]
    public static void Assets_GameKit_Core_Runtime_UGUI_Demo_Demo_UI_unity() { ScenesUpdate.OpenScene("Assets/GameKit/Core/Runtime/UGUI/Demo/Demo_UI.unity"); }
    [MenuItem("Scenes/Attributes")]
    public static void Assets_GameKit_Prototype_Attributes_Attributes_unity() { ScenesUpdate.OpenScene("Assets/GameKit/Prototype/Attributes/Attributes.unity"); }
    [MenuItem("Scenes/Fsm")]
    public static void Assets_GameKit_Prototype_GameKitFsm_Fsm_unity() { ScenesUpdate.OpenScene("Assets/GameKit/Prototype/GameKitFsm/Fsm.unity"); }
    [MenuItem("Scenes/S_Menu")]
    public static void Assets_GameKit_Prototype_Procedure_Scenes_S_Menu_unity() { ScenesUpdate.OpenScene("Assets/GameKit/Prototype/Procedure/Scenes/S_Menu.unity"); }
    [MenuItem("Scenes/S_Procedure")]
    public static void Assets_GameKit_Prototype_Procedure_Scenes_S_Procedure_unity() { ScenesUpdate.OpenScene("Assets/GameKit/Prototype/Procedure/Scenes/S_Procedure.unity"); }
    [MenuItem("Scenes/S_Select")]
    public static void Assets_GameKit_Prototype_Procedure_Scenes_S_Select_unity() { ScenesUpdate.OpenScene("Assets/GameKit/Prototype/Procedure/Scenes/S_Select.unity"); }
    [MenuItem("Scenes/ShaderGround")]
    public static void Assets_GameKit_Prototype_ShaderAssets_ShaderGround_unity() { ScenesUpdate.OpenScene("Assets/GameKit/Prototype/ShaderAssets/ShaderGround.unity"); }
    [MenuItem("Scenes/Main")]
    public static void Assets_GameMain_Scenes_Main_unity() { ScenesUpdate.OpenScene("Assets/GameMain/Scenes/Main.unity"); }
    [MenuItem("Scenes/Test")]
    public static void Assets_GameMain_Scenes_Test_unity() { ScenesUpdate.OpenScene("Assets/GameMain/Scenes/Test.unity"); }
}
#endif
