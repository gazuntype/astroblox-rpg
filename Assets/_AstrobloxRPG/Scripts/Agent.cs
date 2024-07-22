using UnityEngine;

[CreateAssetMenu(fileName = "Agent", menuName = "AstrobloxRPG")]
public class Agent : MonoBehaviour {
    public enum ActionTypes {
        Damage,
        DamageOverTime,
        Heal,
        HealOverTime,
        Buff,
        Debuff
    }

    private AgentType _agentType;
    private BattleManager _battleManager;
    private int _currentHealth, _currentAttack, _currentDefense, _currentSpeed;

    public void InitializeAgent(BattleManager battleManager, AgentType type) {
        _battleManager = battleManager;
        _agentType = type;
        _currentHealth = _agentType.InitialHealth;
        _currentAttack = _agentType.InitialAttack;
        _currentDefense = _agentType.InitialDefense;
        _currentSpeed = _agentType.InitialSpeed;
    }
}
