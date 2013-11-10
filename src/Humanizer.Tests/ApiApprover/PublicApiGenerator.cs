using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CSharp;

// ReSharper disable CheckNamespace
// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
namespace ApiApprover
{
    public class PublicApiGenerator
    {
        public static string CreatePublicApiForAssembly(Assembly assembly)
        {
            var publicApiBuilder = new StringBuilder();
            var cgo = new CodeGeneratorOptions
            {
                BracingStyle = "C",
                BlankLinesBetweenMembers = false
            };

            using (var provider = new CSharpCodeProvider())
            {
                var publicTypes = assembly.GetTypes()
                    .Where(t => t.IsPublic && t.Name != "GeneratedInternalTypeHelper") //GeneratedInternalTypeHelper seems to be a r# runner side effect
                    .OrderBy(t => t.FullName);
                foreach (var publicType in publicTypes)
                {
                    var writer = new StringWriter();
                    var genClass = CreateClassDeclaration(publicType);
                    foreach (var memberInfo in publicType.GetMembers().Where(m => !IsDotNetTypeMember(m)).OrderBy(m => m.Name))
                    {
                        AddMemberToClassDefinition(genClass, memberInfo);
                    }
                    provider.GenerateCodeFromType(genClass, writer, cgo);
                    var gennedClass = GenerateClassCode(writer);
                    publicApiBuilder.AppendLine(gennedClass);
                }
            }
            var publicApi = publicApiBuilder.ToString();
            return publicApi.Trim();
        }

        private static bool IsDotNetTypeMember(MemberInfo m)
        {
            if (m.DeclaringType == null || m.DeclaringType.FullName == null)
                return false;
            return m.DeclaringType.FullName.StartsWith("System") || m.DeclaringType.FullName.StartsWith("Microsoft");
        }

        static void AddMemberToClassDefinition(CodeTypeDeclaration genClass, MemberInfo memberInfo)
        {
            if (memberInfo is MethodInfo)
            {
                var method = (MethodInfo)memberInfo;
                if (method.IsSpecialName) return;
                genClass.Members.Add(GenerateMethod((MethodInfo)memberInfo));
            }
            else if (memberInfo is PropertyInfo)
            {
                genClass.Members.Add(GenerateProperty((PropertyInfo)memberInfo));
            }
            else if (memberInfo is ConstructorInfo)
            {
                genClass.Members.Add(GenerateCtor((ConstructorInfo)memberInfo));
            }
            else if (memberInfo is EventInfo)
            {
                genClass.Members.Add(GenerateEvent((EventInfo)memberInfo));
            }
            else if (memberInfo is FieldInfo)
            {
                genClass.Members.Add(GenerateField((FieldInfo)memberInfo));
            }
        }

        static string GenerateClassCode(StringWriter writer)
        {
            var gennedClass = writer.ToString();
            const string emptyGetSet = @"\s+{\s+get\s+{\s+}\s+set\s+{\s+}\s+}";
            const string emptyGet = @"\s+{\s+get\s+{\s+}\s+}";
            const string getSet = @"\s+{\s+get;\s+set;\s+}";
            const string get = @"\s+{\s+get;\s+}";
            gennedClass = Regex.Replace(gennedClass, emptyGetSet, " { get; set; }", RegexOptions.IgnorePatternWhitespace);
            gennedClass = Regex.Replace(gennedClass, getSet, " { get; set; }", RegexOptions.IgnorePatternWhitespace);
            gennedClass = Regex.Replace(gennedClass, emptyGet, " { get; }", RegexOptions.IgnorePatternWhitespace);
            gennedClass = Regex.Replace(gennedClass, get, " { get; }", RegexOptions.IgnorePatternWhitespace);
            gennedClass = Regex.Replace(gennedClass, @"\s+{\s+}", " { }", RegexOptions.IgnorePatternWhitespace);
            return gennedClass;
        }

        static CodeTypeDeclaration CreateClassDeclaration(Type publicType)
        {
            return new CodeTypeDeclaration(publicType.Name)
            {
                IsClass = publicType.IsClass,
                IsEnum = publicType.IsEnum,
                IsInterface = publicType.IsInterface,
                IsStruct = publicType.IsValueType && !publicType.IsPrimitive && !publicType.IsEnum
            };
        }

        // ReSharper disable BitwiseOperatorOnEnumWihtoutFlags
        public static CodeConstructor GenerateCtor(ConstructorInfo member)
        {
            var method = new CodeConstructor
            {
                Name = member.Name,
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };

            foreach (var parameterInfo in member.GetParameters())
            {
                method.Parameters.Add(new CodeParameterDeclarationExpression(parameterInfo.ParameterType,
                                                                             parameterInfo.Name));
            }
            return method;
        }

        static CodeTypeMember GenerateEvent(EventInfo memberInfo)
        {
            var @event = new CodeMemberEvent
            {
                Name = memberInfo.Name,
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Type = new CodeTypeReference(memberInfo.EventHandlerType)
            };

            return @event;
        }

        static CodeTypeMember GenerateField(FieldInfo memberInfo)
        {
            var field = new CodeMemberField(memberInfo.FieldType, memberInfo.Name)
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };

            return field;
        }

        public static CodeMemberMethod GenerateMethod(MethodInfo member)
        {
            var method = new CodeMemberMethod
            {
                Name = member.Name,
                Attributes = MemberAttributes.Public | MemberAttributes.Final
                // ReSharper restore BitwiseOperatorOnEnumWithoutFlags
            };
            var methodTypeRef = new CodeTypeReference(member.ReturnType);
            method.ReturnType = methodTypeRef;

            var methodParameters = member.GetParameters().ToList();
            var parameterCollection = new CodeParameterDeclarationExpressionCollection();
            foreach (ParameterInfo info in methodParameters)
            {
                var expresion = new CodeParameterDeclarationExpression(info.ParameterType, info.Name);
                parameterCollection.Add(expresion);
            }
            method.Parameters.AddRange(parameterCollection);
            return method;
        }

        public static CodeMemberProperty GenerateProperty(PropertyInfo member)
        {
            var property = new CodeMemberProperty
            {
                Name = member.Name,
                Type = new CodeTypeReference(member.PropertyType),
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                HasGet = member.CanRead,
                HasSet = member.CanWrite
            };

            return property;
        }
    }
}
// ReSharper restore BitwiseOperatorOnEnumWithoutFlags
// ReSharper restore BitwiseOperatorOnEnumWihtoutFlags
// ReSharper restore CheckNamespace
