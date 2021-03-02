using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Quest
{
    public class DomofonButton : Interactable
    {
        public string number;
        public Domofon domofon;

        protected override void Activate()
        {
            domofon.EnterNumber(number);
        }
    }
}