
using UnityEngine;

namespace Assets.Scripts
{
    public interface IAttacker
    {
        public Vector3 Position { get; }
        public void MoveToTarget(Vector3 position);
        public void Attack();
    }
}
