using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleCreator : MonoBehaviour
{
    [SerializeField] GameObject characterPrefab = null;
    public List<Texture> skins = new List<Texture>();

    public GameObject CreateRole(RoleInfo role)
    {
        GameObject r = Instantiate(characterPrefab);
        if (role.skin< skins.Count && skins[role.skin]!=null)
        {
            r.GetComponent<MeshRenderer>().material.mainTexture =  skins[role.skin];
        }
        return r;
    }
}
