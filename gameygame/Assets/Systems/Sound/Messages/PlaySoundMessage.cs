using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundMessage {
    private readonly string _name;
    private readonly string _tag;

    public string Name { get { return _name; } }
    public string Tag { get { return _tag; } }


    public PlaySoundMessage(string name, string tag = null)
    {
        _name = name;
    }

}
