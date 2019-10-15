using System;
using System.Collections.Generic;

namespace Bb.Workflows.Parser
{
    internal class MethodReference
    {

        public string Name { get; set; }

        public string Comment { get; set; }

        public Dictionary<string, Type> Arguments { get; } = new Dictionary<string, Type>();

    }

    /*
     
            Func<object, object> func = null;

            if (value.StartsWith("'") && value.EndsWith("'"))
            {
                value = value.Trim('\'');
                func = ExpressionHelper.GetConstant(value);
            }
            else if (value.StartsWith("'@"))
                func = ExpressionHelper.GetAccessors(typeof(RunContext), value.Substring(2));
            
            else
            {

                if (this._constants.TryGetValue(value, out ConstantExpressionModel m))
                    func = ExpressionHelper.GetConstant(m.Value);

                else
                    func = ExpressionHelper.GetConstant(value);

            }

     */

}
