# Boot 4 Service Collection
Attribute-based injection for Service Collection

A small library to re-jig how classes are added to the ServiceCollection. This idea is kinda sorta stolen from Java (Spring Framework). 

*Note 2019-07-16*: I discovered that AutoMapper introduced attribute-based mapping in their version 8.1.0 release in April 2019 (https://github.com/AutoMapper/AutoMapper/releases/tag/v8.1.0). B4SC is essentially attribute-based injection. So... wave of the future?

## Problem

Writing implementations against interfaces and adding them to the ServiceCollection leads to a couple of minor side effects that I personally dislike. For example, in an implementation project, one might see:
```
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAssureCoreDataAccessLayer(this IServiceCollection services)
        {   
            services.AddScoped<PageSortValidator, PageSortValidator>();
            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

            services.AddScoped<IClaimsDbContext, ClaimsDbContext>();
            services.AddScoped<IDocumentsDbContext, DocumentsDbContext>();
            services.AddScoped<ICompaniesDbContext, CompaniesDbContext>();
            services.AddScoped<ILeadsDbContext, LeadsDbContext>();
            services.AddScoped<IProductsDbContext, ProductDbContext>();
            services.AddScoped<IAccountDbContext, AccountDbContext>();
            services.AddScoped<IAgentsDbContext, AgentDbContext>();
            services.AddScoped<ICardDbContext, CardDbContext>();
            services.AddScoped<IInvoiceDbContext, InvoiceDbContext>();
            etc...

            return services;
        }
    }
```

If there are multiple implementation libraries, each one may end up having such an extension method in each library. Then, when using the implementation(s), one might have a chain of calls to the extension methods on the ServiceCollection itself, e.g.

```
...
services.AddAssureCoreDataAccessLayer();
services.AddAssureCoreRepositoryLayer();
services.AddAssureCoreUtilities();
services.AddAssureCoreIdentityLayer();
etc...
```

I *personally* dislike this.

## Solution

I prefer not to have ServiceCollection extension methods in each implementation library, or to have extension methods written further up the stack by the consumer. I feel both approaches muddy up the code and ruin my day. Instead, the Boot4ServiceCollection introduces a number of class Attributes, specifically AddScopedAttribute, AddSingletonAttribute and AddTransientAttribute which correspond to the serviceCollection.Add...() methods.

Instead of having a ServiceCollection extension method in each implementation library, an appropriate attribute (From Boot4ServiceCollection.Attributes package) can be added to each *class*, e.g.

```
    [AddSingleton(typeof(IDbConnectionFactory))]
    public class DbConnectionFactory : IDbConnectionFactory
    {
      ...
    }
    
    ...
    
    [AddScoped(typeof(IAccountDbContext))]
    public class AccountDbContext : IAccountDbContext
    {
        ...
    }
    
    ...
    
    [AddTransient(typeof(IDocumentsDbContext))]
    public class DocumentsDbContext : IDocumentsDbContext
    {
        ...
    }

```

### Consuming

The consuming project can then just run (from Boot4ServiceCollection package):
```
  ...
  services.Boot();
  etc...
```

## How It Works

The services.Boot() method in the Boot4ServiceCollection package loops through all of the libraries in the current (or specified) directory. The method then loops through all of the exported types where an AddScopedAttribute, AddSingletonAttribute or AddTransientAttribute has been set on the class and calls the appropriate method on the ServiceCollection.

## Final Notes

This is an opinionated approach to tidying up side effects which I personally dislike. I am aware of alternative IoC containers like AutoFac. I personally prefer doing it this way. There are certainly downsides with this approach. This project does not reduce the overall number of lines that need to be written, simply reorganises the placement of those lines in a way I prefer. #WorksForMe #NoWarranty

