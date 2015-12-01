using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace HQF.Tutorial.WebAPI.Filters.Exceptions
{
    public class DemoExceptionHandler : IExceptionHandler
    {
        public virtual Task HandleAsync(ExceptionHandlerContext context,
            CancellationToken cancellationToken)
        {
            if (!ShouldHandle(context))
            {
                return Task.FromResult(0);
            }

            return HandleAsyncCore(context, cancellationToken);
        }

        public virtual Task HandleAsyncCore(ExceptionHandlerContext context,
            CancellationToken cancellationToken)
        {
            HandleCore(context);
            return Task.FromResult(0);
        }

        public virtual void HandleCore(ExceptionHandlerContext context)
        {
        }

        public virtual bool ShouldHandle(ExceptionHandlerContext context)
        {
            //WebAPI v2 does not use IsOutermostCatchBlock anymore
            //IsOutermostCatchBlock does not exists. Use CatchBlock.IsTopLevel instead:
            //http://stackoverflow.com/a/22357634/1616023
            return context.ExceptionContext.CatchBlock.IsTopLevel;
        }
    }
}