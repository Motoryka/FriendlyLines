using UnityEngine;
using System;

public class Result
{
    public int shapeCovering {get; set;}
    public int errorRange { get; set; }

    public Result(int shape, int error)
    {
        this.shapeCovering = shape;
        this.errorRange = error;
    }
}
