using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;
public class YarnVariableStorage : VariableStorageBehaviour
{

    // Make this class a singleton
    public static YarnVariableStorage Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public override void Clear()
    {
        throw new System.NotImplementedException();
    }

    public override bool Contains(string variableName)
    {
        throw new System.NotImplementedException();
    }

    public override (Dictionary<string, float> FloatVariables, Dictionary<string, string> StringVariables, Dictionary<string, bool> BoolVariables) GetAllVariables()
    {
        throw new System.NotImplementedException();
    }

    public override void SetAllVariables(Dictionary<string, float> floats, Dictionary<string, string> strings, Dictionary<string, bool> bools, bool clear = true)
    {
        throw new System.NotImplementedException();
    }

    public override void SetValue(string variableName, string stringValue)
    {
        throw new System.NotImplementedException();
    }

    public override void SetValue(string variableName, float floatValue)
    {
        throw new System.NotImplementedException();
    }

    public override void SetValue(string variableName, bool boolValue)
    {
        throw new System.NotImplementedException();
    }

    public override bool TryGetValue<T>(string variableName, out T result)
    {
        throw new System.NotImplementedException();
    }


}
