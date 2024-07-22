using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))][RequireComponent(typeof(MeshRenderer))][RequireComponent(typeof(MeshCollider))]
public class InverseMeshCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MeshFilter filter = GetComponent<MeshFilter>();
        
        Mesh meshToInvert = filter.sharedMesh;
        
        //Invert Triangles and Normals
        meshToInvert.triangles = meshToInvert.triangles.Reverse().ToArray();
        meshToInvert.normals = meshToInvert.normals.Select(n => -n).ToArray();
    }

    
}
