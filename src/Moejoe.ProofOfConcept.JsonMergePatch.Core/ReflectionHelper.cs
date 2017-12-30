using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Moejoe.ProofOfConcept.JsonMergePatch.Core
{
    public static class ReflectionHelper
    {
        public static MemberInfo FindProperty(LambdaExpression lambdaExpression)
        {
            Expression expressionToCheck = lambdaExpression;

            var done = false;

            while (!done)
            {
                switch (expressionToCheck.NodeType)
                {
                    case ExpressionType.Convert:
                        expressionToCheck = ((UnaryExpression)expressionToCheck).Operand;
                        break;
                    case ExpressionType.Lambda:
                        expressionToCheck = ((LambdaExpression)expressionToCheck).Body;
                        break;
                    case ExpressionType.MemberAccess:
                        var memberExpression = ((MemberExpression)expressionToCheck);

                        if (memberExpression.Expression.NodeType != ExpressionType.Parameter &&
                            memberExpression.Expression.NodeType != ExpressionType.Convert)
                        {
                            throw new ArgumentException(
                                $"Expression '{lambdaExpression}' must resolve to top-level member and not any child object's properties.",
                                nameof(lambdaExpression));
                        }

                        var member = memberExpression.Member;

                        return member;
                    default:
                        done = true;
                        break;
                }
            }
            throw new JsonMergePatchDocumentException("IsSet is only supported for top-level individual members on a type.");
        }

        public static Type GetMemberType(MemberInfo memberInfo)
        {
            switch (memberInfo)
            {
                case MethodInfo mInfo:
                    return mInfo.ReturnType;
                case PropertyInfo pInfo:
                    return pInfo.PropertyType;
                case FieldInfo fInfo:
                    return fInfo.FieldType;
                case null:
                    throw new ArgumentNullException(nameof(memberInfo));
                default:
                    throw new ArgumentOutOfRangeException(nameof(memberInfo));
            }
        }

        public static void SetMemberValue(MemberInfo propertyOrField, object target, object value)
        {
            if (propertyOrField is PropertyInfo property)
            {
                property.SetValue(target, value, null);
                return;
            }
            if (propertyOrField is FieldInfo field)
            {
                field.SetValue(target, value);
                return;
            }
            throw Expected(propertyOrField);
        }

        public static object GetMemberValue(MemberInfo propertyOrField, object target)
        {
            if (propertyOrField is PropertyInfo property)
            {
                return property.GetValue(target, null);
            }
            if (propertyOrField is FieldInfo field)
            {
                return field.GetValue(target);
            }
            throw Expected(propertyOrField);
        }

        public static object GetNullValue(MemberInfo propertyOrField)
        {
            Type type;
            if (propertyOrField is PropertyInfo property)
            {
                type =  property.PropertyType;
            }
            else if (propertyOrField is FieldInfo field)
            {
                type = field.FieldType;
            }
            else
            {
                throw Expected(propertyOrField);
            }
            return type.GetTypeInfo().IsValueType ? Activator.CreateInstance(type) : null;
        }

        public static bool CanBeSet(MemberInfo propertyOrField)
        {
            return propertyOrField is FieldInfo field ?
                !field.IsInitOnly :
                ((PropertyInfo)propertyOrField).CanWrite;
        }

        public static LambdaExpression ToMemberExpression<TType>(MemberInfo p)
        {
            var parameter = Expression.Parameter(typeof(TType), "p");
            var prop = Expression.Property(parameter, p.Name);
            var delegateType = typeof(Func<,>).MakeGenericType(typeof(TType),
                ReflectionHelper.GetMemberType(p));
            return Expression.Lambda(delegateType, prop, parameter);
        }

        private static ArgumentOutOfRangeException Expected(MemberInfo propertyOrField)
            => new ArgumentOutOfRangeException(nameof(propertyOrField), "Expected a property or field, not " + propertyOrField);
    }
}