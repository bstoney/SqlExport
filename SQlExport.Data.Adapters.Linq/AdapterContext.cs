using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SqlExport.Data.Adapters.Linq
{
    public class AdapterContext : ICustomTypeDescriptor
    {

        //private Type BuildTableType()
        //{
        //    EnsureDynamicAssembly();
        //}

        //private void EnsureDynamicAssembly()
        //{
        //    // get the current appdomain
        //    AppDomain ad = AppDomain.CurrentDomain;

        //    // create a new dynamic assembly
        //    AssemblyName an = new AssemblyName();
        //    an.Name = "DynamicRandomAssembly";
        //    AssemblyBuilder ab = ad.DefineDynamicAssembly(
        //     an, AssemblyBuilderAccess.Run );

        //    // create a new module to hold code in the assembly
        //    ModuleBuilder mb = ab.DefineDynamicModule( "RandomModule" );

        //    // create a type in the module
        //    TypeBuilder tb = mb.DefineType(
        //     "DynamicRandomClass", TypeAttributes.Public );

        //    // create a method of the type
        //    Type returntype = typeof( int );
        //    Type[] paramstype = new Type[0];
        //    MethodBuilder methb = tb.DefineMethod( "DynamicRandomMethod",
        //     MethodAttributes.Public, returntype, paramstype );

        //    // generate the MSIL
        //    ILGenerator gen = methb.GetILGenerator();
        //    gen.Emit( OpCodes.Ldc_I4, 1 );
        //    gen.Emit( OpCodes.Ret );

        //    // finish creating the type and make it available
        //    Type t = tb.CreateType();

        //    // create an instance of the new type
        //    Object o = Activator.CreateInstance( t );
        //    // create an empty arguments array
        //    Object[] aa = new Object[0];
        //    // get the method and invoke it
        //    MethodInfo m = t.GetMethod( "DynamicRandomMethod" );
        //    int i = (int)m.Invoke( o, aa );
        //    Console.WriteLine( "Method {0} in Class {1} returned {2}",
        //        m, t, i );
        //}

        #region ICustomTypeDescriptor Members

        public AttributeCollection GetAttributes()
        {
            return new AttributeCollection();
        }

        public string GetClassName()
        {
            return "AdapterContext";
        }

        public string GetComponentName()
        {
            return null;
        }

        public TypeConverter GetConverter()
        {
            return new TypeConverter();
        }

        public EventDescriptor GetDefaultEvent()
        {
            throw new NotImplementedException();
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            throw new NotImplementedException();
        }

        public object GetEditor( Type editorBaseType )
        {
            throw new NotImplementedException();
        }

        public EventDescriptorCollection GetEvents( Attribute[] attributes )
        {
            throw new NotImplementedException();
        }

        public EventDescriptorCollection GetEvents()
        {
            throw new NotImplementedException();
        }

        public PropertyDescriptorCollection GetProperties( Attribute[] attributes )
        {
            return new PropertyDescriptorCollection( new PropertyDescriptor[] { } );
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return new PropertyDescriptorCollection( new PropertyDescriptor[] {
                new CustomPropertyDescriptor( "Table", typeof( string ) ) 
            } );
        }

        public object GetPropertyOwner( PropertyDescriptor pd )
        {
            throw new NotImplementedException();
        }

        #endregion

        private class CustomPropertyDescriptor : PropertyDescriptor
        {
            private string _name;
            private Type _type;

            public CustomPropertyDescriptor( string name, Type type )
                : base( name, null )
            {
                _name = name;
                _type = type;
            }

            public override bool CanResetValue( object component )
            {
                throw new NotImplementedException();
            }

            public override Type ComponentType
            {
                get { throw new NotImplementedException(); }
            }

            public override object GetValue( object component )
            {
                return "Value";
            }

            public override bool IsReadOnly
            {
                get { throw new NotImplementedException(); }
            }

            public override Type PropertyType
            {
                get { return _type; }
            }

            public override void ResetValue( object component )
            {
                throw new NotImplementedException();
            }

            public override void SetValue( object component, object value )
            {
                throw new NotImplementedException();
            }

            public override bool ShouldSerializeValue( object component )
            {
                throw new NotImplementedException();
            }
        }
    }
}
