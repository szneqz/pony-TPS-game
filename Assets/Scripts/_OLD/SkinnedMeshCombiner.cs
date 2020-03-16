using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinnedMeshCombiner : MonoBehaviour
{
    //pamietac by w blenderze wszystkie wartosci zaAPPLYiowac!!!!!!!!!!

    //Opis dzialania
    //Skrypt pobiera informacje o aktualnej skali, pozycji i obrocie zapisujac je
    //Pobiera informacje o teksturze (tekstura jest taka sama dla calej postaci)
    //Łączy w tajemniczy sposob wszystkie meshe (bone weight, verticle)
    //wypluwa to i ustawia skale, pozycje i obrot na ten z poczatku
    //
    //jest wazne, poniewaz skrypt dziala tylko i wylacznie w podstawowej skali, pozycji i obrocie!
    //
    //pamietac by nigdy nie usuwac kosci!!!!!!!!

    public Texture2D publicTexture;

    public void Code()
    {
            Vector3 oldScale = transform.parent.localScale;
            Vector3 oldPostion = transform.parent.position; //pobranie starych zmiennych
            Quaternion oldRotation = transform.parent.rotation;
            Transform armature = transform.parent.Find("Armature");
            Quaternion armatureOldRotation = armature.localRotation;
            armature.localRotation = Quaternion.identity;
            transform.parent.position = Vector3.zero; //ustawiam parenta na (0, 0, 0), bo tam sie nie zbuguje
            transform.parent.rotation = Quaternion.identity;    //resetuje jego obrot
            transform.parent.localScale = new Vector3(1, 1, 1); //restuje skale
            Transform Character = transform.parent.Find("Character_Wings");
            //  Material oldMaterial = Character.GetComponent<SkinnedMeshRenderer>().sharedMaterial;
            Material newMaterial = Character.GetComponent<SkinnedMeshRenderer>().sharedMaterial;
            //   Character.GetComponent<SkinnedMeshRenderer>().sharedMaterial = newMaterial;

            List<Transform> bones = new List<Transform>();
            SkinnedMeshRenderer[] smRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            List<BoneWeight> boneWeights = new List<BoneWeight>();
            List<CombineInstance> combineInstances = new List<CombineInstance>();
            List<Texture2D> textures = new List<Texture2D>();
            int numSubs = 0;

            foreach (SkinnedMeshRenderer smr in smRenderers)
                numSubs += smr.sharedMesh.subMeshCount;

            int[] meshIndex = new int[numSubs];
            int boneOffset = 0;
            for (int s = 0; s < smRenderers.Length; s++)
            {
                SkinnedMeshRenderer smr = smRenderers[s];

                BoneWeight[] meshBoneweight = smr.sharedMesh.boneWeights;

                // May want to modify this if the renderer shares bones as unnecessary bones will get added.
                foreach (BoneWeight bw in meshBoneweight)
                {
                    BoneWeight bWeight = bw;

                    bWeight.boneIndex0 += boneOffset;
                    //    bWeight.boneIndex1 += boneOffset;
                    //    bWeight.boneIndex2 += boneOffset;
                    //    bWeight.boneIndex3 += boneOffset;

                    boneWeights.Add(bWeight);
                }
                boneOffset += smr.bones.Length;

                Transform[] meshBones = smr.bones;
                foreach (Transform bone in meshBones)
                    bones.Add(bone);

                if (smr.material.mainTexture != null)
                    textures.Add(smr.GetComponent<Renderer>().material.mainTexture as Texture2D);

                CombineInstance ci = new CombineInstance();
                ci.mesh = smr.sharedMesh;
                meshIndex[s] = ci.mesh.vertexCount;
                ci.transform = smr.transform.localToWorldMatrix;
                combineInstances.Add(ci);

                Object.Destroy(smr.gameObject);
            }

            List<Matrix4x4> bindposes = new List<Matrix4x4>();

            for (int b = 0; b < bones.Count; b++)
            {
                bindposes.Add(bones[b].worldToLocalMatrix * transform.worldToLocalMatrix);
            }

            SkinnedMeshRenderer r;
            if (gameObject.GetComponent<SkinnedMeshRenderer>() == null)
                r = gameObject.AddComponent<SkinnedMeshRenderer>();
            else
                r = gameObject.GetComponent<SkinnedMeshRenderer>();

            r.sharedMesh = new Mesh();
            r.sharedMesh.CombineMeshes(combineInstances.ToArray(), true, true);

            Texture2D skinnedMeshAtlas = new Texture2D(1024, 1024);
            Rect[] packingResult = skinnedMeshAtlas.PackTextures(textures.ToArray(), 0);
            Vector2[] originalUVs = r.sharedMesh.uv;
            Vector2[] atlasUVs = new Vector2[originalUVs.Length];

            int rectIndex = 0;
            int vertTracker = 0;
            for (int i = 0; i < atlasUVs.Length; i++)
            {
                atlasUVs[i].x = Mathf.Lerp(packingResult[rectIndex].xMin, 1.0f, originalUVs[i].x);  //tu wymuszam 1.0f dzieki czemu nie ma tego glupiego paska czarnego
                atlasUVs[i].y = Mathf.Lerp(packingResult[rectIndex].yMin, packingResult[rectIndex].yMax, originalUVs[i].y);

                if (i >= meshIndex[rectIndex] + vertTracker)
                {
                    vertTracker += meshIndex[rectIndex];
                    rectIndex++;
                }
            }

            //    Material combinedMat = new Material(Shader.Find("Diffuse"));    
            //        combinedMat.mainTexture = skinnedMeshAtlas;
            r.sharedMesh.uv = atlasUVs;
            r.sharedMaterial = newMaterial; //od razu podrzucam prawidlowa teksture

            r.bones = bones.ToArray();
            r.sharedMesh.boneWeights = boneWeights.ToArray();
            r.sharedMesh.bindposes = bindposes.ToArray();
            r.sharedMesh.RecalculateBounds();
            r.sharedMesh.RecalculateNormals();
            r.quality = SkinQuality.Bone1;      //ilosc kosci oddzialywujacych na vertex to 1
                                                //    r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;  //nie castuje cieni


            transform.parent.position = oldPostion; //nadanie poprzedniego obrotu
            transform.parent.rotation = oldRotation;
            transform.parent.localScale = oldScale;
            armature.localRotation = armatureOldRotation;

            transform.GetComponent<Merge>().enabled = true;
    }
}
