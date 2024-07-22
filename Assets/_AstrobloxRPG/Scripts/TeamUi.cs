using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class TeamUi : MonoBehaviour {
    [SerializeField, InfoBox("This is a list of the different agent types that can be used for this battle")]
    private AgentType[] availableAgentTypes;
    
    [SerializeField]
    private Transform agentHolder;
    [SerializeField]
    private GameObject agentUiPrefab;

    private List<AgentUi> _agentUis;

    private void Awake() {
        _agentUis = new List<AgentUi>();
    }

    private void Start() {
        AddAgent();
    }

    public void AddAgent() {
        GameObject newAgent = Instantiate(agentUiPrefab, agentHolder);
        AgentUi agentUi = newAgent.GetComponent<AgentUi>();
        int index = 0;
        agentUi.SetAgentType(index,availableAgentTypes[index]);
        agentUi.SwitchLeft.onClick.AddListener(() => SwitchAgentLeft(agentUi));
        agentUi.SwitchRight.onClick.AddListener(() => SwitchAgentRight(agentUi));
        agentUi.Remove.onClick.AddListener(() => RemoveAgent(agentUi));
        _agentUis.Add(agentUi);
    }

    private void RemoveAgent(AgentUi agentUi) {
        _agentUis.Remove(agentUi);
        Destroy(agentUi.gameObject);
    }

    private void SwitchAgentRight(AgentUi agentUi) {
        int index = agentUi.agentTypeIndex;
        index++;
        if (index >= availableAgentTypes.Length) index = 0;
        agentUi.SetAgentType(index, availableAgentTypes[index]);
    }

    private void SwitchAgentLeft(AgentUi agentUi) {
        int index = agentUi.agentTypeIndex;
        index--;
        if (index < 0) index = availableAgentTypes.Length - 1;
        agentUi.SetAgentType(index, availableAgentTypes[index]);
    }
}
