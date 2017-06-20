using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public partial class UIView
{
	protected object FindProperty(string _propertyName)
	{
		if (string.IsNullOrEmpty(_propertyName) == false)
		{
			//var classRoot = UIViewByNode.First(e => e.Value.IsRoot);
            var propertyInfo = ClassRoot.GetType().GetProperty(_propertyName);

            return propertyInfo.GetValue(ClassRoot, null);
		}
		else
			return null;
	}

    protected System.Reflection.MethodInfo FindMethod(string _methodName)
    {
        if (string.IsNullOrEmpty(_methodName) == false)
        {
            var methodInfo = ClassRoot.GetType().GetMethod(_methodName);

            return methodInfo;
        }
        else
            return null;
    }
}
