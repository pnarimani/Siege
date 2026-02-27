using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Gameplay
{
    public static class AsyncExtensions
    {
        public static async void Forget(this Task task)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}