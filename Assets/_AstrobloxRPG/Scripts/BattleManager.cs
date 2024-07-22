using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class BattleManager : MonoBehaviour {

    public float battleTime => _battleTime;

    [SerializeField]
    private TeamUi teamA;

    [SerializeField]
    private TeamUi teamB;
    
    private List<Agent> _players;
    private List<Agent> _enemies;
    

    private float _battleTime;


}
