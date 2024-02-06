using UnityEngine;

public class HideIfAttribute : PropertyAttribute
{
    public readonly string ConditionalSourceField;
    public readonly bool HideInInspector;
    
    public HideIfAttribute(string conditionalSourceField, bool hideInInspector = true)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = hideInInspector;
    }
}