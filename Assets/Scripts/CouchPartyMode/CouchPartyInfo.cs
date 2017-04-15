using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CouchPartyInfo : MonoBehaviour {

    public Canvas levelSel;

    void Awake()
    {
        if (CouchPartyManager.IsCouchPartyMode)
        {
            levelSel.gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (SimpleInput.GetAnyButtonDown())
        {
            levelSel.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
	}
}
