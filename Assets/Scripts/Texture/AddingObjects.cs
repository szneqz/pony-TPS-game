using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddingObjects : MonoBehaviour
{
    //public int[] hairRandom = new int[3];
    //private ContrMovem mainGO;

    public GameObject lastParent;
    public List<GameObject> objects;

    //private SkinnedMeshRenderer tailSMR;
    //private SkinnedMeshRenderer backhairSMR;
    //private SkinnedMeshRenderer fornthairSMR;

    private Animator anim;

    public void MeshUpdate(int tail, int backHair, int frontHair, Color body, Color HC1, Color HC2, Color HC3, Color eyes, Color magic, int cutie, int tai, int bHair, int fHair)
    {
        anim = transform.parent.GetComponent<Animator>();

        //Debug.Log("EMMMMM  " + transform.parent.name);
        //if (objects[0] == null || objects[1] == null || objects[2] == null)
        //    return;

        objects[0].GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = StaticInfo.datScript.tailSMRList[tail].sharedMesh;
        objects[1].GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = StaticInfo.datScript.backHairSMRList[backHair].sharedMesh;
        objects[2].GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = StaticInfo.datScript.frontHairSMRList[frontHair].sharedMesh;

        Vector3 actualpos = transform.parent.position;      //zapamietuje polozenie i rotacje
        Quaternion actualrot = transform.parent.rotation;   //

        transform.parent.position = Vector3.zero;           //resetuje polozenie i rotacje
        transform.parent.rotation = Quaternion.identity;    //

        GameObject parent = transform.parent.gameObject;

        for (int i = 0; i < 3; i++)
        {
            objects[i] = Instantiate(objects[i]);   //spawnuje czesc
            AddLimb(objects[i], parent);            //dolaczam ja do postaci (ta zinstatiowana sie dubluje)
            Destroy(objects[i]);                    //niszcze stara czesc
        }

        transform.parent.position = actualpos;      //odnawiam polozenie i rotacje
        transform.parent.rotation = actualrot;      //

        GetComponent<SMC2>().datStart(body, HC1, HC2, HC3, eyes, magic, cutie, tai, bHair, fHair);        //zalaczam SkinnedMeshCombiner
        anim.Rebind();
    }

    void AddLimb(GameObject BonedObj, GameObject RootObj)
    {
        SkinnedMeshRenderer[] BonedObjects = BonedObj.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer SkinnedRenderer in BonedObjects)
            ProcessBonedObject(SkinnedRenderer, RootObj, lastParent);
    }

    private void ProcessBonedObject(SkinnedMeshRenderer ThisRenderer, GameObject RootObj, GameObject lastParent)
    {
        /*      Create the SubObject        */
        GameObject NewObj = new GameObject(ThisRenderer.gameObject.name);
        NewObj.transform.parent = RootObj.transform;
        /*      Add the renderer        */
        NewObj.AddComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer NewRenderer = NewObj.GetComponent<SkinnedMeshRenderer>();
        /*      Assemble Bone Structure     */
        Transform[] MyBones = new Transform[ThisRenderer.bones.Length];
        for (int i = 0; i < ThisRenderer.bones.Length; i++)
            MyBones[i] = FindChildByName(ThisRenderer.bones[i].name, RootObj.transform);
        /*      Assemble Renderer       */
        NewRenderer.bones = MyBones;
        NewRenderer.sharedMesh = ThisRenderer.sharedMesh;
        NewRenderer.materials = ThisRenderer.materials;
        //moj dopisek - zmiana parenta na koniec
        NewObj.transform.parent = lastParent.transform;
        NewObj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    private Transform FindChildByName(string ThisName, Transform ThisGObj)
    {
        Transform ReturnObj;
        if (ThisGObj.name == ThisName)
            return ThisGObj.transform;
        foreach (Transform child in ThisGObj)
        {
            ReturnObj = FindChildByName(ThisName, child);
            if (ReturnObj)
                return ReturnObj;
        }
        return null;
    }
}


