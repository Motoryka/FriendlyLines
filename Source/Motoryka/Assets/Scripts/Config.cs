using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

//Config test class
public class Config
{
	public int ConfigNr { get; set; }
	public int UserId { get; set; }
	public string Username { get; set; }
	public string TeacherName { get; set; }
	public DateTime CreateDate { get; set; }
	public int NrOfLevels { get; set; }

    public List<Shape> Shapes { get; set; }
}
