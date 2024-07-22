using UnityEngine;
using UnityEngine.AI;

namespace UnityExtension.Runtime.Projects.unity_extension.Scripts.Runtime.Utils.Extensions
{
    public static class NavMeshAgentExtensions
    {
        public static bool IsArrived(this NavMeshAgent agent, float tolerance)
        {
            return Mathf.Abs((agent.transform.position - agent.destination).magnitude) <= tolerance;
        }
    }
}