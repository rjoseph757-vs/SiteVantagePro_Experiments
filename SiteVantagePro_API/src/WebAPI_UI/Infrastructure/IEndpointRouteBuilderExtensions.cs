namespace SiteVantagePro_API.WebAPI_UI.Infrastructure;

public static class IEndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapGet(this IEndpointRouteBuilder builder, Delegate handler, string pattern = "", bool excludeFromDescription = false)
    {
        Guard.Against.AnonymousMethod(handler);

        if (excludeFromDescription)
        {
            builder.MapGet(pattern, handler)
                .WithName(handler.Method.Name)
                .ExcludeFromDescription();

            return builder;
        }

        builder.MapGet(pattern, handler)
            .WithName(handler.Method.Name);

        return builder;
    }

    public static IEndpointRouteBuilder MapPost(this IEndpointRouteBuilder builder, Delegate handler, string pattern = "", bool excludeFromDescription = false)
    {
        Guard.Against.AnonymousMethod(handler);

        if (excludeFromDescription)
        {
            builder.MapPost(pattern, handler)
                .WithName(handler.Method.Name)
                .ExcludeFromDescription();

            return builder;
        }

        builder.MapPost(pattern, handler)
        .WithName(handler.Method.Name);

        return builder;
    }

    public static IEndpointRouteBuilder MapPut(this IEndpointRouteBuilder builder, Delegate handler, string pattern, bool excludeFromDescription = false)
    {
        Guard.Against.AnonymousMethod(handler);

        if (excludeFromDescription)
        {
            builder.MapPut(pattern, handler)
                .WithName(handler.Method.Name)
                .ExcludeFromDescription();

            return builder;
        }

        builder.MapPut(pattern, handler)
             .WithName(handler.Method.Name);

        return builder;
    }

    public static IEndpointRouteBuilder MapDelete(this IEndpointRouteBuilder builder, Delegate handler, string pattern, bool excludeFromDescription = false)
    {
        Guard.Against.AnonymousMethod(handler);

        if (excludeFromDescription)
        {
            builder.MapDelete(pattern, handler)
                .WithName(handler.Method.Name)
                .ExcludeFromDescription();

            return builder;
        }

        builder.MapDelete(pattern, handler)
            .WithName(handler.Method.Name);

        return builder;
    }
}
