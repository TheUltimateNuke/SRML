﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace SRML.SR.Utils.Debug
{
    public class DebugSystem : MonoBehaviour
    {
        private RaycastHit mainHit;

        private static string GetPath(Transform transform)
        {
            if (transform == null)
            {
                return "null";
            }
            else
            {
                string path = transform.name;
                while (transform.parent != null)
                {
                    transform = transform.parent;
                    path = transform.name + "/" + path;
                }
                return path;
            }
        }

        public void Update()
        {
            if (!DebugCommand.DebugMode)
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            bool def = Physics.queriesHitTriggers;
            Physics.queriesHitTriggers = true;

            foreach (RaycastHit hit in Physics.RaycastAll(ray, 5f))
            {
                if (hit.collider.GetComponent<DebugMarker>() != null)
                    hit.collider.GetComponent<DebugMarker>().SetHover();
            }

            Physics.queriesHitTriggers = def;

            Physics.Raycast(ray, out mainHit, 3000f);
        }

        public void OnGUI()
        {
            if (!DebugCommand.DebugMode)
                return;

            GUILayout.BeginArea(new Rect(15, 250, 450, Screen.height - 400));

            // Title
            GUILayout.Label("<b>DEBUG MODE ACTIVE</b>");
            GUILayout.Space(5);

            // SCENE CONTEXT STUFF
            if (SceneManager.GetActiveScene().name.Equals("worldGenerated"))
            {
                // Vars
                ZoneDirector.Zone zone = SceneContext.Instance.PlayerZoneTracker.GetCurrentZone();
                bool mainHitNull = mainHit.Equals(default(RaycastHit));

                // Player Info
                GUILayout.Label($"<size=20><b>Player Info:</b></size>");
                GUILayout.Space(3);
                GUILayout.Label($"<b>Position: </b>{SceneContext.Instance.Player.transform.position}");
                GUILayout.Label($"<b>Camera Rotation: </b>{Camera.main.transform.eulerAngles}");
                GUILayout.Label($"<b>Zone: </b>{zone}");
                GUILayout.Label($"<b>Map Unlocked: </b>{SceneContext.Instance.PlayerState.HasUnlockedMap(zone)}");
                GUILayout.Label($"<b>EndGame Time: </b>{SceneContext.Instance.PlayerState.GetEndGameTime()}");
                GUILayout.Label($"<b>Ammo Mode: </b>{SceneContext.Instance.PlayerState.GetAmmoMode()}");
                GUILayout.Label($"<b>Look At: </b>" + (mainHitNull ? "None" : $"{mainHit.point}"));
                GUILayout.Space(20);
                if (!mainHitNull)
                {
                    // Object Panel
                    GUILayout.Label($"<size=20><b>Object Inspector:</b></size>");
                    GUILayout.Space(3);
                    GUILayout.Label($"<b>Path: </b>{GetPath(mainHit.collider?.transform)}");
                    GUILayout.Label($"<b>Position: </b>{mainHit.collider?.transform.position}");
                    GUILayout.Label($"<b>Rotation: </b>{mainHit.collider?.transform.rotation}");
                    GUILayout.Label($"<b>Scale: </b>{mainHit.collider?.transform.localScale}");
                    Identifiable.Id id = Identifiable.GetId(mainHit.collider.gameObject);
                    if (id != Identifiable.Id.NONE) GUILayout.Label($"<b>Identifiable.Id: </b>{id}");
                }
            }

            GUILayout.EndArea();
        }
    }
}
