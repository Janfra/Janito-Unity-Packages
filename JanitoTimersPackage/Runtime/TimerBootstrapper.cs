using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Janito.Timers
{
    internal static class TimerBootstrapper
    {
        private static PlayerLoopSystem _timerSystem;

        /// <summary>
        /// Initialises the TimerManager by inserting it into the PlayerLoopSystem's Update phase.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        internal static void Initialise()
        {
            PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();

            if (!InsertTimerManager<Update>(ref currentPlayerLoop, 0))
            {
                Debug.LogError("Failed to insert TimerManager into Update PlayerLoopSystem. TimerManager not initialised.");
                return;
            }

            PlayerLoop.SetPlayerLoop(currentPlayerLoop);


#if UNITY_EDITOR
            // Avoids inserting multiple times the TimerManager when entering Play Mode multiple times in the Editor.
            static void OnPlayModeState(PlayModeStateChange state)
            {
                if (state == PlayModeStateChange.ExitingPlayMode)
                {
                    PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
                    RemoveTimerManager<Update>(ref currentPlayerLoop);
                    PlayerLoop.SetPlayerLoop(currentPlayerLoop);

                    TimerManager.ClearTimers();
                }
            }

            EditorApplication.playModeStateChanged -= OnPlayModeState;
            EditorApplication.playModeStateChanged -= OnPlayModeState;
#endif
        }

        private static bool InsertSystem<T>(ref PlayerLoopSystem system, in PlayerLoopSystem systemToInsert, int index)
        {
            if (system.type != typeof(T))
            {
                return HandleSubSystemLoop<T>(ref system, in systemToInsert, index);
            }

            var playerLoopSystemList = new List<PlayerLoopSystem>();
            if (system.subSystemList != null || system.subSystemList.Length == 0)
            {
                playerLoopSystemList.AddRange(system.subSystemList);
            }
            playerLoopSystemList.Insert(index, systemToInsert);
            system.subSystemList = playerLoopSystemList.ToArray();
            return true;
        }

        private static bool HandleSubSystemLoop<T>(ref PlayerLoopSystem system, in PlayerLoopSystem systemToInsert, int index)
        {
            if (system.subSystemList == null || system.subSystemList.Length == 0)
            {
                return false;
            }

            for (int i = 0; i < system.subSystemList.Length; i++)
            {
                if (!InsertSystem<T>(ref system.subSystemList[i], in systemToInsert, index))
                {
                    continue;
                }
                return true;
            }

            return false;
        }

        private static bool InsertTimerManager<T>(ref PlayerLoopSystem system, int index)
        {
            _timerSystem = new PlayerLoopSystem
            {
                type = typeof(TimerManager),
                updateDelegate = TimerManager.UpdateTimers,
                subSystemList = null
            };

            return InsertSystem<T>(ref system, in _timerSystem, index);
        }

        private static void RemoveTimerManager<T>(ref PlayerLoopSystem system)
        {
            RemoveSystem<T>(ref system, in _timerSystem);
        }   

        private static void RemoveSystem<T>(ref PlayerLoopSystem system, in PlayerLoopSystem systemToRemove)
        {
            if (system.subSystemList == null || system.subSystemList.Length == 0)
            {
                return;
            }

            var playerLoopSystemList = new List<PlayerLoopSystem>(system.subSystemList);
            for (int i = 0; i < playerLoopSystemList.Count; i++)
            {
                if (playerLoopSystemList[i].type == systemToRemove.type && playerLoopSystemList[i].updateFunction == systemToRemove.updateFunction)
                {
                    playerLoopSystemList.RemoveAt(i);
                    system.subSystemList = playerLoopSystemList.ToArray();
                    return;
                }
            }

            HandleSubSystemLoopForRemoval<T>(ref system, in systemToRemove);
        }

        private static void HandleSubSystemLoopForRemoval<T>(ref PlayerLoopSystem system, in PlayerLoopSystem systemToRemove)
        {
            if (system.subSystemList == null || system.subSystemList.Length == 0)
            {
                return;
            }

            for (int i = 0; i < system.subSystemList.Length; i++)
            {
                RemoveSystem<T>(ref system.subSystemList[i], in systemToRemove);
            }
        }

#if UNITY_EDITOR
        [MenuItem("Tools/Print Current PlayerLoopSystem")]
        private static void PrintPlayerLoopSystemToConsole()
        {
            static void PrintPlayerLoopSystemRecursive(PlayerLoopSystem system, int indentLevel, StringBuilder sb)
            {
                sb.Append(' ', indentLevel * 2).AppendLine($"- {system.type}");
                if (system.subSystemList != null)
                {
                    foreach (var subSystem in system.subSystemList)
                    {
                        PrintPlayerLoopSystemRecursive(subSystem, indentLevel + 1, sb);
                    }
                }
            }

            PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
            StringBuilder sb = new StringBuilder();
            foreach (var subSystem in currentPlayerLoop.subSystemList)
            {
                PrintPlayerLoopSystemRecursive(subSystem, 0, sb);
            }

            Debug.Log(sb.ToString());
        }
#endif
    }
}
