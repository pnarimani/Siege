using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace Siege.Gameplay
{
    public static class FixedUpdateRunner
    {
        private static List<Action> _callbacks = new List<Action>();
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            _callbacks.Clear();
            
            
            var loop = PlayerLoop.GetCurrentPlayerLoop();
            
            RemoveFixedUpdateSystem(ref loop, typeof(FixedUpdateRunner));
            
            InsertFixedUpdateSystem(ref loop, new PlayerLoopSystem
            {
                type = typeof(FixedUpdateRunner),
                updateDelegate = OnFixedUpdate,
            });
            PlayerLoop.SetPlayerLoop(loop);
        }

        static void OnFixedUpdate()
        {
            foreach (var callback in _callbacks)
            {
                callback.Invoke();
            }
        }

        static void InsertFixedUpdateSystem(ref PlayerLoopSystem loop, PlayerLoopSystem systemToInsert)
        {
            for (var i = 0; i < loop.subSystemList.Length; i++)
            {
                if (loop.subSystemList[i].type == typeof(FixedUpdate))
                {
                    var subSystems = loop.subSystemList[i].subSystemList;
                    var newSubSystems = new PlayerLoopSystem[subSystems.Length + 1];
                    subSystems.CopyTo(newSubSystems, 0);
                    newSubSystems[subSystems.Length] = systemToInsert;
                    loop.subSystemList[i].subSystemList = newSubSystems;
                    return;
                }
            }
        }

        static void RemoveFixedUpdateSystem(ref PlayerLoopSystem loop, System.Type typeToRemove)
        {
            for (int i = 0; i < loop.subSystemList.Length; i++)
            {
                if (loop.subSystemList[i].type == typeof(FixedUpdate))
                {
                    var subSystems = loop.subSystemList[i].subSystemList;
                    var newSubSystems = new List<PlayerLoopSystem>(subSystems);
                    newSubSystems.RemoveAll(s => s.type == typeToRemove);
                    loop.subSystemList[i].subSystemList = newSubSystems.ToArray();
                    return;
                }
            }
        }

        public static void Remove(Action callback)
        {
            _callbacks.Remove(callback);
        }

        public static void Add(Action callback)
        {
            _callbacks.Add(callback);
        }
    }
}