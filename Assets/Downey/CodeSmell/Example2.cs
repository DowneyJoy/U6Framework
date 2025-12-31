using System;
using UnityEngine;

namespace Downey.CodeSmell.Example2
{
    public enum EnemyType {Flying,Ground,Boss}
    public class Enemy : MonoBehaviour
    {
        public EnemyType type;

        private void Update()
        {
            if (type == EnemyType.Flying) Fly();
            else if (type == EnemyType.Ground) Walk();
            else Roar();
            //-------------------
            switch (type)
            {
                case EnemyType.Flying: Fly(); break;
                case EnemyType.Ground: Walk(); break;
                case EnemyType.Boss: Roar(); break;
            }
        }
        
        void Fly(){}
        void Walk(){}
        void Roar(){}
    }
}