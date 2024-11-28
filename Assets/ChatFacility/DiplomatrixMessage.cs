using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiplomatrixMessage
{
    private string _author ;

    public string Author 
    {
        get { return _author; }
        private set { _author = value; }
    }

    private string _content ;

    public string Content 
    {
        get { return _content; }
        private set { _content = value; }
    }

    public DiplomatrixMessage(string author, string content){
        _author = author;
        _content = content;
    }
     public override string ToString()
    {
        return $"{Author}: {Content}";
    }
}