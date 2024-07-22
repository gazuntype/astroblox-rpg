using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleManager : MonoBehaviour {
    public enum Team{A, B}
    public float battleTime => _battleTime;

    [SerializeField]
    private GameObject agentPrefab;

    [SerializeField]
    private SpriteRenderer teamAHolder;
    [SerializeField]
    private SpriteRenderer teamBHolder;

    [BoxGroup("Ui"), SerializeField]
    private GameObject battleScreen;
    [BoxGroup("Ui"), SerializeField]
    private GameObject hud;
    
    [BoxGroup("Ui"), SerializeField]
    private TeamUi teamAUi;

    [BoxGroup("Ui"), SerializeField]
    private TeamUi teamBUi;
    
    private float _battleTime;
    private List<Agent> _teamA;
    private List<Agent> _teamB;

    private bool _battleStarted;

    public void StartBattle() {
        battleScreen.SetActive(false);
        hud.SetActive(true);
        _teamA = CreateTeam(teamAUi, teamAHolder, Team.A);
        _teamB = CreateTeam(teamBUi, teamBHolder, Team.B);
        _battleStarted = true;
    }

    private void Update() {
        if (!_battleStarted) return;
        _battleTime += Time.deltaTime;
    }

    public Agent GetRandomTeammate(Team team) {
        if (team == Team.A) {
            int index = Random.Range(0, _teamA.Count);
            return _teamA[index];
        } else {
            int index = Random.Range(0, _teamB.Count);
            return _teamB[index];
        }
    }

    public Agent GetRandomOpponent(Team team) {
        if (team == Team.B) {
            int index = Random.Range(0, _teamA.Count);
            return _teamA[index];
        } else {
            int index = Random.Range(0, _teamB.Count);
            return _teamB[index];
        }
    }


    private List<Agent> CreateTeam(TeamUi teamUi, SpriteRenderer teamHolder, Team agentTeam) {
        List<Agent> team = new List<Agent>();
        for (int i = 0; i < teamUi.agentUis.Count; i++) {
            Bounds bounds = teamHolder.bounds;
            Vector2 agentPos = bounds.center;
            agentPos += new Vector2(Random.Range(-bounds.extents.x, bounds.extents.x),
                Random.Range(-bounds.extents.y, bounds.extents.y));
            GameObject newAgent = Instantiate(agentPrefab);
            newAgent.transform.position = agentPos;
            newAgent.transform.SetParent(teamHolder.transform, true);
            Agent agent = newAgent.GetComponent<Agent>();
            agent.InitializeAgent(this, teamUi.agentUis[i].agentType, agentTeam);
            team.Add(agent);
        }

        return team;
    }
}
