using System.Text.RegularExpressions;

namespace Routing.CustomConstraints
{
    public class AlphaNumericConstraint : IRouteConstraint
    {
        public bool Match(
            HttpContext? httpContext, 
            IRouter? route, 
            string routeKey, 
            RouteValueDictionary values, 
            RouteDirection routeDirection)
        {
            if(!values.ContainsKey(routeKey)) 
            {
                return false;
            }

            Regex regex = new Regex("^[a-zA-Z][a-zA-Z0-9]*$");
            string? userNameValue = Convert.ToString(values[routeKey]);
            if(regex.IsMatch(userNameValue))
            {
                return true;
            }

            return false;
        }
    }
}
