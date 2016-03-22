using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace NPFaq.Common.Markup
{
    public class TypeExtension : MarkupExtension
    {
        private string _typeName;
        public string TypeName
        {
            get { return _typeName; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("TypeName");
                _typeName = value;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            IXamlTypeResolver xamlTypeResolver = serviceProvider.GetService(typeof(IXamlTypeResolver)) as IXamlTypeResolver;
            if (xamlTypeResolver == null)
                throw new ArgumentException("xamlTypeResolver");

            Type resolvedType = xamlTypeResolver.Resolve(TypeName);
            return resolvedType;
        }
    }
}
