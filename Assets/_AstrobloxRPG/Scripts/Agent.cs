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
    private const int OTTotalTime = 5;
    private const int OTIntervalTime = 1;
    private const int BuffTime = 3;
    private const int BuffValue = 3;

    public BattleManager.Team agentTeam => _team;
    public int currentHealth => _currentHealth;
    public int currentAttack => _currentAttack;
    public int currentDefense => _currentDefense;
    public int currentSpeed => _currentSpeed;
    public enum ActionType {
        Damage,
        DamageOverTime,
        Heal,
        HealOverTime,
        SpeedBuff,
        AttackBuff,
        DefenseBuff,
        SpeedDebuff,
        AttackDebuff,
        DefenseDebuff,
    }

    [SerializeField]
    private SpriteRenderer spriteRend;

    [BoxGroup("Ui"), SerializeField]
    private TMP_Text agentName;

    [BoxGroup("Ui"), SerializeField]
    private MMProgressBar healthBar;
    [BoxGroup("Ui"), SerializeField]
    private MMProgressBar speedBuffBar;
    [BoxGroup("Ui"), SerializeField]
    private MMProgressBar attackBuffBar;
    [BoxGroup("Ui"), SerializeField]
    private MMProgressBar defenseBuffBar;
    [BoxGroup("Ui"), SerializeField]
    private MMProgressBar speedDebuffBar;
    [BoxGroup("Ui"), SerializeField]
    private MMProgressBar attackDebuffBar;
    [BoxGroup("Ui"), SerializeField]
    private MMProgressBar defenseDebuffBar;

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
    private IEnumerator _activeSpeedBuffCoroutine;
    private IEnumerator _activeAttackBuffCoroutine;
    private IEnumerator _activeDefenseBuffCoroutine;
    private IEnumerator _activeSpeedDebuffCoroutine;
    private IEnumerator _activeAttackDebuffCoroutine;
    private IEnumerator _activeDefenseDebuffCoroutine;

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
        if (!_battleManager.battleStarted) return;
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
        int index = Random.Range(0, Enum.GetValues(typeof(ActionType)).Length);
        Agent target;
        switch ((ActionType)index) {
            default:
                target = _battleManager.GetRandomOpponent(_team);
                target.ReceiveDamage(this);
                break;
            case ActionType.Damage:
                target = _battleManager.GetRandomOpponent(_team);
                target.ReceiveDamage(this);
                break;
            case ActionType.Heal:
                target = _battleManager.GetRandomTeammate(_team);
                target.ReceiveHealing();
                break;
            case ActionType.DamageOverTime:
                target = _battleManager.GetRandomOpponent(_team);
                target.ReceiveDamageOverTime();
                break;
            case ActionType.HealOverTime:
                target = _battleManager.GetRandomTeammate(_team);
                target.ReceiveHealingOverTime();
                break;
            case ActionType.SpeedBuff:
                target = _battleManager.GetRandomTeammate(_team);
                target.ReceiveSpeedBuff();
                break;
            case ActionType.AttackBuff:
                target = _battleManager.GetRandomTeammate(_team);
                target.ReceiveAttackBuff();
                break;
            case ActionType.DefenseBuff:
                target = _battleManager.GetRandomTeammate(_team);
                target.ReceiveDefenseBuff();
                break;
            case ActionType.SpeedDebuff:
                target = _battleManager.GetRandomOpponent(_team);
                target.ReceiveSpeedDebuff();
                break;
            case ActionType.AttackDebuff:
                target = _battleManager.GetRandomOpponent(_team);
                target.ReceiveAttackDebuff();
                break;
            case ActionType.DefenseDebuff:
                target = _battleManager.GetRandomOpponent(_team);
                target.ReceiveDefenseDebuff();
                break;
            
        }
    }
    

    #endregion

    #region Receive Actions

    private void ReceiveDamage(Agent attackingAgent) {
        int damage = attackingAgent._currentAttack - _currentDefense;
        if (damage <= 0) return;
        AffectHealth(-damage);
        damageFeedback.PlayFeedbacks();
    }

    private void ReceiveDamageOverTime() {
        if (_activeDamageCoroutine != null) {
            StopCoroutine(_activeDamageCoroutine);
        }

        _activeDamageCoroutine = HandleDamageOverTime();
        StartCoroutine(_activeDamageCoroutine);
    }

    private IEnumerator HandleDamageOverTime() {
        int time = 0;
        while (time < OTTotalTime) {
            yield return new WaitForSeconds(OTIntervalTime);
            AffectHealth(-OTValue);
            tinyDamageFeedback.PlayFeedbacks();
            time += OTIntervalTime;
        }
    }

    private void ReceiveHealing() {
        AffectHealth(HealValue);
        healFeedback.PlayFeedbacks();
    }

    private void ReceiveHealingOverTime() {
        if (_activeHealCoroutine != null) {
            StopCoroutine(_activeHealCoroutine);
        }

        _activeHealCoroutine = HandleHealingOverTime();
        StartCoroutine(_activeHealCoroutine);
    }
    
    private IEnumerator HandleHealingOverTime() {
        int time = 0;
        while (time < OTTotalTime) {
            yield return new WaitForSeconds(OTIntervalTime);
            AffectHealth(OTValue);
            tinyHealFeedback.PlayFeedbacks();
            time += OTIntervalTime;
        } 
    }
    
    //ToDo: Refactor these buffs and debuffs if you have time.
    private void ReceiveSpeedBuff() {
        if (_activeSpeedBuffCoroutine != null) {
            StopCoroutine(_activeSpeedBuffCoroutine);
            _currentSpeed = _agentType.InitialSpeed;
        }

        _activeSpeedBuffCoroutine = HandleSpeedBuff(speedBuffBar, false);
        StartCoroutine(_activeSpeedBuffCoroutine);
    }
    
    private void ReceiveSpeedDebuff() {
        if (_activeSpeedDebuffCoroutine != null) {
            StopCoroutine(_activeSpeedDebuffCoroutine);
            _currentSpeed = _agentType.InitialSpeed;
        }

        _activeSpeedDebuffCoroutine = HandleSpeedBuff(speedDebuffBar, true);
        StartCoroutine(_activeSpeedDebuffCoroutine);
    }
    private IEnumerator HandleSpeedBuff(MMProgressBar bar, bool isDebuff) {
        bar.gameObject.SetActive(true);
        _currentSpeed += isDebuff? -BuffValue : BuffValue;
        float time = BuffTime;
        while (time > 0) {
            bar.UpdateBar(time, 0, BuffTime);
            time -= Time.deltaTime;
            yield return null;
        }

        _currentSpeed = _agentType.InitialSpeed;
        bar.gameObject.SetActive(false);
    }
    
    private void ReceiveAttackBuff() {
        if (_activeAttackBuffCoroutine != null) {
            StopCoroutine(_activeAttackBuffCoroutine);
            _currentAttack = _agentType.InitialAttack;
        }

        _activeAttackBuffCoroutine = HandleAttackBuff(attackBuffBar, false);
        StartCoroutine(_activeAttackBuffCoroutine);
    }
    
    private void ReceiveAttackDebuff() {
        if (_activeAttackDebuffCoroutine != null) {
            StopCoroutine(_activeAttackDebuffCoroutine);
            _currentAttack = _agentType.InitialAttack;
        }

        _activeAttackDebuffCoroutine = HandleAttackBuff(attackDebuffBar, true);
        StartCoroutine(_activeAttackDebuffCoroutine);
    }
    
    private IEnumerator HandleAttackBuff(MMProgressBar bar, bool isDebuff) {
        bar.gameObject.SetActive(true);
        _currentAttack += isDebuff? -BuffValue : BuffValue;
        float time = BuffTime;
        while (time > 0) {
            bar.UpdateBar(time, 0, BuffTime);
            time -= Time.deltaTime;
            yield return null;
        }

        _currentAttack = _agentType.InitialAttack;
        bar.gameObject.SetActive(false);
    }
    
    private void ReceiveDefenseBuff() {
        if (_activeDefenseBuffCoroutine != null) {
            StopCoroutine(_activeDefenseBuffCoroutine);
            _currentDefense = _agentType.InitialDefense;
        }

        _activeDefenseBuffCoroutine = HandleDefenseBuff(defenseBuffBar, false);
        StartCoroutine(_activeDefenseBuffCoroutine);
    }
    
    private void ReceiveDefenseDebuff() {
        if (_activeDefenseDebuffCoroutine != null) {
            StopCoroutine(_activeDefenseDebuffCoroutine);
            _currentDefense = _agentType.InitialDefense;
        }

        _activeDefenseDebuffCoroutine = HandleDefenseBuff(defenseDebuffBar, true);
        StartCoroutine(_activeDefenseDebuffCoroutine);
    }
    
    private IEnumerator HandleDefenseBuff(MMProgressBar bar, bool isDebuff) {
        bar.gameObject.SetActive(true);
        _currentDefense += isDebuff? -BuffValue : BuffValue;
        float time = BuffTime;
        while (time > 0) {
            bar.UpdateBar(time, 0, BuffTime);
            time -= Time.deltaTime;
            yield return null;
        }

        _currentDefense = _agentType.InitialDefense;
        bar.gameObject.SetActive(false);
    }

    private void AffectHealth(int val) {
        _currentHealth += val;
        if (_currentHealth < 0) {
            StopAllCoroutines();
            _battleManager.HandleAgentDeath(this);
        }
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _agentType.InitialHealth);
        healthBar.UpdateBar(_currentHealth, 0, _agentType.InitialHealth);
    }

    #endregion
    
    
    
}
