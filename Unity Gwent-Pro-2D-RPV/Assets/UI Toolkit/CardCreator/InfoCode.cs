using System;

[Serializable]
public class InfoCode 
{
    public string code;

    public InfoCode( string code )
    {
        this.code = code;
    }
    public InfoCode()
    {
        code = "";
    }
}
