using System.Collections;
using System.Collections.Generic;
using Malimbe.MemberChangeMethod;
using Malimbe.MemberClearanceMethod;
using Malimbe.PropertySerializationAttribute;
using Malimbe.XmlDocumentationAttribute;
using System;
using Tilia.Interactions.Interactables.Interactables;
using UnityEngine;
using UnityEngine.Events;
using Zinnia.Data.Attribute;
using Zinnia.Data.Type;
using Zinnia.Rule;
using Tilia.Interactions.SnapZone;

public class UnsnapSnapzoneAboveOnDeactivation : MonoBehaviour
{

    public SnapZoneConfigurator conf;
    // Start is called before the first frame update

    public UnsnapSnapzoneAboveOnDeactivation()
    {
        conf.Unsnap();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
