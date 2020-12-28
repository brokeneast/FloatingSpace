/*
 * 用來調整席位位置之介面
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeatingPosSettingsView : MonoBehaviour
{
    [SerializeField] GameObject seatingPosAnchorPrefab = null;//席位位置茅點
    [SerializeField] GameObject seatingPosAnchorPlace = null;
    public List<SeatingAnchor> posAnchors = new List<SeatingAnchor>();

    public void Refresh(List<Seat> seats)
    {
        int distance = Mathf.Abs(posAnchors.Count - seats.Count);
        if (posAnchors.Count > seats.Count)
        {
            for (int i = 0; i < distance; i++)
            {
                Destroy(posAnchors[i].gameObject);
                posAnchors.RemoveAt(0);
            }
        }
        else if (posAnchors.Count < seats.Count)
        {
            for (int i = 0; i < distance; i++)
            {
                GameObject sp = Instantiate(seatingPosAnchorPrefab, seatingPosAnchorPlace.transform);
                SeatingAnchor spAnchor = sp.GetComponent<SeatingAnchor>();
                posAnchors.Add(spAnchor);
            }
        }

        //排列
        for (int i = 0; i < seats.Count; i++)
        {
            posAnchors[i].Init(i, seats[i]);
        }
    }
}
