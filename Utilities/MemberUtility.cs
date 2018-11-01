using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Utilities
{
    public static class MemberUtility
    {
        public static MemberInfo GetMemberInfo<TSource, TMember>(Expression<Func<TSource, TMember>> propTransform)
        {
            // -----------------------------------------------------------------------------------------------------------------
            // Using Clarius Reflector (http://www.codeplex.com/Release/ProjectReleases.aspx?ProjectName=clarius&ReleaseId=9495)
            // to determine the property from reflection
            // -----------------------------------------------------------------------------------------------------------------
            LambdaExpression lambda = propTransform;
            if (lambda == null)
            {
                throw new ArgumentException("Not a lambda expression", "propTransform");
            }
            MemberExpression memberExpr = null;

            // The Func<ITrackable, object> we use returns an object, so first statement can be either 
            // a cast (if the field/property does not return an object) or the direct member access.
            switch (lambda.Body.NodeType)
            {
                case ExpressionType.Convert:
                    memberExpr = ((UnaryExpression)lambda.Body).Operand as MemberExpression;
                    break;
                case ExpressionType.MemberAccess:
                    memberExpr = lambda.Body as MemberExpression;
                    break;
            }

            if (memberExpr == null)
            {
                throw new ArgumentException("Not a member access", "member");
            }

            return memberExpr.Member;
        }

        // Gets the PropertyInfo associated with a particular object and transform
        public static PropertyInfo GetPropertyInfo<TSource, TMember>(TSource sourceObject,
            Expression<Func<TSource, TMember>> propTransform)
        {
            PropertyInfo propInfo = GetMemberInfo(propTransform) as PropertyInfo;

            if (propInfo == null)
            {
                throw new ArgumentException("Not a property", "member");
            }

            return propInfo;
        }
    }
}
