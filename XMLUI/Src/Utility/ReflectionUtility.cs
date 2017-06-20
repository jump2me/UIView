using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ReflectionUtility 
{
    public static object FindProperty(UIView _view, string _propertyName)
	{
		if (string.IsNullOrEmpty(_propertyName) == false)
		{
			var propertyInfo = _view.ClassRoot.GetType().GetProperty(_propertyName);

			return propertyInfo.GetValue(_view.ClassRoot, null);
		}
		else
			return null;
	}

	public static System.Reflection.MethodInfo FindMethod(UIView _view, string _methodName)
	{
		if (string.IsNullOrEmpty(_methodName) == false)
		{
			var methodInfo = _view.ClassRoot.GetType().GetMethod(_methodName);

			return methodInfo;
		}
		else
			return null;
	}
}
