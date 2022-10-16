using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Testing
{
    /// <summary>
    /// Activate will spread out sub-function objects that are clickable and correspond to different feedback functions
    /// Click on one of these sub-function objects will close all sub-function objects and activate the feedback
    /// E.g. The user input will start rotating / translating the object / the color of the object will be changed.
    /// </summary>
    public class TestSubfunctionTouchable2DObject : BaseTouchable2DObject
    {
        protected override void ActivateObject()
        {
            base.ActivateObject();
            
            
        }

        /// <summary>
        /// Expand the sub function objects and inject this object to them.
        /// </summary>
        private void ExpandSubFunctionObjects()
        {
            
        }
        
        /// <summary>
        /// Close all sub function objects
        /// </summary>
        private void CloseSubFunctionObjects()
        {
            
        }
    }   
}
