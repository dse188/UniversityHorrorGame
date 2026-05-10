using UnityEditor;
using Unity.AI.MCP.Editor;

[InitializeOnLoad]
static class McpConnectionPolicyFix
{
    static McpConnectionPolicyFix()
    {
        EditorApplication.delayCall += ApplyFix;
        UnityMCPBridge.MaxDirectConnectionsPolicyChanged += ApplyFix;
    }

    static void ApplyFix()
    {
        var current = UnityMCPBridge.MaxDirectConnectionsResolver?.Invoke() ?? 0;
        if (current != -1)
            UnityMCPBridge.MaxDirectConnectionsResolver = () => -1;
    }
}
