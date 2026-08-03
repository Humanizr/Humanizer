#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;

namespace Humanizer.Docs;

public sealed class ApiAccessRecord
{
    public required string Access { get; init; }

    public required string DeclaredAccess { get; init; }

    public required string DisplayName { get; init; }

    public required string Id { get; init; }

    public required string Kind { get; init; }

    public required string Name { get; init; }

    public required string Namespace { get; init; }

    public required int ParameterCount { get; init; }

    public required string PageName { get; init; }

    public required string QualifiedDisplayName { get; init; }

    public required string TypeDisplayName { get; init; }

    public required string TypeId { get; init; }

    public required string TypeKind { get; init; }
}

public static class ApiAccessReader
{
    enum ApiAccess
    {
        Public,
        Protected
    }

    public static ApiAccessRecord[] Read(string assemblyPath)
    {
        using var stream = File.OpenRead(assemblyPath);
        using var peReader = new PEReader(stream);
        if (!peReader.HasMetadata)
            throw new InvalidDataException($"Assembly has no metadata: {assemblyPath}");

        var reader = peReader.GetMetadataReader();
        var provider = new DocumentationIdTypeProvider(reader);
        var accessCache = new Dictionary<TypeDefinitionHandle, ApiAccess?>();
        var records = new List<ApiAccessRecord>();
        foreach (var typeHandle in reader.TypeDefinitions)
        {
            var typeAccess = GetEffectiveTypeAccess(reader, typeHandle, accessCache);
            if (typeAccess is null)
                continue;

            AddTypeRecords(
                reader,
                provider,
                typeHandle,
                typeAccess.Value,
                accessCache,
                records);
        }

        var duplicate = records
            .GroupBy(record => record.Id, StringComparer.Ordinal)
            .FirstOrDefault(group => group.Count() > 1);
        if (duplicate is not null)
            throw new InvalidDataException($"Assembly API has duplicate documentation ID: {duplicate.Key}");

        var pageCollision = records
            .Where(record => record.Kind == "Type")
            .GroupBy(record => record.PageName, StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault(group => group.Count() > 1);
        if (pageCollision is not null)
            throw new InvalidDataException(
                $"Assembly API types share canonical page {pageCollision.Key}: " +
                string.Join(
                    ", ",
                    pageCollision
                        .Select(record => record.Id)
                        .OrderBy(id => id, StringComparer.Ordinal)));

        return records.OrderBy(record => record.Id, StringComparer.Ordinal).ToArray();
    }

    static void AddTypeRecords(
        MetadataReader reader,
        DocumentationIdTypeProvider provider,
        TypeDefinitionHandle typeHandle,
        ApiAccess typeAccess,
        Dictionary<TypeDefinitionHandle, ApiAccess?> accessCache,
        List<ApiAccessRecord> records)
    {
        var type = reader.GetTypeDefinition(typeHandle);
        var typeId = GetTypeName(reader, typeHandle);
        var typeDisplayName = GetTypeDisplayName(reader, typeHandle);
        var typeKind = GetTypeKind(reader, type);
        var qualifiedDisplayName = GetQualifiedDisplayName(
            reader,
            typeHandle,
            out var typeNamespace);
        var pageName = GetPageName(qualifiedDisplayName);
        var declaredTypeAccess = GetTypeAccess(type.Attributes) ?? typeAccess;
        AddRecord(
            records,
            typeAccess,
            declaredTypeAccess,
            $"T:{typeId}",
            "Type",
            typeDisplayName,
            0,
            typeId,
            typeDisplayName,
            typeKind,
            pageName,
            typeNamespace,
            qualifiedDisplayName);

        if (typeKind == "delegate")
            return;

        var explicitAccess = GetExplicitImplementationAccess(
            reader,
            type,
            accessCache);

        foreach (var fieldHandle in type.GetFields())
        {
            var field = reader.GetFieldDefinition(fieldHandle);
            var declaredAccess = GetFieldAccess(field.Attributes);
            var access = GetEffectiveAccess(typeAccess, declaredAccess);
            if (access is null || declaredAccess is null)
                continue;

            var name = reader.GetString(field.Name);
            if (typeKind == "enum" && name == "value__")
                continue;
            AddRecord(
                records,
                access.Value,
                declaredAccess.Value,
                $"F:{typeId}.{EscapeMemberName(name)}",
                "Field",
                name,
                0,
                typeId,
                typeDisplayName,
                typeKind,
                pageName,
                typeNamespace,
                qualifiedDisplayName);
        }

        var accessorHandles = new HashSet<MethodDefinitionHandle>();
        foreach (var eventHandle in type.GetEvents())
        {
            var eventDefinition = reader.GetEventDefinition(eventHandle);
            var accessors = eventDefinition.GetAccessors();
            AddAccessorHandles(
                accessorHandles,
                accessors.Adder,
                accessors.Remover,
                accessors.Raiser);
            var declaredAccess = GetAccessorAccess(
                reader,
                explicitAccess,
                accessors.Adder,
                accessors.Remover,
                accessors.Raiser);
            var access = GetEffectiveAccess(typeAccess, declaredAccess);
            if (access is null || declaredAccess is null)
                continue;

            var name = reader.GetString(eventDefinition.Name);
            AddRecord(
                records,
                access.Value,
                declaredAccess.Value,
                $"E:{typeId}.{EscapeMemberName(name)}",
                "Event",
                name,
                0,
                typeId,
                typeDisplayName,
                typeKind,
                pageName,
                typeNamespace,
                qualifiedDisplayName);
        }

        foreach (var propertyHandle in type.GetProperties())
        {
            var property = reader.GetPropertyDefinition(propertyHandle);
            var accessors = property.GetAccessors();
            AddAccessorHandles(
                accessorHandles,
                accessors.Getter,
                accessors.Setter);
            var declaredAccess = GetAccessorAccess(
                reader,
                explicitAccess,
                accessors.Getter,
                accessors.Setter);
            var access = GetEffectiveAccess(typeAccess, declaredAccess);
            if (access is null || declaredAccess is null)
                continue;

            var name = reader.GetString(property.Name);
            var signature = property.DecodeSignature(provider, genericContext: null);
            var id = $"P:{typeId}.{EscapeMemberName(name)}";
            if (signature.ParameterTypes.Length > 0)
                id += $"({string.Join(",", signature.ParameterTypes)})";

            AddRecord(
                records,
                access.Value,
                declaredAccess.Value,
                id,
                "Property",
                name,
                signature.ParameterTypes.Length,
                typeId,
                typeDisplayName,
                typeKind,
                pageName,
                typeNamespace,
                qualifiedDisplayName);
        }

        foreach (var methodHandle in type.GetMethods())
        {
            var method = reader.GetMethodDefinition(methodHandle);
            var metadataAccess = GetMethodAccess(method.Attributes);
            var declaredAccess = explicitAccess.TryGetValue(methodHandle, out var implementationAccess)
                ? implementationAccess
                : metadataAccess;
            var access = GetEffectiveAccess(typeAccess, declaredAccess);
            if (access is null || declaredAccess is null)
                continue;

            var name = reader.GetString(method.Name);
            if (accessorHandles.Contains(methodHandle))
                continue;

            var kind = name == ".ctor" || name == ".cctor"
                ? "Constructor"
                : name.StartsWith("op_", StringComparison.Ordinal)
                    ? "Operator"
                    : "Method";
            var memberName = name switch
            {
                ".ctor" => "#ctor",
                ".cctor" => "#cctor",
                _ => EscapeMemberName(name)
            };
            var genericArity = method.GetGenericParameters().Count;
            if (genericArity > 0)
                memberName += $"``{genericArity}";

            var signature = method.DecodeSignature(provider, genericContext: null);
            var id = $"M:{typeId}.{memberName}";
            if (signature.ParameterTypes.Length > 0)
                id += $"({string.Join(",", signature.ParameterTypes)})";
            if (name is
                "op_Implicit" or
                "op_Explicit" or
                "op_CheckedExplicit")
                id += $"~{signature.ReturnType}";

            AddRecord(
                records,
                access.Value,
                metadataAccess ?? declaredAccess.Value,
                id,
                kind,
                name,
                signature.ParameterTypes.Length,
                typeId,
                typeDisplayName,
                typeKind,
                pageName,
                typeNamespace,
                qualifiedDisplayName);
        }
    }

    static Dictionary<MethodDefinitionHandle, ApiAccess> GetExplicitImplementationAccess(
        MetadataReader reader,
        TypeDefinition type,
        Dictionary<TypeDefinitionHandle, ApiAccess?> accessCache)
    {
        var result = new Dictionary<MethodDefinitionHandle, ApiAccess>();
        foreach (var implementationHandle in type.GetMethodImplementations())
        {
            var implementation = reader.GetMethodImplementation(implementationHandle);
            if (implementation.MethodBody.Kind != HandleKind.MethodDefinition)
                continue;

            var access = GetDeclarationAccess(
                reader,
                implementation.MethodDeclaration,
                accessCache);
            if (access is not null)
                result[(MethodDefinitionHandle)implementation.MethodBody] = access.Value;
        }

        return result;
    }

    static ApiAccess? GetDeclarationAccess(
        MetadataReader reader,
        EntityHandle declaration,
        Dictionary<TypeDefinitionHandle, ApiAccess?> accessCache)
    {
        switch (declaration.Kind)
        {
            case HandleKind.MethodDefinition:
                var method = reader.GetMethodDefinition((MethodDefinitionHandle)declaration);
                return GetEffectiveAccess(
                    GetEffectiveTypeAccess(reader, method.GetDeclaringType(), accessCache),
                    GetMethodAccess(method.Attributes));
            case HandleKind.MemberReference:
                var member = reader.GetMemberReference((MemberReferenceHandle)declaration);
                return GetDeclarationTypeAccess(reader, member.Parent, accessCache);
            case HandleKind.MethodSpecification:
                var specification = reader.GetMethodSpecification((MethodSpecificationHandle)declaration);
                return GetDeclarationAccess(reader, specification.Method, accessCache);
            default:
                throw new NotSupportedException(
                    $"Unsupported explicit-interface declaration handle: {declaration.Kind}");
        }
    }

    static ApiAccess? GetDeclarationTypeAccess(
        MetadataReader reader,
        EntityHandle handle,
        Dictionary<TypeDefinitionHandle, ApiAccess?> accessCache)
    {
        return handle.Kind switch
        {
            HandleKind.TypeDefinition => GetEffectiveTypeAccess(
                reader,
                (TypeDefinitionHandle)handle,
                accessCache),
            HandleKind.TypeReference => ApiAccess.Public,
            HandleKind.TypeSpecification => reader.
                GetTypeSpecification((TypeSpecificationHandle)handle).
                DecodeSignature(
                    new DeclarationAccessTypeProvider(accessCache),
                    genericContext: null),
            HandleKind.MethodDefinition => GetDeclarationAccess(reader, handle, accessCache),
            _ => null
        };
    }

    static void AddRecord(
        List<ApiAccessRecord> records,
        ApiAccess access,
        ApiAccess declaredAccess,
        string id,
        string kind,
        string name,
        int parameterCount,
        string typeId,
        string typeDisplayName,
        string typeKind,
        string pageName,
        string typeNamespace,
        string qualifiedDisplayName) =>
        records.Add(new ApiAccessRecord
        {
            Access = ToText(access),
            DeclaredAccess = ToText(declaredAccess),
            DisplayName = kind switch
            {
                "Type" => typeDisplayName,
                "Constructor" => GetNonGenericName(typeDisplayName),
                _ => name
            },
            Id = id,
            Kind = kind,
            Name = name,
            Namespace = typeNamespace,
            ParameterCount = parameterCount,
            PageName = pageName,
            QualifiedDisplayName = qualifiedDisplayName,
            TypeDisplayName = typeDisplayName,
            TypeId = typeId,
            TypeKind = typeKind
        });

    static string GetNonGenericName(string displayName)
    {
        var genericStart = displayName.IndexOf('<');
        return genericStart < 0 ? displayName : displayName[..genericStart];
    }

    static string ToText(ApiAccess access) =>
        access == ApiAccess.Public ? "public" : "protected";

    static ApiAccess? GetEffectiveTypeAccess(
        MetadataReader reader,
        TypeDefinitionHandle handle,
        Dictionary<TypeDefinitionHandle, ApiAccess?> cache)
    {
        if (cache.TryGetValue(handle, out var cached))
            return cached;

        var type = reader.GetTypeDefinition(handle);
        var access = GetTypeAccess(type.Attributes);
        var declaringType = type.GetDeclaringType();
        if (!declaringType.IsNil)
            access = GetEffectiveAccess(
                GetEffectiveTypeAccess(reader, declaringType, cache),
                access);

        cache[handle] = access;
        return access;
    }

    static ApiAccess? GetEffectiveAccess(ApiAccess parent, ApiAccess? member) =>
        GetEffectiveAccess((ApiAccess?)parent, member);

    static ApiAccess? GetEffectiveAccess(ApiAccess? parent, ApiAccess? member)
    {
        if (parent is null || member is null)
            return null;
        return parent == ApiAccess.Protected || member == ApiAccess.Protected
            ? ApiAccess.Protected
            : ApiAccess.Public;
    }

    static ApiAccess? GetTypeAccess(TypeAttributes attributes) =>
        (attributes & TypeAttributes.VisibilityMask) switch
        {
            TypeAttributes.Public or TypeAttributes.NestedPublic => ApiAccess.Public,
            TypeAttributes.NestedFamily or TypeAttributes.NestedFamORAssem => ApiAccess.Protected,
            _ => null
        };

    static ApiAccess? GetFieldAccess(FieldAttributes attributes) =>
        (attributes & FieldAttributes.FieldAccessMask) switch
        {
            FieldAttributes.Public => ApiAccess.Public,
            FieldAttributes.Family or FieldAttributes.FamORAssem => ApiAccess.Protected,
            _ => null
        };

    static ApiAccess? GetMethodAccess(MethodAttributes attributes) =>
        (attributes & MethodAttributes.MemberAccessMask) switch
        {
            MethodAttributes.Public => ApiAccess.Public,
            MethodAttributes.Family or MethodAttributes.FamORAssem => ApiAccess.Protected,
            _ => null
        };

    static ApiAccess? GetAccessorAccess(
        MetadataReader reader,
        IReadOnlyDictionary<MethodDefinitionHandle, ApiAccess> explicitAccess,
        params MethodDefinitionHandle[] handles)
    {
        ApiAccess? result = null;
        foreach (var handle in handles)
        {
            if (handle.IsNil)
                continue;
            var access = explicitAccess.TryGetValue(handle, out var implementationAccess)
                ? implementationAccess
                : GetMethodAccess(reader.GetMethodDefinition(handle).Attributes);
            if (access == ApiAccess.Public)
                return access;
            if (access == ApiAccess.Protected)
                result = access;
        }
        return result;
    }

    static void AddAccessorHandles(
        HashSet<MethodDefinitionHandle> accessorHandles,
        params MethodDefinitionHandle[] handles)
    {
        foreach (var handle in handles)
        {
            if (!handle.IsNil)
                accessorHandles.Add(handle);
        }
    }

    static string GetTypeKind(MetadataReader reader, TypeDefinition type)
    {
        if ((type.Attributes & TypeAttributes.Interface) != 0)
            return "interface";

        var baseType = GetEntityTypeName(reader, type.BaseType);
        return baseType switch
        {
            "System.Enum" => "enum",
            "System.MulticastDelegate" => "delegate",
            "System.ValueType" => "struct",
            _ => "class"
        };
    }

    static string GetEntityTypeName(MetadataReader reader, EntityHandle handle) =>
        handle.Kind switch
        {
            HandleKind.TypeDefinition => GetTypeName(reader, (TypeDefinitionHandle)handle),
            HandleKind.TypeReference => GetTypeName(reader, (TypeReferenceHandle)handle),
            _ => ""
        };

    static string GetTypeName(MetadataReader reader, TypeDefinitionHandle handle)
    {
        var type = reader.GetTypeDefinition(handle);
        var name = reader.GetString(type.Name);
        var declaringType = type.GetDeclaringType();
        if (!declaringType.IsNil)
            return $"{GetTypeName(reader, declaringType)}.{name}";

        var @namespace = reader.GetString(type.Namespace);
        return string.IsNullOrEmpty(@namespace) ? name : $"{@namespace}.{name}";
    }

    static string GetTypeName(MetadataReader reader, TypeReferenceHandle handle)
    {
        var type = reader.GetTypeReference(handle);
        var name = reader.GetString(type.Name);
        if (type.ResolutionScope.Kind == HandleKind.TypeReference)
            return $"{GetTypeName(reader, (TypeReferenceHandle)type.ResolutionScope)}.{name}";

        var @namespace = reader.GetString(type.Namespace);
        return string.IsNullOrEmpty(@namespace) ? name : $"{@namespace}.{name}";
    }

    static string GetTypeDisplayName(MetadataReader reader, TypeDefinitionHandle handle)
    {
        var type = reader.GetTypeDefinition(handle);
        var metadataName = reader.GetString(type.Name);
        var tick = metadataName.LastIndexOf('`');
        if (tick < 0)
            return metadataName;

        if (!int.TryParse(metadataName[(tick + 1)..], out var arity))
            throw new InvalidDataException($"Malformed generic type name: {metadataName}");

        var parameterNames = type.GetGenericParameters()
            .Select(genericHandle => reader.GetString(reader.GetGenericParameter(genericHandle).Name))
            .TakeLast(arity);
        return $"{metadataName[..tick]}<{string.Join(",", parameterNames)}>";
    }

    static string GetQualifiedDisplayName(
        MetadataReader reader,
        TypeDefinitionHandle handle,
        out string typeNamespace)
    {
        var segments = new Stack<string>();
        var current = handle;
        string? @namespace = null;
        while (!current.IsNil)
        {
            var type = reader.GetTypeDefinition(current);
            segments.Push(GetTypeDisplayName(reader, current));
            if (type.GetDeclaringType().IsNil)
                @namespace = reader.GetString(type.Namespace);
            current = type.GetDeclaringType();
        }

        typeNamespace = @namespace ?? "";
        return string.IsNullOrEmpty(typeNamespace)
            ? string.Join(".", segments)
            : $"{typeNamespace}.{string.Join(".", segments)}";
    }

    static string GetPageName(string qualifiedDisplayName) =>
        qualifiedDisplayName
            .Replace("<", "_", StringComparison.Ordinal)
            .Replace(">", "_", StringComparison.Ordinal)
            .Replace(",", "_", StringComparison.Ordinal);

    static string EscapeMemberName(string name) =>
        name.Replace('.', '#').Replace('<', '{').Replace('>', '}');

    sealed class DeclarationAccessTypeProvider : ISignatureTypeProvider<ApiAccess?, object?>
    {
        readonly Dictionary<TypeDefinitionHandle, ApiAccess?> accessCache;

        public DeclarationAccessTypeProvider(
            Dictionary<TypeDefinitionHandle, ApiAccess?> accessCache) =>
            this.accessCache = accessCache;

        public ApiAccess? GetArrayType(ApiAccess? elementType, ArrayShape shape) => elementType;

        public ApiAccess? GetByReferenceType(ApiAccess? elementType) => elementType;

        public ApiAccess? GetFunctionPointerType(MethodSignature<ApiAccess?> signature)
        {
            var access = signature.ReturnType;
            foreach (var parameterType in signature.ParameterTypes)
                access = GetEffectiveAccess(access, parameterType);
            return access;
        }

        public ApiAccess? GetGenericInstantiation(
            ApiAccess? genericType,
            ImmutableArray<ApiAccess?> typeArguments) => genericType;

        public ApiAccess? GetGenericMethodParameter(object? genericContext, int index) =>
            ApiAccess.Public;

        public ApiAccess? GetGenericTypeParameter(object? genericContext, int index) =>
            ApiAccess.Public;

        public ApiAccess? GetModifiedType(
            ApiAccess? modifier,
            ApiAccess? unmodifiedType,
            bool isRequired) => unmodifiedType;

        public ApiAccess? GetPinnedType(ApiAccess? elementType) => elementType;

        public ApiAccess? GetPointerType(ApiAccess? elementType) => elementType;

        public ApiAccess? GetPrimitiveType(PrimitiveTypeCode typeCode) => ApiAccess.Public;

        public ApiAccess? GetSZArrayType(ApiAccess? elementType) => elementType;

        public ApiAccess? GetTypeFromDefinition(
            MetadataReader metadataReader,
            TypeDefinitionHandle handle,
            byte rawTypeKind) => GetEffectiveTypeAccess(
                metadataReader,
                handle,
                accessCache);

        public ApiAccess? GetTypeFromReference(
            MetadataReader metadataReader,
            TypeReferenceHandle handle,
            byte rawTypeKind) => ApiAccess.Public;

        public ApiAccess? GetTypeFromSpecification(
            MetadataReader metadataReader,
            object? genericContext,
            TypeSpecificationHandle handle,
            byte rawTypeKind) => GetDeclarationTypeAccess(
                metadataReader,
                handle,
                accessCache);
    }

    sealed class DocumentationIdTypeProvider : ISignatureTypeProvider<string, object?>
    {
        readonly MetadataReader reader;

        public DocumentationIdTypeProvider(MetadataReader reader) => this.reader = reader;

        public string GetArrayType(string elementType, ArrayShape shape)
        {
            var bounds = new string[shape.Rank];
            Array.Fill(bounds, "0:");
            return $"{elementType}[{string.Join(",", bounds)}]";
        }

        public string GetByReferenceType(string elementType) => $"{elementType}@";

        public string GetFunctionPointerType(MethodSignature<string> signature) =>
            throw new NotSupportedException(
                "Public API function-pointer documentation IDs are not supported.");

        public string GetGenericInstantiation(
            string genericType,
            ImmutableArray<string> typeArguments)
        {
            var argumentIndex = 0;
            var encodedType = Regex.Replace(
                genericType,
                "`(?<arity>[0-9]+)",
                match =>
                {
                    var arity = int.Parse(match.Groups["arity"].Value);
                    if (argumentIndex + arity > typeArguments.Length)
                        throw new InvalidDataException(
                            $"Generic type arity exceeds its arguments: {genericType}");

                    var arguments = typeArguments
                        .Skip(argumentIndex)
                        .Take(arity);
                    argumentIndex += arity;
                    return $"{{{string.Join(",", arguments)}}}";
                });
            if (argumentIndex != typeArguments.Length)
                throw new InvalidDataException(
                    $"Generic type arguments exceed its arity: {genericType}");
            return encodedType;
        }

        public string GetGenericMethodParameter(object? genericContext, int index) => $"``{index}";

        public string GetGenericTypeParameter(object? genericContext, int index) => $"`{index}";

        public string GetModifiedType(string modifier, string unmodifiedType, bool isRequired) =>
            unmodifiedType;

        public string GetPinnedType(string elementType) => elementType;

        public string GetPointerType(string elementType) => $"{elementType}*";

        public string GetPrimitiveType(PrimitiveTypeCode typeCode) => typeCode switch
        {
            PrimitiveTypeCode.Boolean => "System.Boolean",
            PrimitiveTypeCode.Byte => "System.Byte",
            PrimitiveTypeCode.Char => "System.Char",
            PrimitiveTypeCode.Double => "System.Double",
            PrimitiveTypeCode.Int16 => "System.Int16",
            PrimitiveTypeCode.Int32 => "System.Int32",
            PrimitiveTypeCode.Int64 => "System.Int64",
            PrimitiveTypeCode.IntPtr => "System.IntPtr",
            PrimitiveTypeCode.Object => "System.Object",
            PrimitiveTypeCode.SByte => "System.SByte",
            PrimitiveTypeCode.Single => "System.Single",
            PrimitiveTypeCode.String => "System.String",
            PrimitiveTypeCode.TypedReference => "System.TypedReference",
            PrimitiveTypeCode.UInt16 => "System.UInt16",
            PrimitiveTypeCode.UInt32 => "System.UInt32",
            PrimitiveTypeCode.UInt64 => "System.UInt64",
            PrimitiveTypeCode.UIntPtr => "System.UIntPtr",
            PrimitiveTypeCode.Void => "System.Void",
            _ => throw new NotSupportedException($"Unsupported primitive type: {typeCode}")
        };

        public string GetSZArrayType(string elementType) => $"{elementType}[]";

        public string GetTypeFromDefinition(
            MetadataReader metadataReader,
            TypeDefinitionHandle handle,
            byte rawTypeKind) => GetTypeName(metadataReader, handle);

        public string GetTypeFromReference(
            MetadataReader metadataReader,
            TypeReferenceHandle handle,
            byte rawTypeKind) => GetTypeName(metadataReader, handle);

        public string GetTypeFromSpecification(
            MetadataReader metadataReader,
            object? genericContext,
            TypeSpecificationHandle handle,
            byte rawTypeKind) =>
            metadataReader.GetTypeSpecification(handle).DecodeSignature(this, genericContext);
    }
}
