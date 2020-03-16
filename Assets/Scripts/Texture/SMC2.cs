using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMC2 : MonoBehaviour {

    public void datStart(Color body, Color HC1, Color HC2, Color HC3, Color eyes, Color magic, int cutie, int tai, int bHair, int fHair)
    {
        Material mat = transform.parent.Find("Character_Wings").GetComponent<SkinnedMeshRenderer>().sharedMaterial;
        CombineMeshes(gameObject, mat);
        //transform.GetComponent<Merge>().enabled = true;
        GetComponent<Merge>().MergeThat(body, HC1, HC2, HC3, eyes, magic, cutie, tai, bHair, fHair);
        transform.parent.GetComponent<Animator>().Rebind(); //naprawia po calym zabiegu nasza aniamcje
    }

    public static void CombineMeshes(GameObject o, Material m)
    {
        List<Transform> bones = new List<Transform>();
        List<BoneWeight> boneWeights = new List<BoneWeight>();
        List<CombineInstance> combineInstances = new List<CombineInstance>();
        List<Texture2D> textures = new List<Texture2D>();
        List<SkinnedMeshRenderer> smRenderers = new List<SkinnedMeshRenderer>();
        List<Matrix4x4> bindPoses = new List<Matrix4x4>();
        bones.Clear();
        boneWeights.Clear();
        combineInstances.Clear();
        textures.Clear();
        smRenderers.Clear();

        o.GetComponentsInChildren<SkinnedMeshRenderer>(true, smRenderers);


        int boneOffset = 0;
        for (int s = 0; s < smRenderers.Count; s++)
        {
            SkinnedMeshRenderer smr = smRenderers[s];

            BoneWeight[] meshBoneweight = smr.sharedMesh.boneWeights;

            // May want to modify this if the renderer shares bones as unnecessary bones will get added.

            for (int i = 0; i < meshBoneweight.Length; ++i)
            {
                BoneWeight bWeight = meshBoneweight[i];

                bWeight.boneIndex0 += boneOffset;
                bWeight.boneIndex1 += boneOffset;
                bWeight.boneIndex2 += boneOffset;
                bWeight.boneIndex3 += boneOffset;

                boneWeights.Add(bWeight);
            }

            boneOffset += smr.bones.Length;

            Transform[] meshBones = smr.bones;




            for (int i = 0; i < meshBones.Length; ++i)
            {
                bones.Add(meshBones[i]);

                //we take the old bind pose that mapped from our mesh to world to bone,
                //and take out our localToWorldMatrix, so now it's JUST the bone matrix
                //since our skinned mesh renderer is going to be on the root of our object that works
                bindPoses.Add(smr.sharedMesh.bindposes[i] * smr.transform.worldToLocalMatrix);
            }

            if (smr.material.mainTexture != null)
            {
                textures.Add(smr.material.mainTexture as Texture2D);
            }

            CombineInstance ci = new CombineInstance();
            ci.mesh = smr.sharedMesh;
            ci.transform = smr.transform.localToWorldMatrix;
            combineInstances.Add(ci);



            GameObject.DestroyImmediate(smr);
        }



        SkinnedMeshRenderer r = o.AddComponent<SkinnedMeshRenderer>();
        r.sharedMesh = new Mesh();
        r.sharedMesh.CombineMeshes(combineInstances.ToArray(), true, true);

        Texture2D mainTexture = textures.Count > 0 ? textures[0] : null;
        Vector2[] originalUVs = r.sharedMesh.uv;



        Material combinedMat = m;
        combinedMat.mainTexture = mainTexture;
        r.sharedMaterial = combinedMat;

        r.bones = bones.ToArray();
        r.sharedMesh.boneWeights = boneWeights.ToArray();
        r.sharedMesh.bindposes = bindPoses.ToArray();
        r.sharedMesh.RecalculateBounds();

        foreach(Transform a in o.transform)
        {
            Destroy(a.gameObject);
        }
    }
}
