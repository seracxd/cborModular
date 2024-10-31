using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.DataModels
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class DataTypeAttribute : Attribute
    {
        public Type ExpectedType { get; }

        public DataTypeAttribute(Type expectedType)
        {
            ExpectedType = expectedType;
        }
    }
}
