using System;
using System.ComponentModel;
using System.Net;
using System.Reflection;
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
    /// <summary>
    /// 静态扩展标记
    /// </summary>
    public class StaticExtension : MarkupExtension
    {
        private string _member;
        private Type _memberType;

        public StaticExtension()
        {
        }

        public StaticExtension(string member)
        {
            if (member == null)
            {
                throw new ArgumentNullException("member");
            }
            this._member = member;
        }

        private bool GetFieldOrPropertyValue(Type type, string name, out object value)
        {
            FieldInfo field = null;
            Type baseType = type;
            do
            {
                field = baseType.GetField(name, BindingFlags.Public | BindingFlags.Static);
                if (field != null)
                {
                    value = field.GetValue(null);
                    return true;
                }
                baseType = baseType.BaseType;
            }
            while (baseType != null);
            PropertyInfo property = null;
            baseType = type;
            do
            {
                property = baseType.GetProperty(name, BindingFlags.Public | BindingFlags.Static);
                if (property != null)
                {
                    value = property.GetValue(null, null);
                    return true;
                }
                baseType = baseType.BaseType;
            }
            while (baseType != null);
            value = null;
            return false;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            object obj2;
            if (this._member == null)
            {
                throw new InvalidOperationException("MarkupExtensionStaticMember");
            }
            Type memberType = this.MemberType;
            string str = null;
            string str2 = null;
            if (memberType != null)
            {
                str = this._member;
                str2 = memberType.FullName + "." + this._member;
            }
            else
            {
                str2 = this._member;
                int index = this._member.IndexOf('.');
                if (index < 0)
                {
                    throw new ArgumentException("MarkupExtensionBadStatic", "Member");
                }
                string qualifiedTypeName = this._member.Substring(0, index);
                if (qualifiedTypeName == string.Empty)
                {
                    throw new ArgumentException("MarkupExtensionBadStatic", "Member");
                }
                if (serviceProvider == null)
                {
                    throw new ArgumentNullException("serviceProvider");
                }
                IXamlTypeResolver service = serviceProvider.GetService(typeof(IXamlTypeResolver)) as IXamlTypeResolver;
                if (service == null)
                {
                    throw new ArgumentException("MarkupExtensionNoContext", "Member");
                }
                memberType = service.Resolve(qualifiedTypeName);
                str = this._member.Substring(index + 1, (this._member.Length - index) - 1);
                if (str == string.Empty)
                {
                    throw new ArgumentException("MarkupExtensionBadStatic", "Member");
                }
            }
            if (memberType.IsEnum)
            {
                return Enum.Parse(memberType, str, true);
            }
            if (!this.GetFieldOrPropertyValue(memberType, str, out obj2))
            {
                throw new ArgumentException("MarkupExtensionBadStatic", "Member");
            }
            return obj2;
        }

        public string Member
        {
            get
            {
                return this._member;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this._member = value;
            }
        }

        [DefaultValue((string)null)]
        public Type MemberType
        {
            get
            {
                return this._memberType;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this._memberType = value;
            }
        }
    }
}
