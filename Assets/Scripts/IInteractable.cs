using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    interface IInteractable
    {
        string Description { get; }
        void Interact();
    }
}
