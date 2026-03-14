using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public interface IInteractable
    {
        string Message { get; }
        void Interact();
        void EnableOutline();
        void DisableOutline();
    }
}
