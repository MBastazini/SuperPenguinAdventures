using UnityEngine;
using System;
using NUnit.Framework;
using UnityEngine.Events;

public enum ComponentType
{
    Health,
    // Adicione outros tipos aqui conforme necessário
}


public class GetScriptsOfClass : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private ComponentType selectedType;

    public UnityEvent<Component[]> ReturnAllScriptsOfSelectedType;

    void Awake()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target object is not assigned.");
            return;
        }

        Type type = GetTypeFromEnum(selectedType);
        if (type == null)
        {
            Debug.LogError($"No valid type found for {selectedType}");
            return;
        }

        Component[] components = targetObject.GetComponents(type);
        //print($"Found {components.Length} components of type {type.Name} on {targetObject.name}");
        Health healthScript = components[0] as Health;
        //print($"Health script found: {healthScript.GetHealthAndMaxHealth()}");
        ReturnAllScriptsOfSelectedType?.Invoke(components);
    }

    private Type GetTypeFromEnum(ComponentType typeEnum)
    {
        switch (typeEnum)
        {
            case ComponentType.Health:
                return typeof(Health);
            // Adicione mais casos conforme necessário
            default:
                return null;
        }
    }

}
