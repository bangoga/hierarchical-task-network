using System;
using System.Collections.Generic;

public delegate void TreeVisitor<T>(T nodeData);


// General Tree -- > any plan tree can be made using it 
public class GeneralPlan<T>
{
    public T data;
    public LinkedList<GeneralPlan<T>> children;

    public GeneralPlan(T data)
    {
        this.data = data;
        children = new LinkedList<GeneralPlan<T>>();
    }

    public void AddChild(T data)
    {
        children.AddFirst(new GeneralPlan<T>(data));
    }

    public GeneralPlan<T> GetChild(int i)
    {
        foreach (GeneralPlan<T> n in children)
            if (--i == 0)
                return n;
        return null;
    }

    public void Traverse(GeneralPlan<T> node, TreeVisitor<T> visitor)
    {
        visitor(node.data);
        foreach (GeneralPlan<T> kid in node.children)
            Traverse(kid, visitor);
    }

    // removes the 
    public void RemoveChild()
    {
        children.RemoveFirst();
    }

}