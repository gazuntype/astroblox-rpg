using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AgentUi : MonoBehaviour {
    public AgentType agentType => _agentType;
    public int agentTypeIndex => _agentTypeIndex;
    public Button SwitchLeft => switchLeft;
    public Button SwitchRight => switchRight;
    public Button Remove => remove;
    
    private AgentType _agentType;
    private int _agentTypeIndex;
    [SerializeField]
    private Image image;
    [SerializeField]
    private TMP_Text agentName;

    [BoxGroup("Buttons"), SerializeField]
    private Button switchLeft;
    [BoxGroup("Buttons"), SerializeField]
    private Button switchRight;
    [BoxGroup("Buttons"), SerializeField]
    private Button remove;

    public void SetAgentType(int index, AgentType type) {
        _agentTypeIndex = index;
        _agentType = type;
        image.sprite = _agentType.AgentIcon;
        agentName.text = _agentType.AgentName;
    }

}
