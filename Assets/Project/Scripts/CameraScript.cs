using System;
using UnityEngine;


public class CameraScript : MonoBehaviour
{
    //  MEMBERS
    public Action PostRenderAction;


    //  METHODS
    private void OnPostRender()
    {
        if(PostRenderAction!=null)
        {
            PostRenderAction();
        }
    }
}
