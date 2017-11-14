using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDropable{

	string[] DropItems { get; set; }
    float DropProbability { get; set; }

    void Drop(float probability);
}
