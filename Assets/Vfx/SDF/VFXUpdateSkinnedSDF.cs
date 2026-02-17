using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.VFX;
using UnityEngine.VFX.SDF;

public class VFXUpdateSkinnedSDF : MonoBehaviour
{
        MeshToSDFBaker m_Baker;
        public SkinnedMeshRenderer m_SkinnedMeshRenderer;
        Mesh m_Mesh;
        public VisualEffect m_Vfx;
        public int maxResolution = 64;
        Vector3 center;
        Vector3 boxSize;
        public int signPassCount = 1;
        public float threshold = 0.5f;
        
        
    // Start is called before the first frame update
    void Start()
    {
        m_Mesh = new Mesh();
        
        if (m_Vfx == null)
        {
            m_Vfx = GetComponent<VisualEffect>();
        }

        if (m_SkinnedMeshRenderer == null)
        {
            m_SkinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        }

        m_SkinnedMeshRenderer.BakeMesh(m_Mesh);

        center = m_SkinnedMeshRenderer.bounds.center;
        boxSize = m_SkinnedMeshRenderer.bounds.size;
        m_Baker = new MeshToSDFBaker(boxSize, center, maxResolution, m_Mesh, signPassCount, threshold);
        m_Baker.BakeSDF();
        
        //m_Vfx.SetTexture("SDF", m_Baker.SdfTexture);
        //m_Vfx.SetVector3("SDFBoxSize", m_Baker.GetActualBoxSize());
    }

    // Update is called once per frame
    void Update()
    {
        // m_SkinnedMeshRenderer.BakeMesh(m_Mesh);
        //
        // m_Baker.BakeSDF();
        //
        //
        // m_Vfx.SetVector3("SDFCenter", center);
        // m_Vfx.SetVector3("SDFBoxSize", m_Baker.GetActualBoxSize());
        //
        // m_Vfx.SetTexture("WalkingSDF", m_Baker.SdfTexture);
        // m_Vfx.SetTexture("WalkingSDFConform", m_Baker.SdfTexture);


        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            m_SkinnedMeshRenderer.BakeMesh(m_Mesh);

            m_Baker.BakeSDF();
            
            //center = m_SkinnedMeshRenderer.bounds.center;
            
            m_Vfx.SetVector3("SDF Center", center);
            m_Vfx.SetVector3("SDF BoxSize", m_Baker.GetActualBoxSize());
            
            m_Vfx.SetVector3("SDF Center Offset", gameObject.transform.position);
            m_Vfx.SetVector3("SDF Rotation", gameObject.transform.rotation.eulerAngles);

            m_Vfx.SetTexture("SDF", m_Baker.SdfTexture);
            m_Vfx.SetBool("IsConformToShape",true);
            // m_Vfx.SetTexture("WalkingSDFConform", m_Baker.SdfTexture);
            
            m_Vfx.SendEvent("Activate");
        }
        else
        {
            // m_Vfx.SetBool("IsConformToShape",false);
            m_Vfx.SendEvent("Deactivate");
            
        }

        // if (Keyboard.current.kKey.IsPressed())
        // {
        //     Debug.Log("PRESS");
        //     m_Vfx.SetTexture("WalkingSDFConform", m_Baker.SdfTexture);
        // }
        // else
        // {
        //     m_Vfx.SetTexture("WalkingSDFConform", sphereSDF);
        // }

    }
    
    void OnDestroy()
    {
        if (m_Baker != null)
        {
            m_Baker.Dispose();
        }
    }
}
