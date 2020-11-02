using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

    public class ImagePage : Menu
    {
        // ===================================================================
        void OnEnable()
        {

        }

        // ===================================================================
        void OnDisable()
        {

        }

        // ===================================================================
        public override void PlayIn(string comingFromMenu)
        {
            base.PlayIn(comingFromMenu);
        }


        // ===================================================================
        public override void PlayInComplete()
        {
            base.PlayInComplete();
        }

        // ===================================================================
        public override void PlayOut(string nextMenu)
        {
            base.PlayOut(nextMenu);
        }
    }
