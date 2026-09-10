using System;
using System.Collections.Generic;
using GamePlay.Units;
using UnityEngine;

namespace GamePlay.Fractions
{
    public class Fraction : MonoBehaviour
    {
        [SerializeField] private FractionConfig fractionConfig;
        [SerializeField] private Transform unitsParent;
        
        private List<Unit> units = new List<Unit>();
        
        public IReadOnlyList<Unit> Units => units.AsReadOnly();
        public event Action<Unit> OnUnitCreated;
        
        private const float SpawnRadius = 5f;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, SpawnRadius);
        }

        private void Start()
        {
            CreateUnits();
        }

        private void CreateUnits()
        {
            foreach (var unit in fractionConfig.Units)
            {
                var startingPosition = transform.position;
                var position = new Vector3(UnityEngine.Random.Range(startingPosition.x - SpawnRadius, startingPosition.x + SpawnRadius),
                    0, UnityEngine.Random.Range(startingPosition.z - SpawnRadius, startingPosition.z + SpawnRadius));
                var unitInstance = Instantiate(unit.OverriddenPrefab ?? fractionConfig.BasePrefab, position, Quaternion.identity, unitsParent);
                
                unitInstance.InitConfig(unit.UnitConfig);
                unitInstance.name = unit.UnitConfig.ToString();
                
                units.Add(unitInstance);
                OnUnitCreated?.Invoke(unitInstance);
            }
        }

        public void OnUpdate()
        {
            
        }
    }
}