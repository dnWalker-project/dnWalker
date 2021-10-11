using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using Z3.LinqBinding;

namespace dnWalker.Z3
{
    public class Solver : ISolver
    {
        public Dictionary<string, object> Solve(Expression expression, IList<ParameterExpression> parameters)
        {
            using (var ctx = new Z3Context())
            {
                ctx.Log = Console.Out; // see internal logging

                var builder = new ClassBuilder("Parameters");
                var myclass = builder.CreateObject(
                    parameters.Select(p => p.Name).ToArray(),
                    parameters.Select(p => p.Type).ToArray());

                Type TP = myclass.GetType();

                var context = Activator.CreateInstance(TP);

                // co dela tenhle cyklus? nijak neovlivnuje veci mimo
                foreach (var param in parameters)
                {
                    var getDefaultValue =
                        Expression.Lambda<Func<object>>(
                            // it needs to be casted to `object` to create `Func<object>`
                            Expression.Convert(
                                Expression.Default(param.Type),
                                typeof(object)
                            )
                        ).Compile();
                }

                //var method = ctx.GetType().GetMethods().Single(m => m.Name == "NewTheorem" && m.GetParameters().Length == 0);
                //var theorem = ( method.MakeGenericMethod(TP)

                var theorem = new dnWalker.Z3.Theorem(TP, ctx, new List<LambdaExpression> { Expression.Lambda(expression, parameters) });
                // TODO
                /*var theorem = ctx
                    .NewTheorem(context)
                    .Where(Expression.Lambda(expression, parameters));*/
                
                var result = theorem.Solve();
                if (result == null)
                {
                    return null;
                }

                return result.GetType().GetProperties().ToDictionary(x => x.Name, x => x.GetValue(result, null));
            }
        }
    }

    internal class ExpandoTypeDescriptionProvider : TypeDescriptionProvider
    {
        private ICollection<KeyValuePair<string, object>> eoColl;

        public ExpandoTypeDescriptionProvider(ICollection<KeyValuePair<string, object>> eoColl)
        {
            this.eoColl = eoColl;
        }

        public override ICustomTypeDescriptor GetExtendedTypeDescriptor(object instance)
        {
            return new ExpandoObjectTypeDescriptor(instance);
        }
    }

    public class ExpandoObjectTypeDescriptor : ICustomTypeDescriptor
    {
        private readonly IDictionary<string, object> m_Instance;

        public ExpandoObjectTypeDescriptor(dynamic instance)
        {
            m_Instance = instance as IDictionary<string, object>;
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return m_Instance;
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return null;
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return ((ICustomTypeDescriptor)this).GetProperties(new Attribute[0]);
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return new PropertyDescriptorCollection(
                m_Instance.Keys
                          .Select(x => new ExpandoObjectPropertyDescriptor(m_Instance, x))
                          .ToArray<PropertyDescriptor>());
        }

        class ExpandoObjectPropertyDescriptor : PropertyDescriptor
        {
            private readonly IDictionary<string, object> m_Instance;
            private readonly string m_Name;

            public ExpandoObjectPropertyDescriptor(IDictionary<string, object> instance, string name)
                : base(name, null)
            {
                m_Instance = instance;
                m_Name = name;
            }

            public override Type PropertyType
            {
                get { return m_Instance[m_Name].GetType(); }
            }

            public override void SetValue(object component, object value)
            {
                m_Instance[m_Name] = value;
            }

            public override object GetValue(object component)
            {
                return m_Instance[m_Name];
            }

            public override bool IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            public override Type ComponentType
            {
                get { return null; }
            }

            public override bool CanResetValue(object component)
            {
                return false;
            }

            public override void ResetValue(object component)
            {
            }

            public override bool ShouldSerializeValue(object component)
            {
                return false;
            }

            public override string Category
            {
                get { return string.Empty; }
            }

            public override string Description
            {
                get { return string.Empty; }
            }
        }
    }
}
