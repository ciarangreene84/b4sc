# b4sc
Boot 4 Service Collection

Use in a cs file 
```
using Boot4ServiceCollection.Attributes;
[assembly: EnableBoot4ServiceCollection()]
```
Or add the below to your csproj file
```
<ItemGroup>
  <AssemblyAttribute Include="Boot4ServiceCollection.EnableBoot4ServiceCollectionAttribute">
  </AssemblyAttribute>
</ItemGroup>
```
