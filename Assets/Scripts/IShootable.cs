using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootable 
{
    GameObject gameObject { get; }
    Transform transform { get; }

    GameObject Shooter { get; set; }
}
