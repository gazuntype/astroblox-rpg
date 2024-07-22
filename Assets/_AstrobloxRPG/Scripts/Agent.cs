using System;
using System.Collections;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Agent : MonoBehaviour {
    private const int HealValue = 5;
    private const int OTValue = 1;
    private const int OTTotalTime = 5; //in seconds
    private const int OTIntervalTime = 1; //in seconds
    public int currentHealth => _currentHealth;
    public int currentAttack => _currentAttack;
    public int currentDefense => _currentDefense;
    public int currentSpeed => _currentSpeed;
    public enum ActionTypes {
        Damage,
        DamageOverTime,
        Heal,
        HealOverTime,
        Buff,
        Debuff
    }

    [SerializeField]
    private SpriteRenderer spriteRend;

    [BoxGroup("Ui"), SerializeField]
    private TMP_Text agentName;

    [BoxGroup("Ui"), SerializeField]
    private MMProgressBar healthBar;

    [BoxGroup("Feedback"), SerializeField]
    private MMF_Player damageFeedback;
    [BoxGroup("Feedback"), SerializeField]
    private MMF_Player healFeedback;
    [BoxGroup("Feedback"), SerializeField]
    private MMF_Player tinyDamageFeedback;
    [BoxGroup("Feedback"), SerializeField]
    private MMF_Player tinyHealFeedback;

    private BattleManager.Team _team;
    private AgentType _agentType;
    private BattleManager _battleManager;
    private int _currentHealth, _currentAttack, _currentDefense, _currentSpeed;

    private float _lastActionTime;
    private float _waitTimeVariation;

    private IEnumerator _activeDamageCoroutine;
    private IEnumerator _activeHealCoroutine;

    public void InitializeAgent(BattleManager battleManager, AgentType type, BattleManager.Team team) {
        _battleManager = battleManager;
        _agentType = type;
        _team = team;
        agentName.text = _agentType.AgentName;
        spriteRend.sprite = _agentType.AgentIcon;
        _currentHealth = _agentType.InitialHealth;
        _currentAttack = _agentType.InitialAttack;
        _currentDefense = _agentType.InitialDefense;
        _currentSpeed = _agentType.InitialSpeed;
        _waitTimeVariation = Random.Range(-0.2f, 0.2f);
    }

    private void Update() {
        if (CanUseAction()) {
            _lastActionTime = _battleManager.battleTime;
            _waitTimeVariation = Random.Range(-0.2f, 0.2f);
            ChooseAction();
        }
    }

    private bool CanUseAction() {
        //This calculates the time an agent waits before it can use an action.
        float waitTime = 10f / _currentSpeed;
        return _battleManager.battleTime >= _lastActionTime + waitTime + _waitTimeVariation;
    }

    #region Perform Actions

    private void ChooseAction() {
        int index = Random.Range(0, Enum.GetValues(typeof(ActionTypes)).Length);
        switch ((ActionTypes)index) {
            default:
                DealDamage();
                break;
            case ActionTypes.Heal:
                Heal();
                break;
            case ActionTypes.Damage:
                DealDamage();
                break;
            case ActionTypes.DamageOverTime:
                if (_activeDamageCoroutine != null) {
                    StopCoroutine(_activeDamageCoroutine);
                }

                _activeDamageCoroutine = ReceiveDamageOverTime();
                StartCoroutine(_activeDamageCoroutine);
                break;
            case ActionTypes.HealOverTime:
                if (_activeHealCoroutine != null) {
                    StopCoroutine(_activeHealCoroutine);
                }

                _activeHealCoroutine = ReceiveHealingOverTime();
                StartCoroutine(_activeHealCoroutine);
                break;
        }
    }

    private void DealDamage() {
        Agent target = _battleManager.GetRandomOpponent(_team);
        target.ReceiveDamage(this);
    }

    private void Heal() {
        Agent target = _battleManager.GetRandomTeammate(_team);
        target.ReceiveHealing();
    }

    #endregion

    #region Receive Actions

    private void ReceiveDamage(Agent attackingAgent) {
        int damage = attackingAgent._currentAttack - _currentDefense;
        if (damage <= 0) return;
        AffectHealth(-damage);
        damageFeedback.PlayFeedbacks();
    }

    private IEnumerator ReceiveDamageOverTime() {
        int time = 0;
        while (time < OTTotalTime) {
            yield return new WaitForSeconds(OTIntervalTime);
            AffectHealth(-OTValue);
            tinyDamageFeedback.PlayFeedbacks();
            time += OTIntervalTime;
        }
    }

    private IEnumerator ReceiveHealingOverTime() {
        int time = 0;
        while (time < OTTotalTime) {
            yield return new WaitForSeconds(OTIntervalTime);
            AffectHealth(OTValue);
            tinyHealFeedback.PlayFeedbacks();
            time += OTIntervalTime;
        } 
    }

    private void ReceiveHealing() {
        AffectHealth(HealValue);
        healFeedback.PlayFeedbacks();
    }

    private void AffectHealth(int val) {
        _currentHealth += val;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _agentType.InitialHealth);
        healthBar.UpdateBar(_currentHealth, 0, _agentType.InitialHealth);
    }

    #endregion
    
    
    
}
