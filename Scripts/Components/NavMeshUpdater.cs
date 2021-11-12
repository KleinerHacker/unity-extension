using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace UnityExtension.Runtime.extension.Scripts.Components
{
    [AddComponentMenu(UnityExtensionConstants.Root + "/NavMesh Updater")]
    [DisallowMultipleComponent]
    public class NavMeshUpdater : MonoBehaviour
    {
        private NavMeshData _navMeshData;
        private NavMeshBuildSettings _settings;

        #region Builtin Methods

        protected virtual void OnEnable()
        {
            _navMeshData = new NavMeshData();
            _settings = NavMesh.GetSettingsByID(0);
            NavMesh.AddNavMeshData(_navMeshData);
        }

        #endregion
        
        public void UpdateNavMesh(Bounds bounds)
        {
            var sourceList = CollectSources(bounds);
            NavMeshBuilder.UpdateNavMeshDataAsync(_navMeshData, _settings, sourceList, bounds);
        }

        // Collect all the navmesh build sources for enabled objects tagged by this component
        private static List<NavMeshBuildSource> CollectSources(Bounds bounds)
        {
            //TODO: Optimize source collection by bounds
            
            var list = new List<NavMeshBuildSource>();
            list.AddRange(
                FindObjectsOfType<MeshFilter>()
                    .Where(x => x != null && x.sharedMesh != null)
                    .Select(x => new NavMeshBuildSource
                        {
                            shape = NavMeshBuildSourceShape.Mesh,
                            sourceObject = x.sharedMesh,
                            transform = x.transform.localToWorldMatrix,
                            area = 0
                        }
                    )
            );
            list.AddRange(
                FindObjectsOfType<Terrain>()
                    .Where(x => x != null)
                    .Select(x => new NavMeshBuildSource
                        {
                            shape = NavMeshBuildSourceShape.Terrain,
                            sourceObject = x.terrainData,
                            transform = Matrix4x4.TRS(x.transform.position, Quaternion.identity, Vector3.one),
                            area = 0
                        }
                    )
            );

            return list;
        }
    }
}