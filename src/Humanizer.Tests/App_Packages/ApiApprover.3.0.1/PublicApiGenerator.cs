using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CSharp;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using TypeAttributes = System.Reflection.TypeAttributes;

// ReSharper disable CheckNamespace
// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
namespace ApiApprover
{
    public static class CecilEx
    {
        public static IEnumerable<IMemberDefinition> GetMembers(this TypeDefinition type)
        {
            return type.Fields.Cast<IMemberDefinition>()
                .Concat(type.Methods)
                .Concat(type.Properties)
                .Concat(type.Events);
        }
    }


    public static class PublicApiGenerator
    {
        // TODO: Assembly references?
        // TODO: Better handle namespaces - using statements? - requires non-qualified type names
        public static string CreatePublicApiForAssembly(AssemblyDefinition assembly)
        {
            return CreatePublicApiForAssembly(assembly, t => true, true);
        }

        public static string CreatePublicApiForAssembly(AssemblyDefinition assembly, Func<TypeDefinition, bool> shouldIncludeType, bool shouldIncludeAssemblyAttributes)
        {
            var publicApiBuilder = new StringBuilder();
            var cgo = new CodeGeneratorOptions
            {
                BracingStyle = "C",
                BlankLinesBetweenMembers = false,
                VerbatimOrder = false
            };

            using (var provider = new CSharpCodeProvider())
            {
                var compileUnit = new CodeCompileUnit();
                if (shouldIncludeAssemblyAttributes && assembly.HasCustomAttributes)
                {
                    PopulateCustomAttributes(assembly, compileUnit.AssemblyCustomAttributes);
                }

                var publicTypes = assembly.Modules.SelectMany(m => m.GetTypes())
                    .Where(t => !t.IsNested && ShouldIncludeType(t) && shouldIncludeType(t))
                    .OrderBy(t => t.FullName);
                foreach (var publicType in publicTypes)
                {
                    var @namespace = compileUnit.Namespaces.Cast<CodeNamespace>()
                        .FirstOrDefault(n => n.Name == publicType.Namespace);
                    if (@namespace == null)
                    {
                        @namespace = new CodeNamespace(publicType.Namespace);
                        compileUnit.Namespaces.Add(@namespace);
                    }

                    var typeDeclaration = CreateTypeDeclaration(publicType);
                    @namespace.Types.Add(typeDeclaration);
                }

                using (var writer = new StringWriter())
                {
                    provider.GenerateCodeFromCompileUnit(compileUnit, writer, cgo);
                    var typeDeclarationText = NormaliseGeneratedCode(writer);
                    publicApiBuilder.AppendLine(typeDeclarationText);
                }
            }
            return NormaliseLineEndings(publicApiBuilder.ToString().Trim());
        }

        private static string NormaliseLineEndings(string value)
        {
            return Regex.Replace(value, @"\r\n|\n\r|\r|\n", Environment.NewLine);
        }

        private static bool IsDelegate(TypeDefinition publicType)
        {
            return publicType.BaseType != null && publicType.BaseType.FullName == "System.MulticastDelegate";
        }

        private static bool ShouldIncludeType(TypeDefinition t)
        {
            return (t.IsPublic || t.IsNestedPublic || t.IsNestedFamily) && !IsCompilerGenerated(t);
        }

        private static bool ShouldIncludeMember(IMemberDefinition m)
        {
            return !IsCompilerGenerated(m) && !IsDotNetTypeMember(m) && !(m is FieldDefinition);
        }

        private static bool IsCompilerGenerated(IMemberDefinition m)
        {
            return m.CustomAttributes.Any(a => a.AttributeType.FullName == "System.Runtime.CompilerServices.CompilerGeneratedAttribute");
        }

        private static bool IsDotNetTypeMember(IMemberDefinition m)
        {
            if (m.DeclaringType == null || m.DeclaringType.FullName == null)
                return false;
            return m.DeclaringType.FullName.StartsWith("System") || m.DeclaringType.FullName.StartsWith("Microsoft");
        }

        static void AddMemberToTypeDeclaration(CodeTypeDeclaration typeDeclaration, IMemberDefinition memberInfo)
        {
            var methodDefinition = memberInfo as MethodDefinition;
            if (methodDefinition != null)
            {
                if (methodDefinition.IsConstructor)
                    AddCtorToTypeDeclaration(typeDeclaration, methodDefinition);
                else
                    AddMethodToTypeDeclaration(typeDeclaration, methodDefinition);
            }
            else if (memberInfo is PropertyDefinition)
            {
                AddPropertyToTypeDeclaration(typeDeclaration, (PropertyDefinition) memberInfo);
            }
            else if (memberInfo is EventDefinition)
            {
                typeDeclaration.Members.Add(GenerateEvent((EventDefinition)memberInfo));
            }
            else if (memberInfo is FieldDefinition)
            {
                AddFieldToTypeDeclaration(typeDeclaration, (FieldDefinition) memberInfo);
            }
        }

        static string NormaliseGeneratedCode(StringWriter writer)
        {
            var gennedClass = writer.ToString();
            const string autoGeneratedHeader = @"^//-+\s*$.*^//-+\s*$";
            const string emptyGetSet = @"\s+{\s+get\s+{\s+}\s+set\s+{\s+}\s+}";
            const string emptyGet = @"\s+{\s+get\s+{\s+}\s+}";
            const string emptySet = @"\s+{\s+set\s+{\s+}\s+}";
            const string getSet = @"\s+{\s+get;\s+set;\s+}";
            const string get = @"\s+{\s+get;\s+}";
            const string set = @"\s+{\s+set;\s+}";
            gennedClass = Regex.Replace(gennedClass, autoGeneratedHeader, string.Empty,
                RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.Singleline);
            gennedClass = Regex.Replace(gennedClass, emptyGetSet, " { get; set; }", RegexOptions.IgnorePatternWhitespace);
            gennedClass = Regex.Replace(gennedClass, getSet, " { get; set; }", RegexOptions.IgnorePatternWhitespace);
            gennedClass = Regex.Replace(gennedClass, emptyGet, " { get; }", RegexOptions.IgnorePatternWhitespace);
            gennedClass = Regex.Replace(gennedClass, emptySet, " { set; }", RegexOptions.IgnorePatternWhitespace);
            gennedClass = Regex.Replace(gennedClass, get, " { get; }", RegexOptions.IgnorePatternWhitespace);
            gennedClass = Regex.Replace(gennedClass, set, " { set; }", RegexOptions.IgnorePatternWhitespace);
            gennedClass = Regex.Replace(gennedClass, @"\s+{\s+}", " { }", RegexOptions.IgnorePatternWhitespace);
            gennedClass = Regex.Replace(gennedClass, @"\)\s+;", ");", RegexOptions.IgnorePatternWhitespace);
            return gennedClass;
        }

        static CodeTypeDeclaration CreateTypeDeclaration(TypeDefinition publicType)
        {
            if (IsDelegate(publicType))
                return CreateDelegateDeclaration(publicType);

            bool @static = false;
            TypeAttributes attributes = 0;
            if (publicType.IsPublic || publicType.IsNestedPublic)
                attributes |= TypeAttributes.Public;
            if (publicType.IsNestedFamily)
                attributes |= TypeAttributes.NestedFamily;
            if (publicType.IsSealed && !publicType.IsAbstract)
                attributes |= TypeAttributes.Sealed;
            else if (!publicType.IsSealed && publicType.IsAbstract && !publicType.IsInterface)
                attributes |= TypeAttributes.Abstract;
            else if (publicType.IsSealed && publicType.IsAbstract)
                @static = true;

            // Static support is a hack. CodeDOM does support it, and this isn't
            // correct C#, but it's good enough for our API outline
            var name = publicType.Name;

            var index = name.IndexOf('`');
            if (index != -1)
                name = name.Substring(0, index);
            var declaration = new CodeTypeDeclaration(@static ? "static " + name : name)
            {
                CustomAttributes = CreateCustomAttributes(publicType),
                // TypeAttributes must be specified before the IsXXX as they manipulate TypeAttributes!
                TypeAttributes = attributes,
                IsClass = publicType.IsClass,
                IsEnum = publicType.IsEnum,
                IsInterface = publicType.IsInterface,
                IsStruct = publicType.IsValueType && !publicType.IsPrimitive && !publicType.IsEnum,
            };

            if (declaration.IsInterface && publicType.BaseType != null)
                throw new NotImplementedException("Base types for interfaces needs testing");

            PopulateGenericParameters(publicType, declaration.TypeParameters);

            if (publicType.BaseType != null && ShouldOutputBaseType(publicType))
            {
                if (publicType.BaseType.FullName == "System.Enum")
                {
                    var underlyingType = publicType.GetEnumUnderlyingType();
                    if (underlyingType.FullName != "System.Int32")
                        declaration.BaseTypes.Add(CreateCodeTypeReference(underlyingType));
                }
                else
                    declaration.BaseTypes.Add(CreateCodeTypeReference(publicType.BaseType));
            }
            foreach (var @interface in publicType.Interfaces.OrderBy(i => i.FullName))
                declaration.BaseTypes.Add(CreateCodeTypeReference(@interface));

            foreach (var memberInfo in publicType.GetMembers().Where(ShouldIncludeMember).OrderBy(m => m.Name))
                AddMemberToTypeDeclaration(declaration, memberInfo);

            // Fields should be in defined order for an enum
            var fields = !publicType.IsEnum
                ? publicType.Fields.OrderBy(f => f.Name)
                : (IEnumerable<FieldDefinition>)publicType.Fields;
            foreach (var field in fields)
                AddMemberToTypeDeclaration(declaration, field);

            foreach (var nestedType in publicType.NestedTypes.Where(ShouldIncludeType).OrderBy(t => t.FullName))
            {
                var nestedTypeDeclaration = CreateTypeDeclaration(nestedType);
                declaration.Members.Add(nestedTypeDeclaration);
            }

            return declaration;
        }

        private static CodeTypeDeclaration CreateDelegateDeclaration(TypeDefinition publicType)
        {
            var invokeMethod = publicType.Methods.Single(m => m.Name == "Invoke");
            var name = publicType.Name;
            var index = name.IndexOf('`');
            if (index != -1)
                name = name.Substring(0, index);
            var declaration = new CodeTypeDelegate(name)
            {
                Attributes = MemberAttributes.Public,
                CustomAttributes = CreateCustomAttributes(publicType),
                ReturnType = CreateCodeTypeReference(invokeMethod.ReturnType),
            };

            // CodeDOM. No support. Return type attributes.
            PopulateCustomAttributes(invokeMethod.MethodReturnType, declaration.CustomAttributes, type => ModifyCodeTypeReference(type, "return:"));
            PopulateGenericParameters(publicType, declaration.TypeParameters);
            PopulateMethodParameters(invokeMethod, declaration.Parameters);

            // Of course, CodeDOM doesn't support generic type parameters for delegates. Of course.
            if (declaration.TypeParameters.Count > 0)
            {
                var parameterNames = from parameterType in declaration.TypeParameters.Cast<CodeTypeParameter>()
                    select parameterType.Name;
                declaration.Name = string.Format("{0}<{1}>", declaration.Name, string.Join(", ", parameterNames));
            }

            return declaration;
        }

        private static bool ShouldOutputBaseType(TypeDefinition publicType)
        {
            return publicType.BaseType.FullName != "System.Object" && publicType.BaseType.FullName != "System.ValueType";
        }

        private static void PopulateGenericParameters(IGenericParameterProvider publicType, CodeTypeParameterCollection parameters)
        {
            foreach (var parameter in publicType.GenericParameters)
            {
                if (parameter.HasCustomAttributes)
                    throw new NotImplementedException("Attributes on type parameters is not supported. And weird");

                // A little hacky. Means we get "in" and "out" prefixed on any constraints, but it's either that
                // or add it as a custom attribute, which looks even weirder
                var name = parameter.Name;
                if (parameter.IsCovariant)
                    name = "out " + name;
                if (parameter.IsContravariant)
                    name = "in " + name;

                var typeParameter = new CodeTypeParameter(name)
                {
                    HasConstructorConstraint =
                        parameter.HasDefaultConstructorConstraint && !parameter.HasNotNullableValueTypeConstraint
                };
                if (parameter.HasNotNullableValueTypeConstraint)
                    typeParameter.Constraints.Add(" struct"); // Extra space is a hack!
                if (parameter.HasReferenceTypeConstraint)
                    typeParameter.Constraints.Add(" class");
                foreach (var constraint in parameter.Constraints.Where(t => t.FullName != "System.ValueType"))
                {
                    typeParameter.Constraints.Add(CreateCodeTypeReference(constraint.GetElementType()));
                }
                parameters.Add(typeParameter);
            }
        }

        private static CodeAttributeDeclarationCollection CreateCustomAttributes(ICustomAttributeProvider type)
        {
            var attributes = new CodeAttributeDeclarationCollection();
            PopulateCustomAttributes(type, attributes);
            return attributes;
        }

        private static void PopulateCustomAttributes(ICustomAttributeProvider type,
            CodeAttributeDeclarationCollection attributes)
        {
            PopulateCustomAttributes(type, attributes, ctr => ctr);
        }

        private static void PopulateCustomAttributes(ICustomAttributeProvider type,
            CodeAttributeDeclarationCollection attributes, Func<CodeTypeReference, CodeTypeReference> codeTypeModifier)
        {
            foreach (var customAttribute in type.CustomAttributes.Where(ShouldIncludeAttribute).OrderBy(a => a.AttributeType.FullName).ThenBy(a => ConvertAttrbuteToCode(codeTypeModifier, a)))
            {
                var attribute = GenerateCodeAttributeDeclaration(codeTypeModifier, customAttribute);
                attributes.Add(attribute);
            }
        }

        private static CodeAttributeDeclaration GenerateCodeAttributeDeclaration(Func<CodeTypeReference, CodeTypeReference> codeTypeModifier, CustomAttribute customAttribute)
        {
            var attribute = new CodeAttributeDeclaration(codeTypeModifier(CreateCodeTypeReference(customAttribute.AttributeType)));
            foreach (var arg in customAttribute.ConstructorArguments)
            {
                attribute.Arguments.Add(new CodeAttributeArgument(CreateInitialiserExpression(arg)));
            }
            foreach (var field in customAttribute.Fields.OrderBy(f => f.Name))
            {
                attribute.Arguments.Add(new CodeAttributeArgument(field.Name, CreateInitialiserExpression(field.Argument)));
            }
            foreach (var property in customAttribute.Properties.OrderBy(p => p.Name))
            {
                attribute.Arguments.Add(new CodeAttributeArgument(property.Name, CreateInitialiserExpression(property.Argument)));
            }
            return attribute;
        }

        // Litee: This method is used for additional sorting of custom attributes when multiple values are allowed
        private static object ConvertAttrbuteToCode(Func<CodeTypeReference, CodeTypeReference> codeTypeModifier, CustomAttribute customAttribute)
        {
            using (var provider = new CSharpCodeProvider())
            {
                var cgo = new CodeGeneratorOptions
                {
                    BracingStyle = "C",
                    BlankLinesBetweenMembers = false,
                    VerbatimOrder = false
                };
                var attribute = GenerateCodeAttributeDeclaration(codeTypeModifier, customAttribute);
                var declaration = new CodeTypeDeclaration("DummyClass")
                {
                    CustomAttributes = new CodeAttributeDeclarationCollection(new[] { attribute }),
                };
                using (var writer = new StringWriter())
                {
                    provider.GenerateCodeFromType(declaration, writer, cgo);
                    return writer.ToString();
                }
            }
        }

        private static readonly HashSet<string> SkipAttributeNames = new HashSet<string>
        {
            "System.CodeDom.Compiler.GeneratedCodeAttribute",
            "System.ComponentModel.EditorBrowsableAttribute",
            "System.Runtime.CompilerServices.AsyncStateMachineAttribute",
            "System.Runtime.CompilerServices.CompilerGeneratedAttribute",
            "System.Runtime.CompilerServices.CompilationRelaxationsAttribute",
            "System.Runtime.CompilerServices.ExtensionAttribute",
            "System.Runtime.CompilerServices.RuntimeCompatibilityAttribute",
            "System.Reflection.DefaultMemberAttribute",
            "System.Diagnostics.DebuggableAttribute",
            "System.Diagnostics.DebuggerNonUserCodeAttribute",
            "System.Diagnostics.DebuggerStepThroughAttribute",
            "System.Reflection.AssemblyCompanyAttribute",
            "System.Reflection.AssemblyConfigurationAttribute",
            "System.Reflection.AssemblyCopyrightAttribute",
            "System.Reflection.AssemblyDescriptionAttribute",
            "System.Reflection.AssemblyFileVersionAttribute",
            "System.Reflection.AssemblyInformationalVersionAttribute",
            "System.Reflection.AssemblyProductAttribute",
            "System.Reflection.AssemblyTitleAttribute",
            "System.Reflection.AssemblyTrademarkAttribute"
        };

        private static bool ShouldIncludeAttribute(CustomAttribute attribute)
        {
            return !SkipAttributeNames.Contains(attribute.AttributeType.FullName);
        }

        private static CodeExpression CreateInitialiserExpression(CustomAttributeArgument attributeArgument)
        {
            if (attributeArgument.Value is CustomAttributeArgument)
            {
                return CreateInitialiserExpression((CustomAttributeArgument) attributeArgument.Value);
            }

            if (attributeArgument.Value is CustomAttributeArgument[])
            {
                var initialisers = from argument in (CustomAttributeArgument[]) attributeArgument.Value
                    select CreateInitialiserExpression(argument);
                return new CodeArrayCreateExpression(CreateCodeTypeReference(attributeArgument.Type), initialisers.ToArray());
            }

            var type = attributeArgument.Type.Resolve();
            var value = attributeArgument.Value;
            if (type.BaseType != null && type.BaseType.FullName == "System.Enum")
            {
                var originalValue = Convert.ToInt64(value);
                if (type.CustomAttributes.Any(a => a.AttributeType.FullName == "System.FlagsAttribute"))
                {
                    //var allFlags = from f in type.Fields
                    //    where f.Constant != null
                    //    let v = Convert.ToInt64(f.Constant)
                    //    where v == 0 || (originalValue & v) != 0
                    //    select (CodeExpression)new CodeFieldReferenceExpression(typeExpression, f.Name);
                    //return allFlags.Aggregate((current, next) => new CodeBinaryOperatorExpression(current, CodeBinaryOperatorType.BitwiseOr, next));

                    // I'd rather use the above, as it's just using the CodeDOM, but it puts
                    // brackets around each CodeBinaryOperatorExpression
                    var flags = from f in type.Fields
                                where f.Constant != null
                                let v = Convert.ToInt64(f.Constant)
                                where v == 0 || (originalValue & v) != 0
                                select type.FullName + "." + f.Name;
                    return new CodeSnippetExpression(flags.Aggregate((current, next) => current + " | " + next));
                }

                var allFlags = from f in type.Fields
                               where f.Constant != null
                               let v = Convert.ToInt64(f.Constant)
                               where v == originalValue
                               select new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(CreateCodeTypeReference(type)), f.Name);
                return allFlags.FirstOrDefault();
            }

            if (type.FullName == "System.Type" && value is TypeReference)
            {
                return new CodeTypeOfExpression(CreateCodeTypeReference((TypeReference)value));
            }

            if (value is string)
            {
                // CodeDOM outputs a verbatim string. Any string with \n is treated as such, so normalise
                // it to make it easier for comparisons
                value = Regex.Replace((string)value, @"\n", "\\n");
                value = Regex.Replace((string)value, @"\r\n|\r\\n", "\\r\\n");
            }

            return new CodePrimitiveExpression(value);
        }

        private static void AddCtorToTypeDeclaration(CodeTypeDeclaration typeDeclaration, MethodDefinition member)
        {
            if (member.IsAssembly || member.IsPrivate)
                return;

            var method = new CodeConstructor
            {
                CustomAttributes = CreateCustomAttributes(member),
                Name = member.Name,
                Attributes = GetMethodAttributes(member)
            };
            PopulateMethodParameters(member, method.Parameters);

            typeDeclaration.Members.Add(method);
        }

        private static void AddMethodToTypeDeclaration(CodeTypeDeclaration typeDeclaration, MethodDefinition member)
        {
            if (member.IsAssembly || member.IsPrivate || member.IsSpecialName)
                return;

            var returnType = CreateCodeTypeReference(member.ReturnType);
            if (IsAsyncMethod(member))
                returnType = MakeAsync(returnType);

            var method = new CodeMemberMethod
            {
                Name = member.Name,
                Attributes = GetMethodAttributes(member),
                CustomAttributes = CreateCustomAttributes(member),
                ReturnType = returnType,
            };
            PopulateCustomAttributes(member.MethodReturnType, method.ReturnTypeCustomAttributes);
            PopulateGenericParameters(member, method.TypeParameters);
            PopulateMethodParameters(member, method.Parameters, IsExtensionMethod(member));

            typeDeclaration.Members.Add(method);
        }

        private static bool IsAsyncMethod(ICustomAttributeProvider method)
        {
            return method.CustomAttributes.Any(a => a.AttributeType.FullName == "System.Runtime.CompilerServices.AsyncStateMachineAttribute");
        }

        private static bool IsExtensionMethod(ICustomAttributeProvider method)
        {
            return method.CustomAttributes.Any(a => a.AttributeType.FullName == "System.Runtime.CompilerServices.ExtensionAttribute");
        }

        private static void PopulateMethodParameters(IMethodSignature member,
            CodeParameterDeclarationExpressionCollection parameters, bool isExtension = false)
        {
            foreach (var parameter in member.Parameters)
            {
                FieldDirection direction = 0;
                if (parameter.IsOut)
                    direction |= FieldDirection.Out;
                else if (parameter.ParameterType.IsByReference)
                    direction |= FieldDirection.Ref;

                var parameterType = parameter.ParameterType.IsByReference
                    ? parameter.ParameterType.GetElementType()
                    : parameter.ParameterType;

                var type = CreateCodeTypeReference(parameterType);

                if (isExtension)
                {
                    type = ModifyCodeTypeReference(type, "this");
                    isExtension = false;
                }

                var name = parameter.HasConstant
                    ? string.Format("{0} = {1}", parameter.Name, FormatParameterConstant(parameter))
                    : parameter.Name;
                var expression = new CodeParameterDeclarationExpression(type, name)
                {
                    Direction = direction,
                    CustomAttributes = CreateCustomAttributes(parameter)
                };
                parameters.Add(expression);
            }
        }

        private static object FormatParameterConstant(IConstantProvider parameter)
        {
            return parameter.Constant is string ? string.Format("\"{0}\"", parameter.Constant) : (parameter.Constant ?? "null");
        }

        static MemberAttributes GetMethodAttributes(MethodDefinition method)
        {
            MemberAttributes access = 0;
            if (method.IsFamily)
                access = MemberAttributes.Family;
            if (method.IsPublic)
                access = MemberAttributes.Public;
            if (method.IsAssembly)
                access = MemberAttributes.Assembly;
            if (method.IsFamilyAndAssembly)
                access = MemberAttributes.FamilyAndAssembly;
            if (method.IsFamilyOrAssembly)
                access = MemberAttributes.FamilyOrAssembly;

            MemberAttributes scope = 0;
            if (method.IsStatic)
                scope |= MemberAttributes.Static;
            if (method.IsFinal || !method.IsVirtual)
                scope |= MemberAttributes.Final;
            if (method.IsAbstract)
                scope |= MemberAttributes.Abstract;
            if (method.IsVirtual && !method.IsNewSlot)
                scope |= MemberAttributes.Override;

            MemberAttributes vtable = 0;
            if (IsHidingMethod(method))
                vtable = MemberAttributes.New;

            return access | scope | vtable;
        }

        private static bool IsHidingMethod(MethodDefinition method)
        {
            var typeDefinition = method.DeclaringType;

            // If we're an interface, just try and find any method with the same signature
            // in any of the interfaces that we implement
            if (typeDefinition.IsInterface)
            {
                var interfaceMethods = from @interfaceReference in typeDefinition.Interfaces
                    let interfaceDefinition = @interfaceReference.Resolve()
                    where interfaceDefinition != null
                    select interfaceDefinition.Methods;

                return interfaceMethods.Any(ms => MetadataResolver.GetMethod(ms, method) != null);
            }

            // If we're not an interface, find a base method that isn't virtual
            return !method.IsVirtual && GetBaseTypes(typeDefinition).Any(d => MetadataResolver.GetMethod(d.Methods, method) != null);
        }

        private static IEnumerable<TypeDefinition> GetBaseTypes(TypeDefinition type)
        {
            var baseType = type.BaseType;
            while (baseType != null)
            {
                var definition = baseType.Resolve();
                if (definition == null)
                    yield break;
                yield return definition;

                baseType = baseType.DeclaringType;
            }
        }

        private static void AddPropertyToTypeDeclaration(CodeTypeDeclaration typeDeclaration, PropertyDefinition member)
        {
            var getterAttributes = member.GetMethod != null ? GetMethodAttributes(member.GetMethod) : 0;
            var setterAttributes = member.SetMethod != null ? GetMethodAttributes(member.SetMethod) : 0;

            if (!HasVisiblePropertyMethod(getterAttributes) && !HasVisiblePropertyMethod(setterAttributes))
                return;

            var propertyAttributes = GetPropertyAttributes(getterAttributes, setterAttributes);

            var propertyType = member.PropertyType.IsGenericParameter
                ? new CodeTypeReference(member.PropertyType.Name)
                : CreateCodeTypeReference(member.PropertyType);

            var property = new CodeMemberProperty
            {
                Name = member.Name,
                Type = propertyType,
                Attributes = propertyAttributes,
                CustomAttributes = CreateCustomAttributes(member),
                HasGet = member.GetMethod != null && HasVisiblePropertyMethod(getterAttributes),
                HasSet = member.SetMethod != null && HasVisiblePropertyMethod(setterAttributes)
            };

            // Here's a nice hack, because hey, guess what, the CodeDOM doesn't support
            // attributes on getters or setters
            if (member.GetMethod != null && member.GetMethod.HasCustomAttributes)
            {
                PopulateCustomAttributes(member.GetMethod, property.CustomAttributes, type => ModifyCodeTypeReference(type, "get:"));
            }
            if (member.SetMethod != null && member.SetMethod.HasCustomAttributes)
            {
                PopulateCustomAttributes(member.GetMethod, property.CustomAttributes, type => ModifyCodeTypeReference(type, "set:"));
            }

            foreach (var parameter in member.Parameters)
            {
                property.Parameters.Add(
                    new CodeParameterDeclarationExpression(CreateCodeTypeReference(parameter.ParameterType),
                        parameter.Name));
            }

            // TODO: CodeDOM has no support for different access modifiers for getters and setters
            // TODO: CodeDOM has no support for attributes on setters or getters - promote to property?

            typeDeclaration.Members.Add(property);
        }

        private static MemberAttributes GetPropertyAttributes(MemberAttributes getterAttributes, MemberAttributes setterAttributes)
        {
            MemberAttributes access = 0;
            var getterAccess = getterAttributes & MemberAttributes.AccessMask;
            var setterAccess = setterAttributes & MemberAttributes.AccessMask;
            if (getterAccess == MemberAttributes.Public || setterAccess == MemberAttributes.Public)
                access = MemberAttributes.Public;
            else if (getterAccess == MemberAttributes.Family || setterAccess == MemberAttributes.Family)
                access = MemberAttributes.Family;
            else if (getterAccess == MemberAttributes.FamilyAndAssembly || setterAccess == MemberAttributes.FamilyAndAssembly)
                access = MemberAttributes.FamilyAndAssembly;
            else if (getterAccess == MemberAttributes.FamilyOrAssembly || setterAccess == MemberAttributes.FamilyOrAssembly)
                access = MemberAttributes.FamilyOrAssembly;
            else if (getterAccess == MemberAttributes.Assembly || setterAccess == MemberAttributes.Assembly)
                access = MemberAttributes.Assembly;
            else if (getterAccess == MemberAttributes.Private || setterAccess == MemberAttributes.Private)
                access = MemberAttributes.Private;

            // Scope should be the same for getter and setter. If one isn't specified, it'll be 0
            var getterScope = getterAttributes & MemberAttributes.ScopeMask;
            var setterScope = setterAttributes & MemberAttributes.ScopeMask;
            var scope = (MemberAttributes) Math.Max((int) getterScope, (int) setterScope);

            // Vtable should be the same for getter and setter. If one isn't specified, it'll be 0
            var getterVtable = getterAttributes & MemberAttributes.VTableMask;
            var setterVtable = setterAttributes & MemberAttributes.VTableMask;
            var vtable = (MemberAttributes) Math.Max((int) getterVtable, (int) setterVtable);

            return access | scope | vtable;
        }

        private static bool HasVisiblePropertyMethod(MemberAttributes attributes)
        {
            var access = attributes & MemberAttributes.AccessMask;
            return access == MemberAttributes.Public || access == MemberAttributes.Family ||
                   access == MemberAttributes.FamilyOrAssembly;
        }

        static CodeTypeMember GenerateEvent(EventDefinition eventDefinition)
        {
            var @event = new CodeMemberEvent
            {
                Name = eventDefinition.Name,
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                CustomAttributes = CreateCustomAttributes(eventDefinition),
                Type = CreateCodeTypeReference(eventDefinition.EventType)
            };

            return @event;
        }

        static void AddFieldToTypeDeclaration(CodeTypeDeclaration typeDeclaration, FieldDefinition memberInfo)
        {
            if (memberInfo.IsPrivate || memberInfo.IsAssembly || memberInfo.IsSpecialName)
                return;

            MemberAttributes attributes = 0;
            if (memberInfo.HasConstant)
                attributes |= MemberAttributes.Const;
            if (memberInfo.IsFamily)
                attributes |= MemberAttributes.Family;
            if (memberInfo.IsPublic)
                attributes |= MemberAttributes.Public;
            if (memberInfo.IsStatic && !memberInfo.HasConstant)
                attributes |= MemberAttributes.Static;

            // TODO: Values for readonly fields are set in the ctor
            var codeTypeReference = CreateCodeTypeReference(memberInfo.FieldType);
            if (memberInfo.IsInitOnly)
                codeTypeReference = MakeReadonly(codeTypeReference);
            var field = new CodeMemberField(codeTypeReference, memberInfo.Name)
            {
                Attributes = attributes,
                CustomAttributes = CreateCustomAttributes(memberInfo)
            };

            if (memberInfo.HasConstant)
                field.InitExpression = new CodePrimitiveExpression(memberInfo.Constant);

            typeDeclaration.Members.Add(field);
        }

        private static CodeTypeReference MakeReadonly(CodeTypeReference typeReference)
        {
            return ModifyCodeTypeReference(typeReference, "readonly");
        }

        private static CodeTypeReference MakeAsync(CodeTypeReference typeReference)
        {
            return ModifyCodeTypeReference(typeReference, "async");
        }

        private static CodeTypeReference ModifyCodeTypeReference(CodeTypeReference typeReference, string modifier)
        {
            using (var provider = new CSharpCodeProvider())
                return new CodeTypeReference(modifier + " " + provider.GetTypeOutput(typeReference));
        }

        private static CodeTypeReference CreateCodeTypeReference(TypeReference type)
        {
            var typeName = GetTypeName(type);
            return new CodeTypeReference(typeName, CreateGenericArguments(type));
        }

        private static string GetTypeName(TypeReference type)
        {
            if (type.IsGenericParameter)
                return type.Name;

            if (!type.IsNested)
            {
                return (!string.IsNullOrEmpty(type.Namespace) ? (type.Namespace + ".") : "") + type.Name;
            }

            return GetTypeName(type.DeclaringType) + "." + type.Name;
        }

        private static CodeTypeReference[] CreateGenericArguments(TypeReference type)
        {
            var genericInstance = type as IGenericInstance;
            if (genericInstance == null) return null;

            var genericArguments = new List<CodeTypeReference>();
            foreach (var argument in genericInstance.GenericArguments)
            {
                genericArguments.Add(CreateCodeTypeReference(argument));
            }
            return genericArguments.ToArray();
        }
    }
}
// ReSharper restore BitwiseOperatorOnEnumWihtoutFlags
// ReSharper restore CheckNamespace
