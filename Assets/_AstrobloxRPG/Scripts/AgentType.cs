using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "AgentType", menuName = "AstrobloxRPG/Agent Type")]
public class AgentType : ScriptableObject {
    public string AgentName => agentName;
    public Sprite AgentIcon => agentIcon;
    public int InitialHealth => initialHealth;
    public int InitialAttack => initialAttack;
    public int InitialDefense => initialDefense;
    public int InitialSpeed => initialSpeed;
    
    [SerializeField]
    private string agentName;

    [SerializeField]
    private Sprite agentIcon;
    
    [BoxGroup("Agent Stats"), SerializeField]
    private int initialHealth;
    [BoxGroup("Agent Stats"), SerializeField]
    private int initialAttack;
    [BoxGroup("Agent Stats"), SerializeField]
    private int initialDefense;
    [BoxGroup("Agent Stats"), SerializeField]
    private int initialSpeed;
}
