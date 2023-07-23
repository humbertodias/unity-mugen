using System.Linq;
using UnityEngine;

public class FightEngineDatabase
{

    private Texture2D[] m_database;
    private static FightEngineDatabase s_instance;

    public static FightEngineDatabase Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = new FightEngineDatabase();
            return s_instance;
        }
        set
        {
            s_instance = value;
        }
    }
    public FightEngineDatabase()
    {
        m_database = Resources.LoadAll<Texture2D>("TextureDatabase/IconFightEngine");
    }
    public Texture2D GetTexture(string name)
    {
        Texture2D found = m_database.Where((x) => x.name == name).FirstOrDefault();
        if (found == null)
            found = new Texture2D(2, 2);
        return found;
    }
}
