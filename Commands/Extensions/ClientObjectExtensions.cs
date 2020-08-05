using Microsoft.SharePoint.Client;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace PnP.PowerShell.Commands.Extensions
{
    //TODO: Should this be in core?
    public static class ClientObjectExtensions
    {
        public static void LoadProperties<T>(this T clientObject, params string[] properties) where T : ClientObject
        {
            if (properties == null)
                return;

            Expression<Func<T, object>>[] retrievals = properties.Select(item => GetClientObjectExpression(clientObject, item)).ToArray();

            LoadProperties(clientObject, retrievals);
        }

        public static void LoadProperties<T>(this T clientObject, params Expression<Func<T, object>>[] retrievals) where T : ClientObject
        {
            if (retrievals == null)
                return;

            var loadRequired = false;
            foreach (var expression in retrievals)
            {
                if (!clientObject.IsPropertyAvailable(expression) && !clientObject.IsObjectPropertyInstantiated(expression))
                {
                    clientObject.Context.Load(clientObject, expression);
                    loadRequired = true;
                }
            }

            if (loadRequired)
            {
                clientObject.Context.ExecuteQueryRetry();
            }
        }

        private static Expression<Func<T, object>> GetClientObjectExpression<T>(T clientObject, string property) where T : ClientObject
        {
            var memberExpression = Expression.PropertyOrField(Expression.Constant(clientObject), property);
            var memberName = memberExpression.Member.Name;

            var parameter = Expression.Parameter(typeof(T), "i");
            var cast = Expression.Convert(parameter, memberExpression.Member.ReflectedType);
            var body = Expression.Property(cast, memberName);
            var exp = Expression.Lambda<Func<T, object>>(Expression.Convert(body, typeof(object)), parameter);

            return exp;
        }

        internal static void ClearObjectData(this ClientObject clientObject)
        {
            var info_ClientObject_ObjectData = typeof(ClientObject).GetProperty("ObjectData", BindingFlags.NonPublic | BindingFlags.Instance);
            var objectData = (ClientObjectData)info_ClientObject_ObjectData.GetValue(clientObject, new object[0]);
            objectData.MethodReturnObjects.Clear();
        }
    }
}
