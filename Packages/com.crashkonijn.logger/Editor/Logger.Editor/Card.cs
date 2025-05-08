using System;
using UnityEngine.UIElements;

namespace CrashKonijn.Logger.Editor
{
    internal class Card : VisualElement
    {
        public Card(Action<Card> callback)
        {
            this.name = "card";
            callback(this);
        }
    }
}