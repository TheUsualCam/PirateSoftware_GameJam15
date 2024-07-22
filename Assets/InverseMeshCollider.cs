using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class InverseMeshCollider : MonoBehaviour
{
    public Mesh mesh;
    [ContextMenu("Invert Mesh")]
    void InvertMesh()
    {
        MeshCollider collider = GetComponent<MeshCollider>();

        Mesh meshToInvert = mesh;
        
        //Invert Triangles and Normals
        meshToInvert.triangles = meshToInvert.triangles.Reverse().ToArray();
        meshToInvert.normals = meshToInvert.normals.Select(n => -n).ToArray();
    }

    
}
